using System.Collections.Generic;
using System.Linq;

namespace Clients.Model
{
    /// <summary>
    /// Подтип клиента
    /// </summary>
    public sealed class ClientSubtype
    {
        private ClientSubtype(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }

        /// <summary>
        /// Гражданин без льгот
        /// </summary>
        public static ClientSubtype Regular { get; } = new(1, "Без льгот");

        /// <summary>
        /// Гражданин пожилого возраста
        /// </summary>
        public static ClientSubtype Senior { get; } = new(2, "Пожилой");

        /// <summary>
        /// Инвалид
        /// </summary>
        public static ClientSubtype Disabled { get; } = new(3, "Инвалид");

        /// <summary>
        /// Ветеран
        /// </summary>
        public static ClientSubtype Veteran { get; } = new(4, "Ветеран");

        public static IEnumerable<ClientSubtype> GetAll()
        {
            return new[] {Regular, Senior, Disabled, Veteran};
        }

        public static ClientSubtype GetById(int id)
        {
            return GetAll().First(x => x.Id == id);
        }

        private bool Equals(ClientSubtype other) => Id == other.Id;

        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj) || obj is ClientSubtype other && Equals(other);

        public override int GetHashCode() => Id;
    }
}