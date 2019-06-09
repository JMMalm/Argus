using Argus.Core.Data;
using Argus.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Core.Issue
{
	public class Issue : IEntity
	{
		public int Id { get; set; }
		public int ApplicationId { get; set; }
		public string Description { get; set; }
		public string UserName { get; set; }
		public DateTime DateModified { get; set; }
		public DateTime DateSubmitted { get; set; }
		public DateTime DateClosed { get; set; }
		public Priority Priority { get; set; }
	}
}
