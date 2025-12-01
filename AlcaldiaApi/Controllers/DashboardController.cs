
using AlcaldiaApi.DTOs.DashboardDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta: api/Dashboard
   
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        // Endpoint: GET api/Dashboard/data
        [HttpGet("data")]
        [ProducesResponseType(typeof(DashboardDataDTO), 200)]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var data = await _dashboardRepository.GetAggregatedDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Loguea el error aquí
                return StatusCode(500, "Error al obtener los datos del Dashboard: " + ex.Message);
            }
        }
    }
}
