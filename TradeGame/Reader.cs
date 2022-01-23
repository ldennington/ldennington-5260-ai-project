using CsvHelper;
using CsvHelper.Configuration;
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

        public IDictionary<string, Resource> ReadResources(string path)
        {
            IDictionary<string, Resource> resources = new Dictionary<string, Resource>();

            using var reader = new StreamReader(fileSystem.File.OpenRead(path));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            // CSV Reader syntax for enumerating rows
            var record = new Resource();
            var records = csv.EnumerateRecords(record);
            foreach (var r in records)
            {
                resources.Add(r.Name, r);
            }

            return resources;
        }

        public IList<Country> ReadCountries(string path)
        {
            IList<Country> countries = new List<Country>();

            using var reader = new StreamReader(fileSystem.File.OpenRead(path));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var record = new Country
                {
                    Name = csv.GetField("Country"),
                    State = new Dictionary<string, int>()
                };
                
                foreach(string resource in csv.HeaderRecord)
                {
                    if (resource == "Country")
                        continue;
                    record.State.Add(resource, int.Parse(csv.GetField(resource)));
                }

                countries.Add(record);
            }

            return countries;
        }
    }
}
