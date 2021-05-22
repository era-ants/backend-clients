using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clients.DataTransfer;
using Clients.Model;
using Clients.Model.Operations;
using Dapper;
using FluentValidation;
using Npgsql;

namespace Clients.Services
{
    public sealed class PostgresDbClientsService : IClientsService, IStatisticsService
    {
        private readonly NpgsqlConnection _dataConnection;

        private const string _fullClientDtoSql = @"
SELECT ""Client"".""Guid"", ""FirstName"", ""LastName"", ""ParentName"", ""HasParentName"", ""CardGuid"", 
""Card"".""ValidFrom"" AS ""CardValidFrom"", ""Card"".""ValidUntil"" AS ""CardValidUntil"", ""ClientTypeId"", 
""ClientSubtypeId""
FROM ""Client"" LEFT JOIN ""Card"" ON ""CardGuid"" = ""Card"".""Guid""
";

        public PostgresDbClientsService(NpgsqlConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public Task<IEnumerable<FullClientDto>> GetFullClientsInfo() =>
            _dataConnection
                .QueryAsync<FullClientDto>(_fullClientDtoSql);

        public Task<FullClientDto?> GetFullClientInfoOrDefault(Guid clientGuid) =>
            _dataConnection.QueryFirstOrDefaultAsync<FullClientDto>(@$"{_fullClientDtoSql}
WHERE ""Client"".""Guid"" = @ClientGuid", new {ClientGuid = clientGuid});

        public async Task<Result<RegisterClientSuccess>> RegisterClient(RegisterClientDto registerClientDto)
        {
            Client newClient;
            try
            {
                newClient = Client.New(await CreateClient.NewAsync(
                    new FullName(
                        registerClientDto.FirstName,
                        registerClientDto.LastName,
                        registerClientDto.ParentName,
                        registerClientDto.HasParentName),
                    ClientType.GetById(registerClientDto.ClientTypeId),
                    ClientSubtype.GetById(registerClientDto.ClientSubtypeId),
                    await CreateCard.NewAsync(
                        registerClientDto.CardGuid,
                        registerClientDto.CardValidFrom,
                        registerClientDto.CardValidUntil,
                        guid => _dataConnection.QueryFirstAsync<bool>(@"
SELECT EXISTS(SELECT 1 FROM ""Client"" WHERE ""Guid"" = @Guid)", new {Guid = guid}))));
            }
            catch (ValidationException validationException)
            {
                return Result<RegisterClientSuccess>.Error(new ErrorResult(validationException.Message));
            }

            await _dataConnection.OpenAsync();
            await using var transaction = await _dataConnection.BeginTransactionAsync();
            await _dataConnection.ExecuteAsync(@"
INSERT INTO ""Card"" (""Guid"", ""ValidFrom"", ""ValidUntil"") VALUES (@Guid, @ValidFrom, @ValidUntil)
", newClient.Card);
            await _dataConnection.ExecuteAsync(@"
INSERT INTO ""Client"" (""Guid"", ""FirstName"", ""LastName"", ""ParentName"", ""HasParentName"", ""CardGuid"",
""PassportDataGuid"", ""ClientTypeId"", ""ClientSubtypeId"") VALUES (@Guid, @FirstName, @LastName, @ParentName, 
@HasParentName, @CardGuid, @PassportDataGuid, @ClientTypeId, @ClientSubtypeId)
", new
            {
                newClient.Guid,
                newClient.FullName.FirstName,
                newClient.FullName.LastName,
                newClient.FullName.ParentName,
                newClient.FullName.HasParentName,
                CardGuid = newClient.Card.Guid,
                ClientTypeId = newClient.ClientType.Id,
                ClientSubtypeId = newClient.ClientSubtype.Id
            });
            await transaction.CommitAsync();
            await _dataConnection.CloseAsync();
            return Result<RegisterClientSuccess>.Ok(new RegisterClientSuccess(newClient.Guid));
        }

        public async Task<StatisticsDto> GetStatistics() =>
            new()
            {
                ClientsCount = await _dataConnection.ExecuteScalarAsync<int>(@"SELECT COUNT(""Guid"") FROM ""Client""")
            };
    }
}