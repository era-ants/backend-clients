using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clients.DataAccess;
using Clients.DataTransfer;
using Clients.Model;
using Clients.Model.Operations;
using LinqToDB;

namespace Clients.Services
{
    public sealed class DbClientsService : IClientsService, IStatisticsService
    {
        private readonly AppDataConnection _dataConnection;

        public DbClientsService(AppDataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public async Task<IEnumerable<FullClientDto>> GetFullClientsInfo() =>
            (await _dataConnection.Clients.ToArrayAsync())
            .Select(FullClientDto.FromModel);

        public async Task<FullClientDto> GetFullClientInfo(Guid clientGuid) =>
            FullClientDto.FromModel(
                await _dataConnection.Clients.SingleAsync(x => x.Guid == clientGuid));

        public async Task<Result<RegisterClientSuccess>> RegisterClient(RegisterClientDto registerClientDto)
        {
            var newClient = Client.New(new CreateClient(
                new FullName(
                    registerClientDto.FirstName,
                    registerClientDto.LastName,
                    registerClientDto.ParentName,
                    registerClientDto.HasParentName),
                ClientType.GetById(registerClientDto.ClientTypeId),
                ClientSubtype.GetById(registerClientDto.ClientSubtypeId),
                Card.New(new CreateCard(
                    registerClientDto.CardGuid,
                    registerClientDto.CardValidFrom,
                    registerClientDto.CardValidUntil)),
                PassportData.New(new CreatePassportData(
                    default,
                    default,
                    default,
                    default,
                    default)),
                await _dataConnection.Clients.ToArrayAsync()));
            await _dataConnection.InsertAsync(newClient);
            return Result<RegisterClientSuccess>.Ok(new RegisterClientSuccess(newClient.Guid));
        }

        public async Task<StatisticsDto> GetStatistics() =>
            new()
            {
                ClientsCount = await _dataConnection.Clients.CountAsync(),
            };
    }
}