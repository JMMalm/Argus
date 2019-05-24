﻿using Argus.Core;
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

		public IEnumerable<App> GetAppDataByDate(DateTime viewDate)
		{
			using (var connection = new SqlConnection(_config.GetConnectionString("Argus")))
			{
				connection.Open();
				return connection.Query<App>(SqlGetAppDataByDate, new { Date = viewDate });
			}
		}

		private readonly string SqlGetAppDataByDate =
			@"
			SELECT
				DISTINCT AppName AS [Name],
				COUNT(AppName) AS IssueCount
			FROM AppIssues
			WHERE SubmittedDate = @Date
			GROUP BY AppName";
	}
}
