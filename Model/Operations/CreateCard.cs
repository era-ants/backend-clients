using System;
using System.Threading.Tasks;
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
        private CreateCard(
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
        /// Дата начала срока действия карты
        /// </summary>
        public DateTimeOffset ValidFrom { get; }

        /// <summary>
        /// Дата окончания срока действия карты
        /// </summary>
        public DateTimeOffset ValidUntil { get; }

        /// <summary>
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="validFrom"></param>
        /// <param name="validUntil"></param>
        /// <param name="cardWithGuidExists"></param>
        /// <exception cref="ValidationException"></exception>
        public static async Task<CreateCard> NewAsync(
            Guid guid,
            DateTimeOffset validFrom,
            DateTimeOffset validUntil,
            Func<Guid, Task<bool>> cardWithGuidExists)
        {
            var createCard = new CreateCard(guid, validFrom, validUntil);
            await new CreateCardValidator(cardWithGuidExists).ValidateAndThrowAsync(createCard);
            return createCard;
        }
    }
}