using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clients.DataTransfer;

namespace Clients.Services
{
    public interface IClientsService
    {
        Task<IEnumerable<FullClientDto>> GetFullClientsInfo();

        /// <summary>
        /// Возвращает полную информацию по выбранному клиенту системы.
        /// Если клиента с указанным Guid не существует, возвращает null.
        /// </summary>
        /// <param name="clientGuid"></param>
        Task<FullClientDto?> GetFullClientInfoOrDefault(Guid clientGuid);

        Task<Result<RegisterClientSuccess>> RegisterClient(RegisterClientDto registerClientDto);
    }
}