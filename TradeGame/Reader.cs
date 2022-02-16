using CsvHelper;
using System.Globalization;
using System.IO.Abstractions;
using System.Text.Json;

namespace TradeGame
{
    internal class Reader : IReader
    {
        private IFileSystem fileSystem;

        public Reader(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void ReadResources(string path)
        {
            IDictionary<string, Resource> resources = new Dictionary<string, Resource>();

            using var reader = new StreamReader(fileSystem.File.OpenRead(path));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var record = new Resource
                {
                    Name = csv.GetField("Resource"),
                    Weight = double.Parse(csv.GetField("Weight")),
                    Notes = csv.GetField("Notes")
                };
                Global.Resources.Add(record.Name, record);
            }
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

        public IList<TransformTemplate> ReadTransformTemplates(string path)
        {
            using var reader = new StreamReader(fileSystem.File.OpenRead(path));
            return JsonSerializer.Deserialize<IList<TransformTemplate>>(reader.ReadToEnd());
        }
    }
}
