using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Clients.DataTransfer;
using Clients.Services;
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
        public Task<FullClientDto> Get([Required] Guid clientGuid) => _clientsService.GetFullClientInfo(clientGuid);

        /// <summary>
        /// Регистрирует нового клиента в системе
        /// </summary>
        /// <param name="registerClientDto">Данные для регистрации</param>
        [HttpPost]
        public Task<Result<RegisterClientSuccess>> Post([Required] RegisterClientDto registerClientDto) =>
            _clientsService.RegisterClient(registerClientDto);
    }
}