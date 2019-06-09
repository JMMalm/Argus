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
	public class ApplicationRepository : GenericRepository, IApplicationRepository
	{
		//private readonly IConfiguration _config;

		public ApplicationRepository(IConfiguration config) : base(config)
		{
			_connection = new SqlConnection(config.GetConnectionString("Argus"));
		}

		public IEnumerable<Application> GetApplications()
		{
			return QueryMultiple<Application>(SqlGetApplications);
		}

		public Application GetApplicationById(int id)
		{
			return Query<Application>(SqlGetApplicationById, new { Id = id });
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
