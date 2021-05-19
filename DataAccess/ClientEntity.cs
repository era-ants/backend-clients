using System;
using LinqToDB.Mapping;

namespace Clients.DataAccess
{
    public sealed class ClientEntity
    {
        [PrimaryKey]
        public Guid Guid { get; set; }
    }
}