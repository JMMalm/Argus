using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Core
{
	public interface IAppRepository
	{
		IEnumerable<App> GetAppDataByDate(DateTime viewDate);
	}
}
