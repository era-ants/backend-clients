using System.Threading.Tasks;
using Clients.Model.Validators;
using FluentValidation;

namespace Clients.Model.Operations
{
    public sealed class CreateClient
    {
        /// <summary>
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        private CreateClient(FullName fullName,
            ClientType clientType,
            ClientSubtype clientSubtype,
            CreateCard newCard,
            CreatePassportData newPassportData)
        {
            FullName = fullName;
            ClientType = clientType;
            ClientSubtype = clientSubtype;
            Card = newCard;
            PassportData = newPassportData;
            new CreateClientValidator().ValidateAndThrow(this);
        }

        public FullName FullName { get; }
        public ClientType ClientType { get; }
        public ClientSubtype ClientSubtype { get; }
        public CreateCard Card { get; }
        public CreatePassportData PassportData { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="clientType"></param>
        /// <param name="clientSubtype"></param>
        /// <param name="newCard"></param>
        /// <param name="newPassportData"></param>
        /// <exception cref="ValidationException"></exception>
        public static async Task<CreateClient> NewAsync(
            FullName fullName,
            ClientType clientType,
            ClientSubtype clientSubtype,
            CreateCard newCard,
            CreatePassportData newPassportData)
        {
            var createClient = new CreateClient(fullName, clientType, clientSubtype, newCard, newPassportData);
            await new CreateClientValidator().ValidateAndThrowAsync(createClient);
            return createClient;
        }
    }
}