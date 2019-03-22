using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Dapper;

namespace Todo.API.Models {
    [DataContract]
    public class User {
    
    [DataMember]
    [Key]
    public int Id { get; set; }

    [DataMember]
    public string Username { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }
    }
}