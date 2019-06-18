using System;
using Argus.Core.Application;
using Argus.Core.Data;
using Argus.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Argus.ConsoleApp
{
	public class Program
	{
		static void Main(string[] args)
		{
			var host = ConfigureHost();

			/* NOTE: this is not recommended! (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2#recommendations)
			 * Microsoft recommends separating the console application's core logic into a separate class
			 * to make use of the built-in dependency injection and access services that way.
			 */
			var applicationService = host.Services.GetService<IApplicationService>();

			/* This is another way for configuring a console application with DI.
			 * (https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.commandlineutils.commandlineapplication?view=aspnetcore-1.1)
			 * Note that the docs have not been updated for .NET Core 2.2. Some notes by Scott Allen:
			 * https://odetocode.com/blogs/scott/archive/2018/08/16/three-tips-for-console-applications-in-net-core.aspx
			 */
			//var app = new CommandLineApplication<Application>();
			//app.Conventions
			//	.UseDefaultConventions()
			//	.UseConstructorInjection(host.Services);
			// app.Execute(args);

			var applications = applicationService.GetApplications();
			foreach(var application in applications)
			{
				Console.WriteLine(application.Name);
			}

			Console.WriteLine($"{Environment.NewLine}Press any key to continue.");
			Console.Beep();
			Console.ReadKey();
		}

		/// <summary>
		/// Sets up the host and configures its services, logging, etc., for dependency injection.
		/// </summary>
		/// <returns></returns>
		private static IHost ConfigureHost()
		{
			var host = new HostBuilder()
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config
						//.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
						.Build();
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.Configure<HostOptions>(option =>
					{
						option.ShutdownTimeout = TimeSpan.FromSeconds(20);
					});

					services.AddLogging(loggingBuilder =>
					{
						loggingBuilder.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
						loggingBuilder.AddConsole();
						//loggingBuilder.AddConsole(options =>
						//{
						//	options.IncludeScopes = true;
						//});
					});

					services.AddScoped<IGenericRepository<Application>, GenericRepository<Application>>();
					services.AddScoped<IApplicationService, ApplicationService>();
				})
				.Build();
			return host;
		}
	}
}
