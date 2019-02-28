using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Todo.Models;
using System;
using Microsoft.Extensions.Configuration;

namespace Todo.Repository
{
    public class TodoRepository : BaseRepository, ITodoRepository {

        public TodoRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public IDbConnection Connection 
        {
            get {
                return GetOpenConnection();
            }
        }

        public TodoItem Add (TodoItem item) {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO TodoItems (Title, Description, Completed)"
                                + " VALUES(@Title, @Description, @Completed); SELECT CAST(SCOPE_IDENTITY() as int)";

                var result = dbConnection.Query<int>(sQuery, item).First();
                item.ItemId = result;
                return item;
            }
        }

        public IEnumerable<TodoItem> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                return dbConnection.Query<TodoItem>("SELECT * FROM TodoItems");
            }
        }

        public TodoItem GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM TodoItems"
                           + " WHERE ItemId = @Id";

                return dbConnection.Query<TodoItem>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "DELETE FROM TodoItems"
                                + " WHERE ItemId = @Id";

                dbConnection.Execute(sQuery, new { Id = id });
            }
        }

        public void Update(TodoItem item)
        {
            using (IDbConnection dbConnection = Connection) 
            {
                string sQuery = "UPDATE TodoItems SET Title = @Title,"
                           + " Description = @Description, Completed= @Completed"
                           + " WHERE ItemId = @ItemId";


                dbConnection.Query(sQuery, item);
            }
        }
    }
    
}