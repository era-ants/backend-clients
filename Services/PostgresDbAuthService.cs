using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Clients.DataTransfer;
using Dapper;
using Npgsql;

namespace Clients.Services
{
    public sealed class PostgresDbAuthService : IAuthService
    {
        private readonly NpgsqlConnection _npgsqlConnection;
        private readonly IClientsService _clientsService;

        public PostgresDbAuthService(NpgsqlConnection npgsqlConnection, IClientsService clientsService)
        {
            _npgsqlConnection = npgsqlConnection;
            _clientsService = clientsService;
        }

        public async Task<FullClientDto> AuthenticateGuest(string phoneNumber, string password)
        {
            var guid = await _npgsqlConnection.QueryFirstOrDefaultAsync<Guid>(@"
SELECT ""Guid"" FROM ""GuestCredentials"" WHERE ""PhoneNumber"" = @PhoneNumber AND ""Password"" = @Password",
                new {PhoneNumber = phoneNumber, Password = password});
            if (guid == default)
                throw new AuthenticationException("Wrong phone number or password");
            return (await _clientsService.GetFullClientInfoOrDefaultAsync(await _npgsqlConnection.QueryFirstAsync<Guid>(
                @"SELECT ""Guid"" FROM ""Client"" WHERE ""GuestCredentialsGuid"" = @Guid", new {Guid = guid})))!;
        }

        public async Task<FullClientDto> AuthenticateCitizen(string email, string password)
        {
            var guid = await _npgsqlConnection.QueryFirstOrDefaultAsync<Guid>(@"
SELECT ""Guid"" FROM ""CitizenCredentials"" WHERE ""Email"" = @Email AND ""Password"" = @Password",
                new {Email = email, Password = password});
            if (guid == default)
                throw new AuthenticationException("Wrong phone number or password");
            return (await _clientsService.GetFullClientInfoOrDefaultAsync(await _npgsqlConnection.QueryFirstAsync<Guid>(
                @"SELECT ""Guid"" FROM ""Client"" WHERE ""CitizenCredentialsGuid"" = @Guid", new {Guid = guid})))!;
        }
    }
}