using System;

namespace Clients.DataTransfer
{
    /// <summary>
    /// Данные для регистрации клиента в системе
    /// </summary>
    public sealed class RegisterClientDto
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
        /// Тип клиента. 1 - Гость, 2 - Житель
        /// </summary>
        public int ClientTypeId { get; set; }

        /// <summary>
        /// Подтип клиента. 1 - Без льгот, 2 - Пожилой, 3 - Инвалид, 4 - Ветеран
        /// </summary>
        public int ClientSubtypeId { get; set; }
        
        /// <summary>
        /// Серия паспорта клиента
        /// </summary>
        public short PassportSeries { get; set; }
        
        /// <summary>
        /// Номер паспорта клиента
        /// </summary>
        public int PassportNumber { get; set; }
        
        /// <summary>
        /// Дата выдачи паспорта клиенту
        /// </summary>
        public DateTimeOffset PassportDateOfIssue { get; set; }
        
        /// <summary>
        /// Наименование органа, выдавшего паспорт 
        /// </summary>
        public string PassportDepartmentName { get; set; }
        
        /// <summary>
        /// Код подразделения, выдавшего паспорт
        /// </summary>
        public string PassportDepartmentCode { get; set; }
    }
}