using Argus.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Core.Application
{
	public class ApplicationService : IApplicationService
	{
		private readonly IGenericRepository<Application> _repo;

		public ApplicationService(IGenericRepository<Application> repo)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo), "Repository cannot be null.");
		}

		public Application GetById(int id)
		{
			return _repo.Query(SqlGetApplicationById, new { Id = id });
		}

		public IEnumerable<Application> GetApplications()
		{
			return _repo.QueryMultiple(SqlGetApplications, null);
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
