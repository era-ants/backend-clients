using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Clients.DataTransfer;
using Clients.Model;
using Clients.Model.Operations;
using Dapper;
using Npgsql;

namespace Clients.Services
{
    public sealed class PostgresDbClientsService : IClientsService, IStatisticsService
    {
        private readonly NpgsqlConnection _dataConnection;

        public PostgresDbClientsService(NpgsqlConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        private readonly string _fullClientDtoSql = @$"
SELECT ""Client"".""Guid"", ""FirstName"", ""LastName"", ""ParentName"", ""HasParentName"", ""CardGuid"", 
""Card"".""ValidFrom"" AS ""CardValidFrom"", ""Card"".""ValidUntil"" AS ""CardValidUntil"", ""ClientTypeId"", 
""ClientSubtypeId""
FROM ""Client"" LEFT JOIN ""Card"" ON ""CardGuid"" = ""Card"".""Guid""
";
        public Task<IEnumerable<FullClientDto>> GetFullClientsInfo() => _dataConnection
                .QueryAsync<FullClientDto>(_fullClientDtoSql);

        public Task<FullClientDto> GetFullClientInfo(Guid clientGuid) =>
            _dataConnection.QueryFirstAsync<FullClientDto>(@$"{_fullClientDtoSql}
WHERE ""Client"".""Guid"" = @clientGuid", clientGuid);

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
                    registerClientDto.PassportSeries,
                    registerClientDto.PassportNumber,
                    registerClientDto.PassportDateOfIssue,
                    registerClientDto.PassportDepartmentName,
                    registerClientDto.PassportDepartmentCode))));
            await _dataConnection.OpenAsync();
            await using var transaction = await _dataConnection.BeginTransactionAsync();
            await _dataConnection.ExecuteAsync($@"
INSERT INTO ""Card"" (""Guid"", ""ValidFrom"", ""ValidUntil"") VALUES (@Guid, @ValidFrom, @ValidUntil)
", newClient.Card);
            await _dataConnection.ExecuteAsync($@"
INSERT INTO ""PassportData"" (""Guid"", ""Series"", ""Number"", ""DateOfIssue"", ""DepartmentName"", ""DepartmentCode"")
VALUES (@Guid, @Series, @Number, @DateOfIssue, @DepartmentName, @DepartmentCode)
", newClient.PassportData);
            await _dataConnection.ExecuteAsync($@"
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
                PassportDataGuid = newClient.PassportData.Guid,
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
                ClientsCount = await _dataConnection.ExecuteScalarAsync<int>($@"SELECT COUNT(""Guid"") FROM ""Client"""),
            };
    }
}