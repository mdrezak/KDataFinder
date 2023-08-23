using HtmlAgilityPack;
using KDataFinder.ConsoleApp.Abstraction;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KDataFinder.ConsoleApp.Implementation.HtmlAgilityPack
{
    internal class TableRowDetialObtainer : BaseService<TableRowDetialObtainer, TableObtainerOptions>, ITableRowDetialObtainer
    {
        private readonly DetailObtainerOptions options;
        private readonly IImageToTextService _imageToTextService;
        private readonly IDownloadService _downloadService;
        public TableRowDetialObtainer(ILogger<TableRowDetialObtainer> logger, IOptions<TableObtainerOptions> option, IImageToTextService imageToTextService, IDownloadService downloadService) : base(logger, option)
        {
            this.options = option.Value.DetailObtainerOptions;
            _imageToTextService = imageToTextService;
            _downloadService = downloadService;
        }

        public async Task<List<object>> Obtain(TableRow row)
        {
            List<object?> Result = new();
            var document = new HtmlDocument();
            string currentPageUrl = row.Columns[options.OriginColumnIndex].ToString()!.Split("|||")[options.OriginColumnDataIndex];
            document.LoadHtml(
               await _downloadService.DownloadAsString(currentPageUrl)
            );
            for (int i = 0; i < options.Objectives.Length; i++)
            {
                var objective = options.Objectives[i];
                if (objective.IsContinuedIndexedList)
                {
                    for (int y = 0; y < objective.Count; y++)
                    {
                        if (objective.ImageToText)
                            throw new NotImplementedException("Continued indexed list does not support for ImageToText objectives.");
                        Result.Add(
                            string.IsNullOrEmpty(objective.TargetAttribute) ?
                            document.DocumentNode.QuerySelector(objective.TargetElement.Replace("{0}", y.ToString()))?.InnerText
                            :
                            document.DocumentNode.QuerySelector(objective.TargetElement.Replace("{0}", y.ToString()))?.GetAttributeValue(objective.TargetAttribute, "")
                        );
                    }
                }
                else if (objective.ImageToText)
                {
                    var img = document.DocumentNode.QuerySelector(objective.TargetElement);
                    if (img == null) continue;
                    var imageSource = img.GetAttributeValue("src", null);
                    if (imageSource == null) continue;
                    Uri uri = new Uri(currentPageUrl);
                    var imageUri = $"{uri.Scheme}://{uri.Host}/{imageSource}";
                    var image = await _downloadService.Download(imageUri);
                    if (image == null) continue;
                    string path = $"{Thread.CurrentThread.ManagedThreadId}-{i}-maybe.png";
                    using (var fs = File.Create(path))
                    {
                        await image.CopyToAsync(fs);
                        fs.Close();
                    }
                    //[!TODO] fix OCR bug 
                    string text = await _imageToTextService.ImageToText(path);
                    string text2 = await _imageToTextService.ImageToText(await image.ReadAsByteArrayAsync());
                    File.Delete(path);
                    Result.Add(text);
                }
                else
                {
                    Result.Add(
                        string.IsNullOrEmpty(objective.TargetAttribute) ?
                        document.DocumentNode.QuerySelector(objective.TargetElement)?.InnerText
                        :
                        document.DocumentNode.QuerySelector(objective.TargetElement)?.GetAttributeValue(objective.TargetAttribute, "")
                    );
                }
            }
            return Result.Where(x => x != null).ToList()!;
        }
    }
}
