using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Core
{
	public interface IApplicationRepository
	{
		IEnumerable<Application> GetApplications();

		Application GetApplicationById(int id);
	}
}
