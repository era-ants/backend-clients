using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Clients.DataTransfer;
using Clients.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Clients.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(
            ILogger<ClientsController> logger,
            IClientsService clientsService)
        {
            _logger = logger;
            _clientsService = clientsService;
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
            var clientInfo = await _clientsService.GetFullClientInfoOrDefault(clientGuid);
            if (clientInfo == default) return NotFound();
            return Ok(clientInfo);
        }

        /// <summary>
        /// Регистрирует нового клиента в системе
        /// </summary>
        /// <param name="registerClientDto">Данные для регистрации</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterClientSuccess>> Post([Required] RegisterClientDto registerClientDto)
        {
            var result = await _clientsService.RegisterClient(registerClientDto);
            if (result.IsSuccess)
                return Ok(result.OkResult);
            return BadRequest(result.ErrorResult);
        }
    }
}