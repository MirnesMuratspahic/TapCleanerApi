using TapCleaner.Models;
using TapCleaner.Models.DTO;

namespace TapCleaner.Services.Interfaces
{
    public interface IContainerService
    {
        Task<(ErrorProvider, List<Container>)> GetContainers();
        Task<ErrorProvider> AddContainer(dtoContainer dtoContainer);
        Task<ErrorProvider> ReportContainer(dtoReportContainer userContainer);
        Task<ErrorProvider> CleanContainer(string name);
        Task<ErrorProvider> DeleteContainerByName(string name);
    }
}
