using Argus.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Core.Issue
{
	public class IssueService : IIssueService
	{
		private readonly IGenericRepository<Issue> _repo;

		public IssueService(IGenericRepository<Issue> repo)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo), "Repository cannot be null.");
		}

		public Issue GetIssueById(int id)
		{
			return _repo.Query(SqlGetIssueById, new { Id = id });
		}

		public IEnumerable<Issue> GetIssues()
		{
			return _repo.QueryMultiple(SqlGetIssues, null);
		}

		public IEnumerable<Issue> GetIssuesByDate(DateTime date, DateTime endDate)
		{
			return _repo.QueryMultiple(SqlGetIssuesByDate, new
			{
				Date = date,
				EndDate = endDate
			});
		}

		private readonly string SqlGetIssues =
			@"
			SELECT
				Id,
				ApplicationId,
				Description,
				UserName,
				DateSubmitted,
				DateClosed,
				Priority
			FROM
				Issues";

		private readonly string SqlGetIssueById =
			@"
			SELECT
				Id,
				ApplicationId,
				Description,
				UserName,
				DateSubmitted,
				DateClosed,
				Priority
			FROM
				Issues
			WHERE
				Id = @Id";

		private readonly string SqlGetIssuesByDate =
			@"
			SELECT
				Id,
				ApplicationId,
				Description,
				UserName,
				DateSubmitted,
				DateClosed,
				Priority
			FROM
				Issues
			WHERE
				DateSubmitted >= @Date
				AND DateSubmitted < @EndDate";
	}
}
