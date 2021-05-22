using System;

namespace Clients.DataTransfer
{
    public sealed class RegisterCitizenDto
    {
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
        /// Уникальный идентификатор карты клиента
        /// </summary>
        public Guid CardGuid { get; set; }

        /// <summary>
        /// Дата начала срока действия карты
        /// </summary>
        public DateTimeOffset CardValidFrom { get; set; }

        /// <summary>
        /// Дата окончания срока действия карты
        /// </summary>
        public DateTimeOffset CardValidUntil { get; set; }

        /// <summary>
        /// Подтип клиента. 1 - Без льгот, 2 - Пожилой, 3 - Инвалид, 4 - Ветеран
        /// </summary>
        public int ClientSubtypeId { get; set; }

        /// <summary>
        /// Email клиента для входа в ЕСИА (мок)
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Пароль клиента для входа в ЕСИА (мок)
        /// </summary>
        public string Password { get; set; }
    }
}