using System;
using System.Diagnostics.CodeAnalysis;
using Clients.Model.Operations;

namespace Clients.Model
{
    /// <summary>
    /// Клиент системы (владелец карты Новороссийска)
    /// </summary>
    public sealed class Client
    {
        /// <summary>
        /// Только для ORM и сериализаторов!
        /// </summary>
        public Client(
            Guid guid,
            FullName fullName,
            ClientType clientType,
            ClientSubtype clientSubtype,
            Card card,
            UserRole userRole,
            CitizenCredentials? citizenCredentials,
            GuestCredentials? guestCredentials)
        {
            Guid = guid;
            FullName = fullName;
            ClientType = clientType;
            Card = card;
            ClientSubtype = clientSubtype;
            UserRole = userRole;
            CitizenCredentials = citizenCredentials;
            GuestCredentials = guestCredentials;
        }

        /// <summary>
        /// Guid клиента
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        /// ФИО клиента
        /// </summary>
        public FullName FullName { get; }

        /// <summary>
        /// Тип клиента
        /// </summary>
        public ClientType ClientType { get; }

        /// <summary>
        /// Подтип клиента
        /// </summary>
        public ClientSubtype ClientSubtype { get; }

        /// <summary>
        /// Данные о карте клиента
        /// </summary>
        public Card Card { get; }

        public UserRole UserRole { get; }

        // TODO: переделать в аналог ADT?

        public CitizenCredentials? CitizenCredentials { get; }

        public GuestCredentials? GuestCredentials { get; }

        /// <summary>
        /// Создаёт нового клиента для регистрации в системе
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        [SuppressMessage("Microsoft.CodeAnalysis.BannedApiAnalyzers", "RS0030")]
        public static Client New(CreateClient createClient)
        {
            if (createClient == null)
                throw new ArgumentNullException(nameof(createClient));
            return new Client(
                Guid.NewGuid(),
                createClient.FullName,
                createClient.ClientType,
                createClient.ClientSubtype,
                Card.New(createClient.Card),
                createClient.ClientType == ClientType.Citizen ? UserRole.Citizen : UserRole.Guest,
                createClient.CitizenCredentials == null ? null : CitizenCredentials.New(createClient.CitizenCredentials),
                createClient.GuestCredentials == null ? null : GuestCredentials.New(createClient.GuestCredentials));
        }
    }
}