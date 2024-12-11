using TapCleaner.Models.DTO;
using TapCleaner.Models;

namespace TapCleaner.Services.Interfaces
{
    public interface IQueryService
    {
        /// Adding user query
        Task<ErrorProvider> AddQuery(dtoUserQuery userQuery);
        /// Getting All Query Data
        Task<(ErrorProvider, List<UserQuery>)> GetUsersQueries();
        /// Getting Query From Specific User
        Task<(ErrorProvider, List<UserQuery>)> GetUserQueries(string email);
    }
}
