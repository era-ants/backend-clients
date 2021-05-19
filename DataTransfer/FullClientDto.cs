using System;
using Clients.Model;

namespace Clients.DataTransfer
{
    public sealed class FullClientDto
    {
        /// <summary>
        /// Guid клиента
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// true, если у клиента есть отчество
        /// </summary>
        public bool HasParentName { get; set; }

        /// <summary>
        /// Уникальный идентификатор карты
        /// </summary>
        public Guid CardGuid { get; set; }

        /// <summary>
        /// Дата начала срока действия карты клиента
        /// </summary>
        public DateTimeOffset CardValidFrom { get; set; }

        /// <summary>
        /// Дата окончания срока действия карты клиента
        /// </summary>
        public DateTimeOffset CardValidUntil { get; set; }

        /// <summary>
        /// Тип клиента. 1 - Гость, 2 - Житель
        /// </summary>
        public int ClientTypeId { get; set; }

        /// <summary>
        /// Подтип клиента. 1 - Без льгот, 2 - Пожилой, 3 - Инвалид, 4 - Ветеран
        /// </summary>
        public int ClientSubtypeId { get; set; }

        public static FullClientDto FromModel(Client client) =>
            new()
            {
                Guid = client.Guid,
                FirstName = client.FullName.FirstName,
                LastName = client.FullName.LastName,
                ParentName = client.FullName.ParentName,
                HasParentName = client.FullName.HasParentName,
                CardGuid = client.Card.Guid,
                CardValidFrom = client.Card.ValidFrom,
                CardValidUntil = client.Card.ValidUntil,
                ClientTypeId = client.ClientType.Id,
            };
    }
}