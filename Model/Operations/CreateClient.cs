using System.Collections.Generic;
using Clients.Model.Validators;
using FluentValidation;

namespace Clients.Model.Operations
{
    public sealed class CreateClient
    {
        /// <summary>
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        public CreateClient(FullName fullName,
            ClientType clientType,
            ClientSubtype clientSubtype,
            Card card,
            PassportData passportData)
        {
            FullName = fullName;
            ClientType = clientType;
            ClientSubtype = clientSubtype;
            Card = card;
            PassportData = passportData;
            new CreateClientValidator().ValidateAndThrow(this);
        }

        public FullName FullName { get; }
        public ClientType ClientType { get; }
        public ClientSubtype ClientSubtype { get; }
        public Card Card { get; }
        public PassportData PassportData { get; }
    }
}