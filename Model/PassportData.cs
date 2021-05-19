using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Clients.Model.Operations;

namespace Clients.Model
{
    /// <summary>
    /// Паспортные данные клиента
    /// </summary>
    public sealed class PassportData
    {
        /// <summary>
        /// Только для сериализаторов и ORM!
        /// </summary>
        [JsonConstructor]
        public PassportData(
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

        /// <summary>
        /// Создаёт новые паспортные данные для регистрации в системе
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        [SuppressMessage("Microsoft.CodeAnalysis.BannedApiAnalyzers", "RS0030")]
        public static PassportData New(CreatePassportData createPassportData)
        {
            if (createPassportData == null)
                throw new ArgumentNullException(nameof(createPassportData));
            return new PassportData(
                createPassportData.Series,
                createPassportData.Number,
                createPassportData.DateOfIssue,
                createPassportData.DepartmentName,
                createPassportData.DepartmentCode);
        }
    }
}