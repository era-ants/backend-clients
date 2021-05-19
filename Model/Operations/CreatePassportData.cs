using System;
using Clients.Model.Validators;
using FluentValidation;

namespace Clients.Model.Operations
{
    public sealed class CreatePassportData
    {
        public CreatePassportData(
            short series,
            int number,
            DateTimeOffset dateOfIssue,
            string departmentName,
            string departmentCode)
        {
            Series = series;
            Number = number;
            DateOfIssue = dateOfIssue;
            DepartmentName = departmentName;
            DepartmentCode = departmentCode;
            new CreatePassportDataValidator().ValidateAndThrow(this);
        }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        public short Series { get; }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Дата выдачи паспорта
        /// </summary>
        public DateTimeOffset DateOfIssue { get; }

        /// <summary>
        /// Наименование органа, выдавшего паспорт
        /// </summary>
        public string DepartmentName { get; }

        /// <summary>
        /// Код подразделения
        /// </summary>
        public string DepartmentCode { get; }
    }
}