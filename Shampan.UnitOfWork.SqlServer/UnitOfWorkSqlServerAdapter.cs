using Microsoft.Data.SqlClient;
using Shampan.Core.Interfaces.UnitOfWork;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServerAdapter : IUnitOfWorkAdapter
    {
        private SqlConnection _context { get; set; }
        private SqlTransaction _transaction { get; set; }
        public IUnitOfWorkRepository Repositories { get; set; }        
        public UnitOfWorkSqlServerAdapter(string connectionString, DbConfig dbConfig)
        {
            _context = new SqlConnection(connectionString);
            _context.Open();

            _transaction = _context.BeginTransaction();
        
            Repositories = new UnitOfWorkSqlServerRepository(_context, _transaction, dbConfig);
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            if (_context != null)
            {
                _context.Close();
                _context.Dispose();
            }

            Repositories = null;
        }

        public void SaveChanges()
        {
            _transaction.Commit();
        }


        public void RollBack()
        {
            _transaction.Rollback();
        }
    }
}
