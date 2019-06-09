using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Core.Application
{
	public interface IApplicationService
	{
		Application GetById(int id);
		IEnumerable<Application> GetApplications();
	}
}
