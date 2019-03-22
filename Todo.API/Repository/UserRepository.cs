

using System;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Todo.API.Models;

namespace Todo.API.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration config) : base(config)
        {

        }

        public IDbConnection Connection 
        {
            get {
                return GetOpenConnection();
            }
        }

        public User Login(string username, string password)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var qry = "SELECT * FROM Users WHERE Username = @usr";
                var user = dbConnection.QueryFirstOrDefault<User>(qry , new { usr = username, pwd = password });

                if (user == null) {
                    return null;
                }

                if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) {
                    return null;
                }

                return user;
            }
        }

        public User Create(UserDto userDto) {
            using (IDbConnection dbConnection = Connection)
            {

                var qry = "SELECT * FROM Users WHERE Username = @usr";
                var exists = dbConnection.QueryFirstOrDefault<User>(qry, new { usr = userDto.Username, pwd = userDto.Password });

                if (exists != null) {
                    return null;
                }

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

                var user = new User {
                    Username = userDto.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                string sQuery = "INSERT INTO Users (Username, PasswordHash, PasswordSalt)"
                                + " VALUES(@Username, @PasswordHash, @PasswordSalt); SELECT CAST(SCOPE_IDENTITY() as int)";

                var result = dbConnection.Query<int>(sQuery, user).First();
                user.Id = result;
                return user;
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

    }

}