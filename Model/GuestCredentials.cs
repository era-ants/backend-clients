using System;
using Clients.Model.Operations;

namespace Clients.Model
{
    public sealed class GuestCredentials
    {
        private GuestCredentials(Guid guid, string phoneNumber, string password)
        {
            Guid = guid;
            PhoneNumber = phoneNumber;
            Password = password;
        }
        
        public Guid Guid { get; }

        public string PhoneNumber { get; }

        public string Password { get; }

        public static GuestCredentials New(CreateGuestCredentials createGuestCredentials) => new(
            Guid.NewGuid(), 
            createGuestCredentials.PhoneNumber,
            createGuestCredentials.Password);
    }
}