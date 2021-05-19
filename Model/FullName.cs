using System;
using System.Text.Json.Serialization;

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
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        [JsonConstructor]
        public FullName(string firstName, string lastName, string parentName, bool hasParentName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentNullException(
                    nameof(firstName),
                    $"Argument \"{nameof(firstName)}\" must be specified.");
            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentNullException(
                    nameof(lastName),
                    $"Argument \"{nameof(lastName)}\" must be specified.");
            if (hasParentName && string.IsNullOrEmpty(parentName))
                throw new ArgumentNullException(
                    nameof(parentName),
                    $"If argument \"{nameof(hasParentName)}\" is true, \"{nameof(parentName)}\" must be specified.");
            if (!hasParentName && !string.IsNullOrEmpty(parentName))
                throw new ArgumentException(
                    $"If argument \"{nameof(hasParentName)}\" is false, \"{nameof(parentName)}\" must be empty.",
                    nameof(parentName));

            // TODO: добавить проверки корректности ФИО, если может не быть отчества - явное указание на это

            FirstName = firstName;
            LastName = lastName;
            ParentName = hasParentName ? parentName : string.Empty;
            HasParentName = hasParentName;
            _fullName = HasParentName
                ? $"{LastName} {FirstName} {ParentName}"
                : $"{LastName} {FirstName}";
            _shortName = HasParentName
                ? $"{LastName} {FirstName[0]}. {ParentName[0]}."
                : $"{LastName} {FirstName[0]}.";
        }

        public FullName(string fullName)
        {
            // TODO: добаваить парсинг ФИО
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string ParentName { get; }
        public bool HasParentName { get; }

        public override string ToString() => _fullName;
    }
}