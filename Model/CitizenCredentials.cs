using System;
using Clients.Model.Operations;

namespace Clients.Model
{
    public sealed class CitizenCredentials
    {
        public Guid Guid { get; }
        
        public string Email { get; }

        public string Password { get; }

        private CitizenCredentials(Guid guid, string email, string password)
        {
            Guid = guid;
            Email = email;
            Password = password;
        }

        public static CitizenCredentials New(CreateCitizenCredentials createCitizenCredentials) =>
            new(Guid.NewGuid(), createCitizenCredentials.Email, createCitizenCredentials.Password);
    }
}