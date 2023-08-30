
namespace KDataFinder.ConsoleApp.Abstraction;

internal interface IImageToTextService
{
    Task<string> ImageToTextAsync(byte[] bytes);
    Task<string> ImageToTextAsync(string path);
}
