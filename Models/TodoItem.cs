using System;
using System.ComponentModel.DataAnnotations;
using Dapper;

namespace Todo.Models {
    public class TodoItem {

    [Key]
    public int ItemId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Boolean Completed { get; set; }

    }
}