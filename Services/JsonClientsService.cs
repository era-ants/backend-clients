using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Clients.DataTransfer;
using Clients.Model;
using Clients.Model.Operations;
using FluentValidation;

namespace Clients.Services
{
    /// <summary>
    /// Наивная реализация сервиса клиентов
    /// Данные хранятся в JSON-файле и загружаются при создании сервиса
    /// </summary>
    public sealed class JsonClientsService : IClientsService, IStatisticsService
    {
        private readonly List<Client> _clients;

        public JsonClientsService()
        {
            _clients = File.Exists("clients_data.json")
                ? JsonSerializer.Deserialize<List<Client>>(File.ReadAllText("clients_data.json"))
                : new List<Client>();
        }

        public Task<IEnumerable<FullClientDto>> GetFullClientsInfo() => Task.FromResult(_clients.Select(FullClientDto.FromModel));

        public Task<FullClientDto> GetFullClientInfo(Guid clientGuid) => Task.FromResult(FullClientDto.FromModel(
            _clients.First(client => client.Guid == clientGuid)));

        public async Task<Result<RegisterClientSuccess>> RegisterClient(RegisterClientDto registerClientDto)
        {
            try
            {
                var newClient = Client.New(new CreateClient(
                    new FullName(registerClientDto.FirstName, registerClientDto.LastName, registerClientDto.ParentName,
                        registerClientDto.HasParentName),
                    ClientType.GetById(registerClientDto.ClientTypeId),
                    ClientSubtype.GetById(registerClientDto.ClientSubtypeId),
                    Card.New(new CreateCard(
                        registerClientDto.CardGuid,
                        registerClientDto.CardValidFrom,
                        registerClientDto.CardValidUntil
                    )),
                    PassportData.New(new CreatePassportData(
                        // TODO: реализовать регистрацию паспортных данных
                        default,
                        default,
                        default,
                        default,
                        default
                    ))));

                _clients.Add(newClient);
                await File.WriteAllTextAsync("clients_data.json", JsonSerializer.Serialize(_clients));

                return Result<RegisterClientSuccess>.Ok(new RegisterClientSuccess(newClient.Guid));
            }
            catch (ValidationException validationException)
            {
                return Result<RegisterClientSuccess>.Error(new ErrorResult(validationException.Message));
            }
        }

        public Task<StatisticsDto> GetStatistics() => Task.FromResult(new StatisticsDto
        {
            ClientsCount = _clients.Count,
        });
    }
}