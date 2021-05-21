using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Clients.Hubs
{
    public interface IClientsHubClient
    {
        Task RecieveNewClientGuid(Guid newClientGuid);
    }
    
    public sealed class ClientsHub : Hub<IClientsHubClient>
    {
        public Task ReportAboutNewClient(Guid newClientGuid) => Clients.All.RecieveNewClientGuid(newClientGuid);
    }
}