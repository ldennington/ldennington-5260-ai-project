using System.IO.Abstractions;
using System.Text.Json;

namespace TradeGame
{
    internal class Writer : IWriter
    {
        private IFileSystem fileSystem;
        private IEnv env;

        public Writer(IFileSystem fileSystem, IEnv env)
        {
            this.fileSystem = fileSystem;
            this.env = env;
        }

        public void WriteSchedules()
        {
            IList<Schedule> schedules = new List<Schedule>();
            while (Global.Solutions.Count > 0)
            {
                schedules.Add(Global.Solutions.Dequeue());
            }

            string outputFileName = "output-schedules.json";
            fileSystem.File.WriteAllText(Path.Combine(env.Get("TEMP"), outputFileName), JsonSerializer.Serialize(schedules,
                new JsonSerializerOptions() { WriteIndented = true }));
        }

        public void WritePredictedAndActualEUs(double predicted, double actual)
        {
            string outputFileName = "predicted-and-actual.csv";
            string predictedAndActual = $"{predicted},{actual}\n";
            string outputFilePath = Path.Combine(env.Get("TEMP"), outputFileName);

            if (!fileSystem.File.Exists(outputFilePath))
            {
                string header = "Predicted,Actual\n";
                fileSystem.File.WriteAllText(outputFilePath, header);
            }

            fileSystem.File.AppendAllText(outputFilePath, predictedAndActual);
        }
    }
}
