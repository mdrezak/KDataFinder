using IronOcr;
using KDataFinder.ConsoleApp.Abstraction;

namespace KDataFinder.ConsoleApp.Implementation
{
    internal class ImageToTextSerivce : IImageToTextService
    {
        private readonly IronTesseract _ironTesseract;

        public ImageToTextSerivce()
        {
            _ironTesseract = new IronTesseract()
            {
                Language = OcrLanguage.Persian,
            };
            //_ironTesseract.Configuration.EngineMode = TesseractEngineMode.LstmOnly;
            _ironTesseract.Configuration.WhiteListCharacters = "۰۱۲۳۴۵۶۷۸۹";
        }

        public async Task<string> ImageToTextAsync(byte[] bytes)
        {
            using (var Input = new OcrInput(bytes))
                return (await _ironTesseract.ReadAsync(Input)).Text;
        }

        public async Task<string> ImageToTextAsync(string path)
        {
            using (var Input = new OcrInput(path))
                return (await _ironTesseract.ReadAsync(Input)).Text;
        }
    }
}
