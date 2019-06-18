using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Argus.MVC
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
						.AddJsonFile("secrets.json", optional: false, reloadOnChange: true)
						.Build();
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.ClearProviders();
					logging.AddConsole();
					// logging.AddConsole(options => options.IncludeScopes = true); // Allows use of "using" blocks (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?tabs=aspnetcore2x&view=aspnetcore-2.2#log-scopes)
					logging.AddEventLog(); // Logs to the Windows Event log. (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?tabs=aspnetcore2x&view=aspnetcore-2.2#windows-eventlog-provider)
				})
				.UseStartup<Startup>();
	}
}
