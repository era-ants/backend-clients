using FluentMigrator;

namespace Clients.DatabaseMigrations
{
    [Migration(3)]
    public sealed class AddCredentialsMigration : Migration
    {
        public override void Up()
        {
            Create.Table("GuestCredentials")
                .WithColumn("Guid").AsGuid().PrimaryKey()
                .WithColumn("PhoneNumber").AsString().NotNullable()
                .WithColumn("Password").AsString().NotNullable()
                ;
            Create.Table("CitizenCredentials")
                .WithColumn("Guid").AsGuid().PrimaryKey()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Password").AsString().NotNullable()
                ;
            Create.Column("GuestCredentialsGuid").OnTable("Client")
                .AsGuid().Nullable().ForeignKey("GuestCredentials", "Guid")
                ;
            Create.Column("CitizenCredentialsGuid").OnTable("Client")
                .AsGuid().Nullable().ForeignKey("CitizenCredentials", "Guid")
                ;
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}