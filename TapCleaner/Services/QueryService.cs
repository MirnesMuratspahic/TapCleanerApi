using Microsoft.EntityFrameworkCore;
using TapCleaner.Models.DTO;
using TapCleaner.Models;
using TapCleaner.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TapCleaner.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TapCleaner.Services
{
    public class QueryService : IQueryService
    {
        public ApplicationDbContext DbContext { get; set; }

        public IConfiguration configuration { get; set; }

        public ErrorProvider error = new ErrorProvider() { Status = false };

        public ErrorProvider defaultError = new ErrorProvider() { Status = true, Name = "Property must not be null" };

        public QueryService(ApplicationDbContext context, IConfiguration _configuration)
        {
            DbContext = context;
            configuration = _configuration;
        }

        public async Task<ErrorProvider> AddQuery(dtoUserQuery query)
        {
            if (query == null)
            {
                return defaultError;
            }

            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == query.Email);

            if (user == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "U bazi trenutno nemamo korisnika koji posjeduje taj e-mail."
                };
                return error;
            }

            var userQuery = new UserQuery()
            {
                User = user,
                Query = query.Query,
            };

            DbContext.UserQueries.Add(userQuery);
            await DbContext.SaveChangesAsync();

            error.Name = "Uspješno ste poslali upit!";
            return error;
        }

        public async Task<(ErrorProvider, List<UserQuery>)> GetUsersQueries()
        {
            var userQueries = await DbContext.UserQueries.Include(u => u.User).ToListAsync();

            if (userQueries.Count == 0)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "There are no user queries in the database",
                };
                return (error, null);
            }

            return (error, userQueries);
        }
    }
}
