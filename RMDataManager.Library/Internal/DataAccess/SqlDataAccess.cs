﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RMDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess : IDisposable
    {
        public string GetConnectionString(string name) => ConfigurationManager.ConnectionStrings[name].ConnectionString;
        
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

        public void CommitTransaction()
        {
            transaction?.Commit();
            connection?.Close();
        }

        public void RollbackTransaction()
        {
            transaction?.Rollback();
            connection?.Close();
        }

        public void Dispose()
        {
            CommitTransaction();
        }
    }
}
