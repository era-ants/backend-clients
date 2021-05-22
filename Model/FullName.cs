using System.Text.Json.Serialization;
using Clients.Model.Validators;
using FluentValidation;

namespace Clients.Model
{
    /// <summary>
    /// ФИО по правилам РФ
    /// </summary>
    public sealed class FullName
    {
        private readonly string _fullName;

        private readonly string _shortName;

        /// <summary>
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="parentName">Отчество</param>
        /// <param name="hasParentName">
        /// Явное указание, есть ли отчество. Если true, <see cref="parentName" /> должно быть не пустым.
        /// Если false, <see cref="parentName" /> должно быть пустым
        /// </param>
        /// <exception cref="ValidationException"></exception>
        [JsonConstructor]
        public FullName(string firstName, string lastName, string parentName, bool hasParentName)
        {
            FirstName = firstName;
            LastName = lastName;
            ParentName = string.IsNullOrEmpty(parentName) ? string.Empty : parentName;
            HasParentName = hasParentName;

            new FullNameValidator().ValidateAndThrow(this);

            _fullName = HasParentName
                ? $"{LastName} {FirstName} {ParentName}"
                : $"{LastName} {FirstName}";
            _shortName = HasParentName
                ? $"{LastName} {FirstName[0]}. {ParentName[0]}."
                : $"{LastName} {FirstName[0]}.";
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string ParentName { get; }
        public bool HasParentName { get; }

        public override string ToString() => _fullName;
    }
}