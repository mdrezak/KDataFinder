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

        public string GetTableDetialAddress(TableRow row) => row.Columns[options.OriginColumnIndex].ToString()!.Split("|||")[options.OriginColumnDataIndex];

        public async Task<List<object>> Obtain(TableRow row)
        {
            List<object?> Result = new();
            var document = new HtmlDocument();
            string currentPageUrl = GetTableDetialAddress(row);
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
                            throw new NotImplementedException("Continued indexed list does not support for ImageToTextAsync objectives.");
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

                    //try //OCR روی سیستم من کار نمیکنه به علت RAM و ....
                    //{
                    var img = document.DocumentNode.QuerySelector(objective.TargetElement);
                    var imageSource = img?.GetAttributeValue("src", null);
                    if (imageSource == null) goto imageCanNotProccess;
                    Uri uri = new Uri(currentPageUrl);
                    var imageUri = $"{uri.Scheme}://{uri.Host}/{imageSource}";
                    var image = await _downloadService.Download(imageUri);
                    if (image == null) goto imageCanNotProccess;
                    string text = await _imageToTextService.ImageToTextAsync(await image.ReadAsByteArrayAsync());
                    Result.Add(text);
                //}
                //catch { }
                imageCanNotProccess: Result.Add("");
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
            return Result.ToList()!;
        }
    }
}
