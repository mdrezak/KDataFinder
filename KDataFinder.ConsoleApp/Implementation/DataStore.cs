using KDataFinder.ConsoleApp.Abstraction;
using Microsoft.Extensions.Options;
using System.Text;

namespace KDataFinder.ConsoleApp.Implementation
{
    internal class DataStore : IDataStore
    {
        private readonly DataStoreOptions _options;

        public DataStore(IOptions<DataStoreOptions> options)
        {
            _options = options.Value;
        }

        public IOperationResult SaveObjects(IEnumerable<object> objects) => _options.OutputType switch
        {
            DataStoreOptions.OutputTypes.Csv => SaveObjectsToCsvFile(objects, GenerateAndValidatePath(objects) + ".csv"),
            _ => throw new NotImplementedException(),
        };
        public string GenerateAndValidatePath(IEnumerable<object> objects)
        {
            var array = objects.ToArray();
            StringBuilder sb = new StringBuilder("./Store/");
            foreach (var index in _options.Grouping.Where((x, y) => y + 1 != _options.Grouping.Length))
                sb.Append($"{array[index]}/");
            if (!Directory.Exists(sb.ToString()))
                Directory.CreateDirectory(sb.ToString());
            return sb.Append(array[_options.Grouping.Last()]).ToString();
        }
        public IOperationResult SaveObjectsToCsvFile(IEnumerable<object> objects, string path)
        {
            string csvRow = string.Join(", ", objects);
            while (csvRow.Contains("\n") || csvRow.Contains("\r") || csvRow.Contains($"{Environment.NewLine}"))
            {
                csvRow = csvRow.Replace("\n", "");
                csvRow = csvRow.Replace("\r", "");
                csvRow = csvRow.Replace($"{Environment.NewLine}", "");
            }
            File.AppendAllLines(path, new string[]
            {
                csvRow
            });
            return OperationResult.Succeeded();
        }
    }
}
