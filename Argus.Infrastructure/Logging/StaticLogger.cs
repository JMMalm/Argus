using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Argus.Infrastructure.Logging
{
	public static class StaticLogger
	{
		public static string LogFilePath { get; }

		static StaticLogger()
		{
			LogFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
		}

		public static void Write(string message)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}

			WriteToFile(message);
		}

		public static void Write(Exception ex)
		{
			string exceptionDetails = string.Format("{0}{1}{2}{1}{3}{1}{4}{1}",
				ex.Message,
				Environment.NewLine,
				ex.StackTrace,
				ex.InnerException?.Message,
				ex.InnerException?.StackTrace);

			WriteToFile(exceptionDetails);
		}

		private static void WriteToFile(string message)
		{
			try
			{
				if (!Directory.Exists(LogFilePath))
				{
					Directory.CreateDirectory(LogFilePath);
				}

				string fullFileName = Path.Combine($"{LogFilePath}", $"{DateTime.Now.ToString("yyyy-MM-dd")}.log");

				// UTC time = Central time + 5 hours.
				File.AppendAllText(fullFileName, $"[{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {message}{Environment.NewLine}");
			}
			catch (Exception ex)
			{
				ex.Data.Add("Details", $"Failed to write message in {LogFilePath}!");
				throw ex;
			}
		}
	}
}
