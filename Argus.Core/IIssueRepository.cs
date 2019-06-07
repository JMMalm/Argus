using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Core
{
	public interface IIssueRepository
	{
		IEnumerable<Issue> GetIssues();

		IEnumerable<Issue> GetIssuesByDate(DateTime date);

		Issue GetIssueById(int id);
	}
}
