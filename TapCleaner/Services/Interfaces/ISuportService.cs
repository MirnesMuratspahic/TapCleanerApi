using TapCleaner.Models;

namespace TapCleaner.Services.Interfaces
{
    public interface ISuportService
    {
        Task<(ErrorProvider, string)> GetClosestContainer(string coordinate);
    }
}
