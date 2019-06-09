using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Argus.Core.Data;

namespace Argus.Infrastructure.Repositories
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable
		where TEntity : class, IEntity
	{
		// See more here:
		// https://codereview.stackexchange.com/questions/177588/generic-repository-without-entity-framework
		//

		private readonly IConfiguration _config;
		private readonly string _connectionString;
		private IDbConnection _connection;

		public GenericRepository(IConfiguration config, string connectionString)
		{
			_config = config
				?? throw new ArgumentNullException(nameof(config), "Configuration cannot be null.");
			_connectionString = _config.GetConnectionString(connectionString)
				?? throw new ArgumentNullException("Connection string cannot be null.");

			//_connection = new SqlConnection(_config.GetConnectionString(connectionString));
		}

		public void Dispose()
		{
			if (_connection != null)
			{
				_connection.Dispose();
			}
		}

		public TEntity Query(string sqlOrProcedure, object arguments)
		{
			TEntity result = default(TEntity);

			TryExecute<TEntity>(() => result = _connection.QuerySingle<TEntity>(sqlOrProcedure, arguments, commandType: CommandType.Text, commandTimeout: 60));

			return result;
		}

		public IEnumerable<TEntity> QueryMultiple(string sqlOrProcedure, object arguments)
		{
			IEnumerable<TEntity> result = default(IEnumerable<TEntity>);

			TryExecute<TEntity>(() => result = _connection.Query<TEntity>(sqlOrProcedure, arguments, commandType: CommandType.Text, commandTimeout: 60));

			return result;
		}

		private void TryExecute<IEntity>(Action action)
		{
			try
			{
				using (_connection = new SqlConnection(_connectionString))
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
