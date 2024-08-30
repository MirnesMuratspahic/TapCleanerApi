using TapCleaner.Models;
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
        Task<(ErrorProvider, string)> Login(dtoUserLogin userLogin);
        ///users: Fetch all users endpoint.
        Task<(ErrorProvider, User)> GetUserById(int id);
        ///users/block/(id): Block a user by ID endpoint.
        Task<ErrorProvider> BlockUserById(int id);
        ///register: Register/add user endpoint.
        Task<ErrorProvider> Register(dtoUserRegistration userRegistration);
        /// Update user informations
        Task<ErrorProvider> UpdateUser(int id, dtoUserUpdate user);
    }
}
