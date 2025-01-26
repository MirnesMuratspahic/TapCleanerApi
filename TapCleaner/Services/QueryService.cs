using Microsoft.EntityFrameworkCore;
using TapCleaner.Models.DTO;
using TapCleaner.Models;
using TapCleaner.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TapCleaner.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Mvc;

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
                    Name = "We don't have user with same data."
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

            error.Name = "Query successfully added!";
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

        public async Task<(ErrorProvider, List<UserQuery>)> GetUserQueries(string email)
        {
            var queries = await DbContext.UserQueries.Where(x => x.User.Email == email).Include(x => x.User).ToListAsync();

            return(error, queries);
        }

        public async Task<ErrorProvider> DeleteQuery(int queryId)
        {
            if(queryId < 0)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "ID must be higher than -1"
                };
                return (error);
            }

            var queryFromDatabase = await DbContext.UserQueries.FirstOrDefaultAsync(x => x.Id == queryId);

            if (queryFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "There is no query with same data!"
                };
                return error;
            }

            DbContext.UserQueries.Remove(queryFromDatabase);
            await DbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Query successfully removed!"
            };

            return error;
        }
    }
}
