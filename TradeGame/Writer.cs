using System.IO.Abstractions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TradeGame
{
    internal class Writer : IWriter
    {
        private IFileSystem fileSystem;

        public Writer(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void WriteSchedules(PriorityQueue<Schedule, double> schedules)
        {
            IList<Schedule> formattedSchedules = new List<Schedule>();
            while (schedules.Count > 0)
            {
                formattedSchedules.Add(schedules.Dequeue());
            }

            string outputFileName = "output_schedules.json";
            string jsonString = JsonSerializer.Serialize(formattedSchedules,
                new JsonSerializerOptions() { WriteIndented = true });
            fileSystem.File.WriteAllText(outputFileName, jsonString);
        }
    }
}
