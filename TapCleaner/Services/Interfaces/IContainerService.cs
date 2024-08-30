using TapCleaner.Models;

namespace TapCleaner.Services.Interfaces
{
    public interface IContainerService
    {
        Task<(ErrorProvider, List<Container>)> GetContainers();
        Task<ErrorProvider> AddContainer(Container container);
    }
}
