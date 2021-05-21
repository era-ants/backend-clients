using System;
using System.Threading.Tasks;
using Clients.Model.Validators;
using FluentValidation;

namespace Clients.Model.Operations
{
    public sealed class CreatePassportData
    {
        private CreatePassportData(
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
        /// 
        /// </summary>
        /// <param name="series"></param>
        /// <param name="number"></param>
        /// <param name="dateOfIssue"></param>
        /// <param name="departmentName"></param>
        /// <param name="departmentCode"></param>
        /// <param name="isPassportWithSeriesAndNumberRegistered"></param>
        /// <exception cref="ValidationException"></exception>
        public static async Task<CreatePassportData> NewAsync(
            short series,
            int number,
            DateTimeOffset dateOfIssue,
            string departmentName,
            string departmentCode,
            Func<short, int, Task<bool>> isPassportWithSeriesAndNumberRegistered)
        {
            var createPassportData = new CreatePassportData(series, number, dateOfIssue, departmentName, departmentCode);
            await new CreatePassportDataValidator(isPassportWithSeriesAndNumberRegistered)
                .ValidateAndThrowAsync(createPassportData);
            return createPassportData;
        }
    }
}