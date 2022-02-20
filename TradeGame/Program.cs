using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

namespace TradeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // set up DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ICalculator, Calculator>()
                .AddSingleton<IReader, Reader>()
                .AddSingleton<IWriter, Writer>()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<IEnv, Env>()
                .AddSingleton<Action, TransferTemplate>()
                .AddSingleton<Action, TransformTemplate>()
                .AddSingleton<ScheduleGenerator>()
                .BuildServiceProvider();

            var scheduleGenerator = serviceProvider.GetService<ScheduleGenerator>();
            scheduleGenerator.Execute();
        }
    }
}