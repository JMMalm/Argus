using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Argus.Core;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Argus.Infrastructure.Repositories
{
	public class IssueRepository : IIssueRepository
	{
		private readonly IConfiguration _config;

		public IssueRepository(IConfiguration config)
		{
			_config = config ?? throw new ArgumentNullException(nameof(config), "Configuration cannot be null.");
		}

		public Issue GetIssueById(int id)
		{
			using (var connection = new SqlConnection(_config.GetConnectionString("Argus")))
			{
				connection.Open();
				return connection.Query<Issue>(SqlGetIssueById, new { Id = id }).FirstOrDefault();
			}
		}

		public IEnumerable<Issue> GetIssues()
		{
			using (var connection = new SqlConnection(_config.GetConnectionString("Argus")))
			{
				connection.Open();
				return connection.Query<Issue>(SqlGetIssues);
			}
		}

		public IEnumerable<Issue> GetIssuesByDate(DateTime date, DateTime endDate)
		{
			using (var connection = new SqlConnection(_config.GetConnectionString("Argus")))
			{
				connection.Open();
				return connection.Query<Issue>(SqlGetIssuesByDate, new
				{
					Date = date,
					EndDate = endDate
				});
			}
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