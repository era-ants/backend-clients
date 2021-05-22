using System;
using System.Threading;
using System.Threading.Tasks;
using Clients.Model.Validators;
using FluentValidation;

namespace Clients.Model.Operations
{
    public sealed class CreateGuestCredentials
    {
        private CreateGuestCredentials(string phoneNumber, string password)
        {
            PhoneNumber = phoneNumber;
            Password = password;
        }

        public string PhoneNumber { get; }
        
        public string Password { get; }

        public static async Task<CreateGuestCredentials> NewAsync(
            string phoneNumber,
            string password,
            Func<string, CancellationToken, Task<bool>> isPhoneNumberRegistered)
        {
            var createGuestCredentials = new CreateGuestCredentials(phoneNumber, password);
            await new CreateGuestCredentialsValidator(isPhoneNumberRegistered).ValidateAndThrowAsync(
                createGuestCredentials);
            return createGuestCredentials;
        }
    }
}