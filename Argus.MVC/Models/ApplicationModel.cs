using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Argus.MVC.Models
{
	/// <summary>
	/// An aggregation of a company application and issue data
	/// for the reporting dashboard.
	/// </summary>
	public class ApplicationModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int IssueCount { get; set; }
		public bool HasUrgentPriority { get; set; }
		public string ProductOwnerName { get; set; }
		public string TeamName { get; set; }
		public string Url { get; set; }
	}
}
