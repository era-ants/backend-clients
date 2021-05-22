using System;

namespace Clients.DataTransfer
{
    public sealed class RegisterGuestDto
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
        /// Телефонный номер клиента
        /// </summary>
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// Пароль клиента для входа в систему
        /// </summary>
        public string Password { get; set; }
    }
}