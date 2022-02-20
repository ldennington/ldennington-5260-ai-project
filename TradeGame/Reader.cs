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

        /* adapted from Jeff Baranski's Python file parser
        at https://gist.github.com/jbaranski/209d475c21fe0459c2499ed606cfad9b */
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

        /* adapted from Jeff Baranski's Python file parser
        at https://gist.github.com/jbaranski/209d475c21fe0459c2499ed606cfad9b  */
        public void ReadCountries(string path)
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

                Global.InitialState.Add(record);
            }
        }

        public void ReadTransformTemplates(string path)
        {
            using var reader = new StreamReader(fileSystem.File.OpenRead(path));
            Global.TransformTemplates = JsonSerializer.Deserialize<IList<TransformTemplate>>(reader.ReadToEnd());
        }
    }
}
