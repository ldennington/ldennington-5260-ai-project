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

        /* The ReadResources method was adapted from Jeff Baranski's Python file parser
        at https://gist.github.com/jbaranski/209d475c21fe0459c2499ed606cfad9b */
        public void ReadResources(string path)
        {
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

        /* The ReadCountries method adapted from Jeff Baranski's Python file parser
        at https://gist.github.com/jbaranski/209d475c21fe0459c2499ed606cfad9b */
        public void ReadCountries(string path)
        {
            using var reader = new StreamReader(fileSystem.File.OpenRead(path));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var record = new Country
                {
                    Name = csv.GetField("Country"),
                    IsSelf = bool.Parse(csv.GetField("Self")),
                    State = new Dictionary<string, int>()
                };
                
                foreach(string resource in csv.HeaderRecord)
                {
                    if (resource.Equals("Country") || resource.Equals("Self"))
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
