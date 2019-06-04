using Argus.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Argus.Infrastructure.Repositories
{
	public class ApplicationRepository : IApplicationRepository
	{
		private readonly IConfiguration _config;

		public ApplicationRepository(IConfiguration config)
		{
			_config = config ?? throw new ArgumentNullException(nameof(config), "Configuration cannot be null.");
		}

		public IEnumerable<Application> GetApplications()
		{
			using (var connection = new SqlConnection(_config.GetConnectionString("Argus")))
			{
				connection.Open();
				return connection.Query<Application>(SqlGetApplications);
			}
		}

		public Application GetApplicationById(int id)
		{
			using (var connection = new SqlConnection(_config.GetConnectionString("Argus")))
			{
				connection.Open();
				return connection.Query<Application>(SqlGetApplicationById, new { Id = id }).FirstOrDefault();
			}
		}

		private readonly string SqlGetApplications =
			@"
			SELECT
				Id,
				Name,
				ProductOwnerName,
				TeamName,
				Url,
				IsEnabled,
				DateModified
			FROM
				Applications";

		private readonly string SqlGetApplicationById =
			@"
			SELECT
				Id,
				Name,
				ProductOwnerName,
				TeamName,
				Url,
				IsEnabled,
				DateModified
			FROM
				Applications
			WHERE
				Id = @Id";
	}
}
