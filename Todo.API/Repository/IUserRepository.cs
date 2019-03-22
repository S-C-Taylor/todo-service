using System.Collections.Generic;
using Todo.API.Models;

namespace Todo.API.Repository
{
    public interface IUserRepository
    {
        User Login (string username, string password); 
        User Create(UserDto user);
    }
}