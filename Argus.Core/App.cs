using System;

namespace Argus.Core
{
	/// <summary>
	/// A simple representation of a company application
	/// for the reporting dashboard.
	/// </summary>
	public class App
	{
		public string Name { get; set; }
		public int IssueCount { get; set; }
		public string QueryString { get; set; }

		public App()
		{

		}
	}
}
