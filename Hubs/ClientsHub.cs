using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Clients.Hubs
{
    // public interface IClientsHubClient
    // {
    //     Task RecieveNewClientGuid(Guid newClientGuid);
    // }
    
    public sealed class ClientsHub : Hub
    {
        private readonly ILogger _logger;

        public ClientsHub(ILogger logger)
        {
            _logger = logger;
        }

        public void Send(string user, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Someone has connected! ConnectionId: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Someone has disconnected! ConnectionId: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        // public Task ReportAboutNewClient(Guid newClientGuid) => Clients.All.RecieveNewClientGuid(newClientGuid);
    }
}