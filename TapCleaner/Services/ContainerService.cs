using Microsoft.EntityFrameworkCore;
using TapCleaner.Context;
using TapCleaner.Models;
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

        public async Task<ErrorProvider> AddContainer(Container container)
        {
            if(container == null) 
            {
                return defaultError;
            }

            var newContainer = new Container()
            {
                Name = container.Name,
                Adress = container.Adress,
                Coordinates = container.Coordinates,
                Type = container.Type,
                Condition = container.Condition
            };

            await DbContext.Containers.AddAsync(newContainer);
            await DbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Name = "Succesfully added!",
                Status = false,
            };

            return error;
        }

    }
}
