using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace TradeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            int depthBound = 3;
            int frontierBoundary = 100;

            foreach(string arg in args)
            {
                switch(arg)
                {
                    case string d when d.Contains("--depth="):
                        depthBound = int.Parse(Regex.Match(d, @"\d+").Value);
                        break;
                    case string f when f.Contains("--frontier-boundary="):
                        frontierBoundary = int.Parse(Regex.Match(f, @"\d+").Value);
                        break;
                    default:
                        throw new ArgumentException(arg);
                }
            }

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
            scheduleGenerator.GameScheduler(depthBound, frontierBoundary);
        }
    }
}