using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Argus.Infrastructure.Repositories
{
	public abstract class GenericRepository
	{
		// See more here:
		// https://codereview.stackexchange.com/questions/177588/generic-repository-without-entity-framework
		//

		private readonly IConfiguration _config;
		protected IDbConnection _connection;
		private int _deadlockRetryCount = 0;

		protected string ConnectionString { get; set; }

		public GenericRepository(IConfiguration config)
		{
			_config = config ?? throw new ArgumentNullException(nameof(config), "Configuration cannot be null.");
		}

		protected T Query<T>(string sqlOrProcedure, object arguments, CommandType queryType = CommandType.Text, int timeoutInSeconds = 60)
		{

			T result = default(T);

			TryExecute<T>(() => result = _connection.QuerySingle<T>(sqlOrProcedure, arguments, commandType: queryType, commandTimeout: timeoutInSeconds));

			return result;
		}

		protected IEnumerable<T> QueryMultiple<T>(string sqlOrProcedure, object arguments = null, CommandType queryType = CommandType.Text, int timeoutInSeconds = 60)
		{

			IEnumerable<T> result = default(IEnumerable<T>);

			TryExecute<T>(() => result = _connection.Query<T>(sqlOrProcedure, arguments, commandType: queryType, commandTimeout: timeoutInSeconds));

			return result;
		}

		protected T Do<T>(string sqlOrProcedure, object arguments, CommandType queryType = CommandType.Text, int timeoutInSeconds = 60)
		{
			T result = default(T);

			TryExecute<T>(() => result = _connection.QuerySingle<T>(sqlOrProcedure, arguments, commandType: queryType, commandTimeout: timeoutInSeconds));

			return result;
		}

		private void TryExecute<T>(Action action)
		{
			try
			{
				using (_connection)
				{
					action();
				}
			}
			catch (SqlException sx)
			{
				throw new Exception($"{DateTime.Now}: SQL error {sx.ErrorCode}: {sx.Message}", sx);
			}
			catch (Exception ex)
			{
				throw new Exception($"{DateTime.Now}: Dapper error: {ex.Message}", ex);
			}
		}
	}
}
