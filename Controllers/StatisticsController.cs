using System.Threading.Tasks;
using Clients.DataTransfer;
using Clients.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Clients.Controllers
{
    /// <summary>
    /// Предоставляет статистические данные о клиентах системы
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public sealed class StatisticsController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(
            ILogger<ClientsController> logger,
            IStatisticsService statisticsService)
        {
            _logger = logger;
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Возвращает статистические данные по зарегистрированным в системе клиентам
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<StatisticsDto> Get() => _statisticsService.GetStatistics();
    }
}