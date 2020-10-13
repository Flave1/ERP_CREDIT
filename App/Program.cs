using Banking.Handlers.Deposit;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting; 

namespace Banking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().SeedData().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>  WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}
