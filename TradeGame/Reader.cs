using CsvHelper;
using System.Globalization;
using System.IO.Abstractions;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                resources.Add(record.Name, record);
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

        public IList<TransformTemplate> ReadTransformTemplates(string path)
        {
            using var reader = new StreamReader(fileSystem.File.OpenRead(path));
            JsonSerializerOptions options = new()
            {
                
            };
            return JsonSerializer.Deserialize<IList<TransformTemplate>>(reader.ReadToEnd());
        }
    }

    internal class ResourceConverter : JsonConverter<IDictionary<Resource, int>>
    {
        public override IDictionary<Resource, int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            IDictionary<Resource, int> resourcesAndAmounts = new Dictionary<Resource, int>();
            string currentResource = "";

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                    case JsonTokenType.EndObject:
                        return resourcesAndAmounts;
                    default:
                        var input = reader.GetString();
                        if (!int.TryParse(input, out int result))
                        {
                            currentResource = input;
                            resourcesAndAmounts.Add(new Resource() { Name = currentResource }, 0);
                        }
                        else
                        {
                            var current = resourcesAndAmounts.Where(x => x.Key.Name == currentResource).FirstOrDefault().Key;
                            resourcesAndAmounts[current] = result;
                        }
                        break;
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<Resource, int> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
