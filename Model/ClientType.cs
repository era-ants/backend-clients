using System;
using System.Collections.Generic;
using System.Linq;

namespace Clients.Model
{
    /// <summary>
    /// Тип клиента системы
    /// </summary>
    public sealed class ClientType
    {
        // TODO: обдумать ситуацию с туристами-иностранцами

        private ClientType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }

        /// <summary>
        /// Гость Новороссийска (турист)
        /// </summary>
        public static ClientType Guest { get; } = new(1, "Гость");

        /// <summary>
        /// Житель Новороссийска
        /// </summary>
        public static ClientType Citizen { get; } = new(2, "Житель");

        public static IEnumerable<ClientType> GetAll()
        {
            return new[] {Guest, Citizen};
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static ClientType GetById(int id)
        {
            return GetAll().First(clientType => clientType.Id == id);
        }

        private bool Equals(ClientType other) => Id == other.Id;

        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj) || obj is ClientType other && Equals(other);

        public override int GetHashCode() => Id;
    }
}