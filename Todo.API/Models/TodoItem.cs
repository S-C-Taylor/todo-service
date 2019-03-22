using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Dapper;

namespace Todo.API.Models {
    [DataContract]
    public class TodoItem {

    [Key]
    [DataMember]
    public int ItemId { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public Boolean Completed { get; set; }
    public string CreatedBy { get; set; }

    }
}