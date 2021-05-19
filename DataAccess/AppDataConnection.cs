using Clients.Model;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace Clients.DataAccess
{
    public sealed class AppDataConnection : DataConnection
    {
        public ITable<Client> Clients => GetTable<Client>();
        
        public AppDataConnection(LinqToDbConnectionOptions<AppDataConnection> options)
            :base(options)
        {

        }
    }
}