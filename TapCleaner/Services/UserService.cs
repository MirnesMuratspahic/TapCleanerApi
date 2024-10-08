﻿using Azure.Core;
using TapCleaner.Context;
using TapCleaner.Models;
using TapCleaner.Models.DTO;
using TapCleaner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TapCleaner.Services
{
    public class UserService:IUserService
    {
        public ApplicationDbContext DbContext { get; set; }
        public IConfiguration configuration { get; set; }
        public ErrorProvider error = new ErrorProvider() { Status = false };
        public ErrorProvider defaultError = new ErrorProvider() { Status = true, Name = "Property must not be null" };
        public string EmailClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

        public UserService() { }
        public UserService(ApplicationDbContext context, IConfiguration _configuration)
        {
            DbContext = context;
            configuration = _configuration;
        }

        public async Task<(ErrorProvider, dtoUserInfo)> Login(dtoUserLogin userDto)
        {
            if (userDto == null)
                return (defaultError,null);

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == userDto.Email.ToLower());

            if (userFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "You have not entered correct information!"
                };
                return (error,null);
            }

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, userFromDatabase.PasswordHash))
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "You have not entered correct information!"
                };
                return (error,null);
            }
            var token = CreateToken(userFromDatabase);
            await DbContext.SaveChangesAsync();

            var userInfo = new dtoUserInfo()
            {
                Token = token,
                Rola = userFromDatabase.Role
            };

            return (error, userInfo);

        }



        public async Task<(ErrorProvider, User)> GetUserByEmail([FromBody] string email)
        {

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "There is no user with that email!"

                };
                return (error, null);
            }

            return (error, userFromDatabase);
        }

        public async Task<ErrorProvider> UpdateUser(string email, dtoUserUpdate user)
        {

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x=> x.Email == email);

            if (userFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "There is no user with that email!"

                };
                return error;
            }

            userFromDatabase.FirstName = user.FirstName;
            userFromDatabase.LastName = user.LastName;
            userFromDatabase.ImageUrl = user.ImageUrl;

            DbContext.Users.Update(userFromDatabase);
            await DbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "User updated!"
            };
            return error;
        }

        public async Task<ErrorProvider> BlockUserByEmail(string email)
        {

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x=>x.Email == email);

            if (userFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "There is no user with that email!"

                };
                return error;
            }

            userFromDatabase.Status = "Blocked";
            await DbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "User blocked"
            };

            return error;
        }


        public async Task<ErrorProvider> Register(dtoUserRegistration userDto)
        {
            if (userDto == null)
                return defaultError;

            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == userDto.Email.ToLower());

            if (user != null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "This username is already taken!"
                };
                return error;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashString(userDto.Password);

            var newUser = new User()
            {
                Email = userDto.Email.ToLower(),
                PasswordHash = passwordHash,
                FirstName = userDto.FirstName.ToLower(),
                LastName = userDto.LastName.ToLower(),
                ImageUrl = userDto.ImageUrl,
                Status = "Active",
                Role = userDto.Role,
            };

            await DbContext.Users.AddAsync(newUser);
            await DbContext.SaveChangesAsync();
            var token = CreateToken(newUser);

            error = new ErrorProvider()
            {
                Status = false,
                Name = "User registered!"
            };

            return error;

        }

        public async Task<(ErrorProvider, List<User>)> GetUsers()
        {
            var users = await DbContext.Users.ToListAsync();
            if (users.Count == 0)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "There are no users in the database!"
                };
                return (error, null);
            }

            return (error, users);
        }

        private string CreateToken(User user)
        {
            List<Claim> _claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    claims: _claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
