
namespace KDataFinder.ConsoleApp.Abstraction;

internal interface IImageToTextService
{
    Task<string> ImageToText(byte[] bytes);
    Task<string> ImageToText(string path);
}
