﻿using Auth.Domain;
using Auth.Service;
using Auth.Service.Repositories;
using System.Data.SqlClient;
using Dapper;

namespace Auth.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IAuthConfiguration authConfiguration)
        {
            _connectionString = authConfiguration.GetConnectionString();
        }

        public async Task<User> Get(string email)
        {
            var query = "SELECT Email, PasswordHash, PasswordSalt, CreatedDateTime, LastModifiedDateTime FROM [dbo].[User] WHERE Email = @Email";
            
            using var connection = new SqlConnection(_connectionString);
            
            return await connection.QueryFirstOrDefaultAsync<User>(query, new {Email = email});

        }

        public async Task<User> InsertUser(User user)
        {
            var command = "INSERT INTO [dbo].[User] (Email, PasswordSalt, PasswordHash, CreatedDateTime, LastModifiedDateTime)" +
                " VALUES(@Email, @PasswordSalt, @PasswordHash, @CreatedDateTime, @LastModifiedDateTime)";

            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(command, 
                new { Email = user.Email, PasswordSalt = user.PasswordSalt, PasswordHash = user.PasswordHash, CreatedDateTime = user.CreatedDateTimeUtc, LastModifiedDateTime = user.LastModifiedDateTimeUtc});

            return user;

        }
    }
}