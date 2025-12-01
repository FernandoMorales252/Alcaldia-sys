using AlcaldiaApi.DTOs.DashboardDTOs;
using System.Threading.Tasks;

namespace AlcaldiaApi.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardDataDTO> GetAggregatedDataAsync();
    }
}
