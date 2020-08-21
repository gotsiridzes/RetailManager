using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RMDataManager.Library.Internal.DataAccess
{
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        public SqlDataAccess(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetConnectionString(string name)
        {
            return configuration.GetConnectionString(name);
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection sqlConnection = new SqlConnection(connectionString))
            {
                List<T> rows = sqlConnection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure)
                    .ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        private IDbConnection connection;
        private IDbTransaction transaction;

        public void StartTransaction(string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);
            connection = new SqlConnection(connectionString);
            connection.Open();
            transaction = connection.BeginTransaction();

            isClosed = false;
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            connection.Execute(storedProcedure,
                               parameters,
                               commandType: CommandType.StoredProcedure,
                               transaction: transaction);
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            List<T> rows = connection
            .Query<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure,
                transaction: transaction)
                .ToList();

            return rows;
        }

        private bool isClosed = false;
        private readonly IConfiguration configuration;

        public void CommitTransaction()
        {
            transaction?.Commit();
            connection?.Close();

            isClosed = true;
        }

        public void RollbackTransaction()
        {
            transaction?.Rollback();
            connection?.Close();

            isClosed = true;
        }

        public void Dispose()
        {
            if (isClosed == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    //უნდა დაილოგოს ეს შეცდომა
                }
            }

            transaction = null;
            connection = null;
        }
    }
}
