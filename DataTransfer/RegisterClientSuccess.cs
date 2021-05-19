using System;

namespace Clients.DataTransfer
{
    public sealed class RegisterClientSuccess
    {
        public RegisterClientSuccess(Guid clientGuid)
        {
            ClientGuid = clientGuid;
        }

        public Guid ClientGuid { get; }
    }
}