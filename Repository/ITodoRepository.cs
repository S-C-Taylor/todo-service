using System.Collections.Generic;
using Todo.Models;

namespace Todo.Repository
{
    public interface ITodoRepository
    {
        TodoItem Add (TodoItem item); 
        IEnumerable<TodoItem> GetAll();  
        TodoItem GetByID(int id);
        void Delete(int id);
        void Update(TodoItem item);
    }
}