using Microsoft.EntityFrameworkCore;
using TapCleaner.Context;
using TapCleaner.Models;
using TapCleaner.Models.DTO;
using TapCleaner.Services.Interfaces;

namespace TapCleaner.Services
{
    public class ContainerService:IContainerService
    {
        public ApplicationDbContext DbContext { get; set; }
        public IConfiguration configuration { get; set; }
        public ErrorProvider error = new ErrorProvider() { Status = false };
        public ErrorProvider defaultError = new ErrorProvider() { Status = true, Name = "Property must not be null" };
        public string EmailClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        public int Number = 1;

        public ContainerService() { }
        public ContainerService(ApplicationDbContext context, IConfiguration _configuration)
        {
            DbContext = context;
            configuration = _configuration;
        }

        public async Task<(ErrorProvider, List<Container>)> GetContainers()
        {
            var containers = await DbContext.Containers.ToListAsync();
            if(containers.Count == 0)
            {
                error = new ErrorProvider()
                {
                    Name = "None container in database!",
                    Status = true,
                };
                return (error, null);
            }
            return (error, containers);
        }

        public async Task<ErrorProvider> AddContainer(dtoContainer dtoContainer)
        {
            if(dtoContainer == null) 
            {
                return defaultError;
            }

            var newContainer = new Container()
            {
                Name = "Container" + " " + Number,
                Adress = dtoContainer.Adress,
                Coordinates = dtoContainer.Coordinates,
                Type = dtoContainer.Type,
                Condition = "Empty"
            };

            await DbContext.Containers.AddAsync(newContainer);
            await DbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Successfully added!",
            };

            Number++;

            return error;
        }

        public async Task<ErrorProvider> ReportContainer(dtoReportContainer dtoUserContainer)
        {
            if(dtoUserContainer == null)
            {
                return defaultError;
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == dtoUserContainer.UserEmail);

            if(userFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "None such user!"
                };
                return error;
            }

            var containerFromDatabase = await DbContext.Containers.FirstOrDefaultAsync(x => x.Name == dtoUserContainer.ContainerName);

            if (containerFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "None such container!"
                };
                return error;
            }

            UserContainer userContainer = new UserContainer()
            {
                User = userFromDatabase,
                Container = containerFromDatabase,
                Date = DateTime.Now
            };

            containerFromDatabase.Condition = "Full";
            containerFromDatabase.NumberOfReports += 1;

            await DbContext.UserContainers.AddAsync(userContainer);
            await DbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Reported successfully"
            };

            return error;
        }

        public async Task<ErrorProvider> CleanContainer(string name)
        {
            var container = await DbContext.Containers.FirstOrDefaultAsync(x => x.Name == name);
            if (container == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "None such container",
                };
                return error;
            }

            container.Condition = "Empty";
            container.NumberOfReports = 0;
            await DbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Successfully cleaned!"
            };

            return error;
        }
    }
}
