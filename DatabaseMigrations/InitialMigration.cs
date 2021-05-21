using FluentMigrator;
using FluentMigrator.Postgres;

namespace Clients.DatabaseMigrations
{
    [Migration(1)]
    public sealed class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("PassportData")
                .WithColumn("Guid").AsGuid().PrimaryKey()
                .WithColumn("Series").AsInt16().NotNullable()
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("DateOfIssue").AsDateTimeOffset().NotNullable()
                .WithColumn("DepartmentName").AsString().NotNullable()
                .WithColumn("DepartmentCode").AsString().NotNullable()
                ;
            Create.Index().OnTable("PassportData").OnColumn("Series").Unique().Include("Number");

            Create.Table("Card")
                .WithColumn("Guid").AsGuid().PrimaryKey()
                .WithColumn("ValidFrom").AsDateTimeOffset().NotNullable()
                .WithColumn("ValidUntil").AsDateTimeOffset().NotNullable()
                ;

            Create.Table("Client")
                .WithColumn("Guid").AsGuid().PrimaryKey()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().NotNullable()
                .WithColumn("ParentName").AsString().NotNullable()
                .WithColumn("HasParentName").AsBoolean().NotNullable()
                .WithColumn("CardGuid").AsGuid().NotNullable().ForeignKey("Card", "Guid")
                .WithColumn("PassportDataGuid").AsGuid().NotNullable().ForeignKey("PassportData", "Guid")
                .WithColumn("ClientTypeId").AsInt32().NotNullable()
                .WithColumn("ClientSubtypeId").AsInt32().NotNullable()
                ;
        }

        public override void Down()
        {
            Delete.Table("Client");
            Delete.Table("Card");
            Delete.Table("PassportData");
        }
    }
}