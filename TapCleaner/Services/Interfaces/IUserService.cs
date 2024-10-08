﻿using TapCleaner.Models;
using TapCleaner.Models.DTO;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace TapCleaner.Services.Interfaces
{
    public interface IUserService
    {
        ///users: Get all users
        Task<(ErrorProvider, List<User>)> GetUsers();
        ///login: Admin login authentication endpoint.
        Task<(ErrorProvider, dtoUserInfo)> Login(dtoUserLogin userLogin);
        ///users: Fetch all users endpoint.
        Task<(ErrorProvider, User)> GetUserByEmail(string email);
        ///users/block/(id): Block a user by ID endpoint.
        Task<ErrorProvider> BlockUserByEmail(string email);
        ///register: Register/add user endpoint.
        Task<ErrorProvider> Register(dtoUserRegistration userRegistration);
        /// Update user informations
        Task<ErrorProvider> UpdateUser(string email, dtoUserUpdate user);
    }
}
