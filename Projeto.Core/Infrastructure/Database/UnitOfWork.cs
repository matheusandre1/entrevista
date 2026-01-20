using System;
using System.Data;

namespace Projeto.Core.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConnectionFactory _connectionFactory;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed;

        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = _connectionFactory.CreateConnection();
                    _connection.Open();
                }
                return _connection;
            }
        }

        public IDbTransaction Transaction => _transaction;

        public void Begin()
        {
            if (_transaction == null)
            {
                _transaction = Connection.BeginTransaction();
            }
        }

        public void Commit()
        {
            _transaction?.Commit();
            DisposeTransaction();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            DisposeTransaction();
        }

        private void DisposeTransaction()
        {
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DisposeTransaction();
                    _connection?.Dispose();
                    _connection = null;
                }
                _disposed = true;
            }
        }
    }
}
