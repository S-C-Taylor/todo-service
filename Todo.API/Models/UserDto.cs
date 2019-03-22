using System;
using System.ComponentModel.DataAnnotations;
using Dapper;

namespace Todo.API.Models {
    public class UserDto {   
    public string Username { get; set; }
    public string Password { get; set; }
    }
}