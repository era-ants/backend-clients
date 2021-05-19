using System;
using Clients.Model.Validators;
using FluentValidation;

namespace Clients.Model.Operations
{
    public sealed class CreateCard
    {
        /// <summary>
        /// </summary>
        /// <param name="guid">Уникальный идентификатор карты</param>
        /// <param name="validFrom">Дата начала срока действия карты</param>
        /// <param name="validUntil">Дата окончания срока действия карты</param>
        public CreateCard(
            Guid guid,
            DateTimeOffset validFrom,
            DateTimeOffset validUntil)
        {
            Guid = guid;
            ValidFrom = validFrom;
            ValidUntil = validUntil;
            new CreateCardValidator().ValidateAndThrow(this);
        }

        // TODO: разобраться, какие существуют идентификаторы карт
        /// <summary>
        /// Уникальный идентификатор карты
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        /// Дата начала срока действия карты
        /// </summary>
        public DateTimeOffset ValidFrom { get; }

        /// <summary>
        /// Дата окончания срока действия карты
        /// </summary>
        public DateTimeOffset ValidUntil { get; }
    }
}