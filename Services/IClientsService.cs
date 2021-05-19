using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clients.DataTransfer;

namespace Clients.Services
{
    public interface IClientsService
    {
        Task<IEnumerable<FullClientDto>> GetFullClientsInfo();

        Task<FullClientDto> GetFullClientInfo(Guid clientGuid);

        Task<Result<RegisterClientSuccess>> RegisterClient(RegisterClientDto registerClientDto);
    }
}