using KDataFinder.ConsoleApp.Abstraction;
using Microsoft.Extensions.Options;
using Tesseract;
using IronOcr;
using System.Drawing;

namespace KDataFinder.ConsoleApp.Implementation;

#nullable disable
internal class ImageToTextOptions
{
    public EngineMode EngineMode { get; set; }
    public string DataPath { get; set; }
    public string Language { get; set; }
}
#nullable restore
internal class ImageToTextSerivce1 : IImageToTextService, IDisposable
{

    private readonly ImageToTextOptions _options;

    public ImageToTextSerivce1(IOptions<ImageToTextOptions> options)
    {
        _options = options.Value;
    }
    public void Dispose()
    { }

    public Task<string> ImageToTextAsync(byte[] bytes)
    {
        using (var engine = new TesseractEngine(_options.DataPath, _options.Language, _options.EngineMode))
        {
            using (var img = Pix.LoadFromMemory(bytes))
            {
                using (var page = engine.Process(img))
                {
                    return Task.FromResult(page.GetText());
                }
            }
        }
    }

    public Task<string> ImageToTextAsync(string path)
    {
        using (var engine = new TesseractEngine(_options.DataPath, _options.Language, _options.EngineMode))
        using (var img = Pix.LoadFromFile(path))
        using (var page = engine.Process(img))
            return Task.FromResult(page.GetText());
    }
}

internal class ImageToTextSerivce0 : IImageToTextService
{
    private readonly IronTesseract _ironTesseract;
    private readonly ImageToTextOptions _options;

    public ImageToTextSerivce0(IOptions<ImageToTextOptions> options)
    {
        _options = options.Value;
        _ironTesseract = new IronTesseract()
        {
            Language = OcrLanguage.Persian,
        };
        //_ironTesseract.Configuration.EngineMode = TesseractEngineMode.LstmOnly;
        _ironTesseract.Configuration.WhiteListCharacters = "۰۱۲۳۴۵۶۷۸۹";
    }

    public async Task<string> ImageToTextAsync(byte[] bytes)
    {
        Console.WriteLine("ImageToTextAsync `Called(ImageToTextSerivce0)");
        using (var Input = new OcrInput(bytes))
            return (await _ironTesseract.ReadAsync(Input)).Text;
    }

    public async Task<string> ImageToTextAsync(string path)
    {
        using (var Input = new OcrInput(path))
            return (await _ironTesseract.ReadAsync(Input)).Text;
    }
}

