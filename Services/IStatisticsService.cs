using System.Threading.Tasks;
using Clients.DataTransfer;

namespace Clients.Services
{
    public interface IStatisticsService
    {
        Task<StatisticsDto> GetStatistics();
    }
}