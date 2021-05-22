using System;
using System.Threading;
using System.Threading.Tasks;
using Clients.Model.Validators;
using FluentValidation;

namespace Clients.Model.Operations
{
    public sealed class CreateCitizenCredentials
    {
        private CreateCitizenCredentials(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }

        public string Password { get; }

        public static async Task<CreateCitizenCredentials> NewAsync(
            string email,
            string password,
            Func<string, CancellationToken, Task<bool>> isEmailAlreadyRegistered)
        {
            var createCitizenCredentials = new CreateCitizenCredentials(email, password);
            await new CreateCitizenCredentialsValidator(isEmailAlreadyRegistered).ValidateAndThrowAsync(
                createCitizenCredentials);
            return createCitizenCredentials;
        }
    }
}