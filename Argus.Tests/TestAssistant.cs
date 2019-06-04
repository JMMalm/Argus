using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Argus.Tests
{
	/// <summary>
	/// Handles common logic and figurative tasks for tests to reduce duplicate code.
	/// </summary>
	public class TestAssistant
	{
		public static IConfiguration GetConfig()
		{
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile(@"c:\code\argus\argus.MVC\secrets.json", optional: false, reloadOnChange: true)
				.Build();
		}
	}
}
