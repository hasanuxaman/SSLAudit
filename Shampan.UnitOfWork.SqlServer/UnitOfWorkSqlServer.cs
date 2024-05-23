using Microsoft.Extensions.Configuration;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServer : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly DbConfig _dbConfig;

        public UnitOfWorkSqlServer(IConfiguration configuration, DbConfig dbConfig)
        {
            _configuration = configuration;
            _dbConfig = dbConfig;
        }

        public IUnitOfWorkAdapter Create()
        {
            var connectionString = _configuration.GetConnectionString("DBContext");

            connectionString = connectionString.Replace("@db", _dbConfig.DbName);

            return new UnitOfWorkSqlServerAdapter(connectionString, _dbConfig);
        }


        public IUnitOfWorkAdapter CreateSage()
        {
            var connectionString = _configuration.GetConnectionString("DBContextSage");

            connectionString = connectionString.Replace("@sageDb", _dbConfig.SageDbName);

            return new UnitOfWorkSqlServerAdapter(connectionString, _dbConfig);
        }

        public IUnitOfWorkAdapter CreateAuth()
        {
            var connectionString = _configuration.GetConnectionString("AuthContext");

            return new UnitOfWorkSqlServerAdapter(connectionString, _dbConfig);
        }

        
    }
}
