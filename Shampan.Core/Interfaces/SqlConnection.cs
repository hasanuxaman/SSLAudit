namespace Shampan.Core.Interfaces.Repository
{
    internal class SqlConnection
    {
        private object connectionString;

        public SqlConnection(object connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}