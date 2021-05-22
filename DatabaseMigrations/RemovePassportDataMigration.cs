using System;
using FluentMigrator;

namespace Clients.DatabaseMigrations
{
    [Migration(2)]
    public sealed class RemovePassportDataMigration : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_Client_PassportDataGuid_PassportData_Guid").OnTable("Client");
            Delete.Column("PassportDataGuid").FromTable("Client");
            Delete.Table("PassportData");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}