using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace PositionManagement.Api.Repositories {
    public class Repository {
        protected readonly IDbConnection _dbConnection;

        public Repository(IOptions<ConnectionStringProvider> connectionStringProvider) {
            var connectionString = connectionStringProvider.Value;
            if (connectionString == null) {
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionStringProvider));
            } else {
                _dbConnection = new SqlConnection(connectionString.PositionConnection);
            }
        }
    }
}
