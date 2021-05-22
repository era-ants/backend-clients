using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Clients.DataTransfer;
using Clients.Hubs;
using Clients.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Clients.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class ClientsController : ControllerBase
    {
        private readonly IHubContext<ClientsHub> _clientsHub;
        private readonly IClientsService _clientsService;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(
            ILogger<ClientsController> logger,
            IClientsService clientsService,
            IHubContext<ClientsHub> clientsHub)
        {
            _logger = logger;
            _clientsService = clientsService;
            _clientsHub = clientsHub;
        }

        /// <summary>
        /// Возвращает полные данные по всем клиентам системы
        /// </summary>
        [HttpGet]
        public Task<IEnumerable<FullClientDto>> Get() => _clientsService.GetFullClientsInfo();

        /// <summary>
        /// Возвращает полные данные по выбранному клиенту системы
        /// </summary>
        /// <param name="clientGuid">Guid клиента</param>
        [HttpGet("{clientGuid:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FullClientDto>> Get([Required] Guid clientGuid)
        {
            var clientInfo = await _clientsService.GetFullClientInfoOrDefaultAsync(clientGuid);
            if (clientInfo == default) return NotFound();
            return Ok(clientInfo);
        }

        /// <summary>
        /// Регистрирует нового гостя Новороссийска в системе
        /// </summary>
        /// <param name="registerCitizenDto">Данные для регистрации</param>
        [HttpPost("RegisterGuest")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterClientSuccess>> Post([Required] RegisterGuestDto registerCitizenDto)
        {
            var result = await _clientsService.RegisterGuest(registerCitizenDto);
            if (!result.IsSuccess) return BadRequest(result.ErrorResult);
            // TODO: изменить порядок выполнения рассылки
            await _clientsHub.Clients.All.SendAsync("Notify",
                $"New user has been added! Guid: {result.OkResult.ClientGuid}");
            return Ok(result.OkResult);
        }
        
        /// <summary>
        /// Регистрирует нового жителя Новороссийска в системе
        /// </summary>
        /// <param name="registerCitizenDto">Данные для регистрации</param>
        [HttpPost("RegisterCitizen")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterClientSuccess>> Post([Required] RegisterCitizenDto registerCitizenDto)
        {
            var result = await _clientsService.RegisterCitizen(registerCitizenDto);
            if (!result.IsSuccess) return BadRequest(result.ErrorResult);
            // TODO: изменить порядок выполнения рассылки
            await _clientsHub.Clients.All.SendAsync("Notify",
                $"New user has been added! Guid: {result.OkResult.ClientGuid}");
            return Ok(result.OkResult);
        }
    }
}