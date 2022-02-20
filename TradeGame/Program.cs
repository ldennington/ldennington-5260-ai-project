using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TradeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            host.RunAsync();
        }

        // set up service container for DI and register services
        static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddSingleton<Action, TransferTemplate>()
                            .AddSingleton<Action, TransformTemplate>()
                            .AddSingleton<ICalculator, Calculator>()
                            .AddSingleton<IReader, Reader>()
                            .AddSingleton<IWriter, Writer>());
    }
}