using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Core.Issue
{
	public interface IIssueService
	{
		IEnumerable<Issue> GetIssues();

		IEnumerable<Issue> GetIssuesByDate(DateTime date, DateTime endDate);

		Issue GetIssueById(int id);
	}
}
