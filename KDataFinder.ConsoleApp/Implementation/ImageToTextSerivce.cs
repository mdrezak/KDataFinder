using IronOcr;
using KDataFinder.ConsoleApp.Abstraction;

namespace KDataFinder.ConsoleApp.Implementation
{
    internal class ImageToTextSerivce : IImageToTextService
    {
        private readonly IronTesseract _ironTesseract;

        public ImageToTextSerivce()
        {
            _ironTesseract = new IronTesseract();
            _ironTesseract.Language = OcrLanguage.Arabic; //read from appsetting.json
        }

        public async Task<string> ImageToText(byte[] bytes)
        {
            using (var Input = new OcrInput(bytes))
                return (await _ironTesseract.ReadAsync(Input)).Text;
        }

        public async Task<string> ImageToText(string path)
        {
            using (var Input = new OcrInput(path))
                return (await _ironTesseract.ReadAsync(Input)).Text;
        }
    }
}
