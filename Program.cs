using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using var host = CreateDefaultHost(args).Build();
            var service = ActivatorUtilities.CreateInstance<Startup>(host.Services);
            service.Run(args);
        }

        static IHostBuilder CreateDefaultHost(string[] args) => Host.CreateDefaultBuilder(args);
    }
}
