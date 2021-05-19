using System;
using System.Diagnostics.CodeAnalysis;
using Clients.Model.Operations;

namespace Clients.Model
{
    /// <summary>
    /// Карта Новороссийска
    /// </summary>
    public sealed class Card
    {
        /// <summary>
        /// Только для сериализаторов и ORM!
        /// </summary>
        public Card(
            Guid guid,
            DateTimeOffset validFrom,
            DateTimeOffset validUntil)
        {
            Guid = guid;
            ValidFrom = validFrom;
            ValidUntil = validUntil;
        }

        // TODO: разобраться, какие существуют идентификаторы карт
        /// <summary>
        /// Уникальный идентификатор карты
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        /// Дата активации карты
        /// </summary>
        public DateTimeOffset ValidFrom { get; }

        /// <summary>
        /// Дата окончания срока действия карты
        /// </summary>
        public DateTimeOffset ValidUntil { get; }

        /// <summary>
        /// Создаёт данные о карте клиента для регистрации в системе
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        [SuppressMessage("Microsoft.CodeAnalysis.BannedApiAnalyzers", "RS0030")]
        public static Card New(CreateCard createCard)
        {
            if (createCard == null) throw new ArgumentNullException(nameof(createCard));
            return new Card(
                createCard.Guid,
                createCard.ValidFrom,
                createCard.ValidUntil);
        }
    }
}