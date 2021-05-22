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
        private const string _fullClientDtoSql = @"
SELECT ""Client"".""Guid"", ""FirstName"", ""LastName"", ""ParentName"", ""HasParentName"", ""CardGuid"", 
""Card"".""ValidFrom"" AS ""CardValidFrom"", ""Card"".""ValidUntil"" AS ""CardValidUntil"", ""ClientTypeId"", 
""ClientSubtypeId"", ""Email"", ""PhoneNumber""
FROM ""Client"" LEFT JOIN ""Card"" ON ""CardGuid"" = ""Card"".""Guid"" 
    LEFT JOIN ""GuestCredentials"" ON ""GuestCredentialsGuid"" = ""GuestCredentials"".""Guid""
    LEFT JOIN ""CitizenCredentials"" ON ""CitizenCredentialsGuid"" = ""CitizenCredentials"".""Guid""
";

        private readonly NpgsqlConnection _dataConnection;

        public PostgresDbClientsService(NpgsqlConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public Task<IEnumerable<FullClientDto>> GetFullClientsInfo() =>
            _dataConnection
                .QueryAsync<FullClientDto>(_fullClientDtoSql);

        public Task<FullClientDto?> GetFullClientInfoOrDefaultAsync(Guid clientGuid) =>
            _dataConnection.QueryFirstOrDefaultAsync<FullClientDto>(@$"{_fullClientDtoSql}
WHERE ""Client"".""Guid"" = @ClientGuid", new {ClientGuid = clientGuid});

        public async Task<Result<RegisterClientSuccess>> RegisterGuest(RegisterGuestDto registerGuestDto)
        {
            Client newClient;
            try
            {
                newClient = Client.New(await CreateClient.NewAsync(
                    new FullName(
                        registerGuestDto.FirstName,
                        registerGuestDto.LastName,
                        registerGuestDto.ParentName,
                        registerGuestDto.HasParentName),
                    ClientType.Guest, 
                    ClientSubtype.GetById(registerGuestDto.ClientSubtypeId),
                    await CreateCard.NewAsync(
                        registerGuestDto.CardGuid,
                        registerGuestDto.CardValidFrom,
                        registerGuestDto.CardValidUntil,
                        guid => _dataConnection.QueryFirstAsync<bool>(@"
SELECT EXISTS(SELECT 1 FROM ""Client"" WHERE ""Guid"" = @Guid)", new {Guid = guid})),
                    null,
                    await CreateGuestCredentials.NewAsync(
                        registerGuestDto.PhoneNumber,
                        registerGuestDto.Password,
                        (phoneNumber, _) => _dataConnection.QueryFirstAsync<bool>(@"
SELECT EXISTS(SELECT 1 FROM ""GuestCredentials"" WHERE ""PhoneNumber"" = @PhoneNumber)", new {PhoneNumber = phoneNumber})
                        )));
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
INSERT INTO ""GuestCredentials"" (""Guid"", ""PhoneNumber"", ""Password"") VALUES (@Guid, @PhoneNumber, @Password)",
                newClient.GuestCredentials);
            await _dataConnection.ExecuteAsync(@"
INSERT INTO ""Client"" (""Guid"", ""FirstName"", ""LastName"", ""ParentName"", ""HasParentName"", ""CardGuid"",
""ClientTypeId"", ""ClientSubtypeId"", ""GuestCredentialsGuid"") VALUES (@Guid, @FirstName, @LastName, @ParentName, 
@HasParentName, @CardGuid, @ClientTypeId, @ClientSubtypeId, @GuestCredentialsGuid)
", new
            {
                newClient.Guid,
                newClient.FullName.FirstName,
                newClient.FullName.LastName,
                newClient.FullName.ParentName,
                newClient.FullName.HasParentName,
                CardGuid = newClient.Card.Guid,
                ClientTypeId = newClient.ClientType.Id,
                ClientSubtypeId = newClient.ClientSubtype.Id,
                GuestCredentialsGuid = newClient.GuestCredentials!.Guid,
            });
            await transaction.CommitAsync();
            await _dataConnection.CloseAsync();
            return Result<RegisterClientSuccess>.Ok(new RegisterClientSuccess(newClient.Guid));
        }

        public async Task<Result<RegisterClientSuccess>> RegisterCitizen(RegisterCitizenDto registerCitizenDto)
        {
            Client newClient;
            try
            {
                newClient = Client.New(await CreateClient.NewAsync(
                    new FullName(
                        registerCitizenDto.FirstName,
                        registerCitizenDto.LastName,
                        registerCitizenDto.ParentName,
                        registerCitizenDto.HasParentName),
                    ClientType.Guest, 
                    ClientSubtype.GetById(registerCitizenDto.ClientSubtypeId),
                    await CreateCard.NewAsync(
                        registerCitizenDto.CardGuid,
                        registerCitizenDto.CardValidFrom,
                        registerCitizenDto.CardValidUntil,
                        guid => _dataConnection.QueryFirstAsync<bool>(@"
SELECT EXISTS(SELECT 1 FROM ""Client"" WHERE ""Guid"" = @Guid)", new {Guid = guid})),
                    await CreateCitizenCredentials.NewAsync(
                        registerCitizenDto.Email,
                        registerCitizenDto.Password,
                        (email, _) => _dataConnection.QueryFirstAsync<bool>(@"
SELECT EXISTS(SELECT 1 FROM ""CitizenCredentials"" WHERE ""Email"" = @Email)", new {Email = email})),
                        null)
                );
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
INSERT INTO ""CitizenCredentials"" (""Guid"", ""Email"", ""Password"") VALUES (@Guid, @Email, @Password)",
                newClient.CitizenCredentials);
            await _dataConnection.ExecuteAsync(@"
INSERT INTO ""Client"" (""Guid"", ""FirstName"", ""LastName"", ""ParentName"", ""HasParentName"", ""CardGuid"",
""ClientTypeId"", ""ClientSubtypeId"", ""CitizenCredentialsGuid"") VALUES (@Guid, @FirstName, @LastName, @ParentName, 
@HasParentName, @CardGuid, @ClientTypeId, @ClientSubtypeId, @CitizenCredentialsGuid)
", new
            {
                newClient.Guid,
                newClient.FullName.FirstName,
                newClient.FullName.LastName,
                newClient.FullName.ParentName,
                newClient.FullName.HasParentName,
                CardGuid = newClient.Card.Guid,
                ClientTypeId = newClient.ClientType.Id,
                ClientSubtypeId = newClient.ClientSubtype.Id,
                CitizenCredentialsGuid = newClient.CitizenCredentials!.Guid
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