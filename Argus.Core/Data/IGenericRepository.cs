using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Core.Data
{
	public interface IGenericRepository<TEntity> where TEntity : class, IEntity
	{
		TEntity Query(string sqlOrProcedure, object arguments);

		IEnumerable<TEntity> QueryMultiple(string sqlOrProcedure, object arguments);
	}
}
