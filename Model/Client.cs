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
            PassportData passportData)
        {
            Guid = guid;
            FullName = fullName;
            ClientType = clientType;
            Card = card;
            ClientSubtype = clientSubtype;
            PassportData = passportData;
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

        // TODO: обязательно ли туристам предоставлять нам паспортные данные для оформления карты?
        /// <summary>
        /// Паспортные данные клиента
        /// </summary>
        public PassportData PassportData { get; }

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
                createClient.Card,
                createClient.PassportData);
        }
    }
}