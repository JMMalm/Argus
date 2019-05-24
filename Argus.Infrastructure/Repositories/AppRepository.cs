using Argus.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Argus.Infrastructure.Repositories
{
	public class AppRepository
	{
		private readonly IConfiguration _config;

		public AppRepository(IConfiguration config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("Configuration cannot be null.");
			}
			_config = config;
		}

		public IEnumerable<App> GetUsers()
		{
			using (var connection = new SqlConnection(_config.GetConnectionString("DapperContribDemo")))
			{
				connection.Open();
				return connection.Query<App>(SqlGetAppDataByDate);
			}
		}

		private readonly string SqlGetAppDataByDate =
			@"
			SELECT
				DISTINCT AppName AS [Name],
				COUNT(AppName) AS IssueCount,
				IssueCount
			FROM AppIssues
			WHERE SubmittedDate = @Date
			GROUP BY AppName";
	}
}
