
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Todo.Repository {
    public class BaseRepository {
        private readonly IConfiguration config;

        public BaseRepository(IConfiguration config)
        {
            this.config = config;
        }

        public SqlConnection GetOpenConnection() {
            //string cs = config["Data:DefaultConnection:ConnectionString"];
            string cs = config.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
    }
}