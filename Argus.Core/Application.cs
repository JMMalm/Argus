using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Core
{
	public class Application
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ProductOwnerName { get; set; }
		public string TeamName { get; set; }
		public string Url { get; set; }

		/// <summary>
		/// Determines if the application is set to display in the UI.
		/// </summary>
		public bool IsEnabled { get; set; }
		public DateTime DateModified { get; set; }
	}
}
