using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Argus.MVC.Middleware
{
	/// <summary>
	/// Middleware component to write exceptions to a log file.
	/// </summary>
	/// <remarks>
	///		This class will write to file any application errors that occur during
	///		the HTTP request.
	/// </remarks>
	public class ErrorLogging
	{
		private IConfiguration _config;
		private readonly RequestDelegate _next;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="configuration">The application configuration collection. (e.g. appSettings.json)</param>
		/// <param name="next">The next middleware component to execute.</param>
		/// <remarks>
		///		Call the constructor in Startup.cs' "Configure" method, like so:
		///		app.UseMiddleware<Middleware.ErrorLogging>(Configuration);
		///
		///		Regarding the "next" parameter: all middleware components must either execute
		///		the next link in the pipeline (_next) or terminate by not calling _next.
		/// </remarks>
		public ErrorLogging(IConfiguration configuration, RequestDelegate next)
		{
			_config = configuration;
			_next = next;
		}

		/// <summary>
		/// The code that runs when this middleware is invoked.
		/// </summary>
		/// <param name="context">The HTTP request context.</param>
		/// <returns></returns>
		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception e)
			{
				string logDirectory = Path.Combine(_config.GetValue<string>("logFilePath"));
				string requestData = FormatRequestInfo(context.Request);
				string exceptionData = FormatExceptionInfo(e);

				WriteToFile(logDirectory, $"{requestData}{Environment.NewLine}{exceptionData}");
				//System.Diagnostics.Debug.WriteLine($"The following error happened: {e.Message}");

				throw;
			}
		}

		/// <summary>
		/// Writes the provided message to the provided directory.
		/// </summary>
		/// <param name="logDirectory">The destination directory of the log file.</param>
		/// <param name="message">The message to be written.</param>
		/// <remarks>
		///		This method handles the creation of the actual log file if it does not exist.
		/// </remarks>
		private void WriteToFile(string logDirectory, string message)
		{
			try
			{
				if (!Directory.Exists(logDirectory))
				{
					Directory.CreateDirectory(logDirectory);
				}

				string logFilePathAndName = Path.Combine($"{logDirectory}", $"{DateTime.Now.ToString("yyyy-MM-dd")}.log");

				// UTC time = Central time + 5 hours.
				// NewLine creates a blank line between log file entries.
				File.AppendAllText(logFilePathAndName, $"[{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}] {message}{Environment.NewLine}{Environment.NewLine}");
			}
			catch (Exception ex)
			{
				// Keeping the "ex.Data" for now because it may have interesting uses later.
				ex.Data.Add("Details", $"Failed to write message in {logDirectory}!");

				// TODO:It would be more preferable to throw the original exception, perhaps
				// with file-writing this file-writing error info attached.
				throw ex;
			}
		}

		/// <summary>
		/// Formats request data into a conveniently readable string.
		/// </summary>
		/// <param name="request">The HTTP request.</param>
		/// <returns>A string of the HTTP request data.</returns>
		private string FormatRequestInfo(HttpRequest request)
		{
			return $"{request.Method} {request.Scheme}://{request.Host}{request.Path}{request.QueryString} ({request.HttpContext.Response.StatusCode})";
		}

		/// <summary>
		/// Formats exception data into a conveniently readable string.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private string FormatExceptionInfo(Exception e)
		{
			string exceptionInfo = string.Empty;
			if (e != null)
			{
				exceptionInfo = $"{e.Message}{Environment.NewLine}{e.StackTrace}";
				if (e.InnerException != null)
				{
					exceptionInfo += $"{Environment.NewLine}INNER EXCEPTION: {FormatExceptionInfo(e.InnerException)}";
				}
			}

			return exceptionInfo;
		}
	}
}
