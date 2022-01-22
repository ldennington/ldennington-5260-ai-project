using CsvHelper;
using System.Globalization;
using System.IO.Abstractions;

namespace TradeGame
{
    internal class Reader : IReader
    {
        private IFileSystem fileSystem;

        public Reader(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public IList<Resource> ReadCsv(string path)
        {
            IEnumerable<Resource> resources = new List<Resource>();
            using var reader = new StreamReader(fileSystem.File.OpenRead(path));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Resource>().ToList();
        }
    }
}
