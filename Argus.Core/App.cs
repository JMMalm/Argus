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
		public string Url { get; set; }
		public bool HasUrgentPriority { get; set; }

		public App()
		{

		}
	}
}
