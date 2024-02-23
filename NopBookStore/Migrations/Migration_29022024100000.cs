using FluentMigrator;

namespace NopBookStore.Migrations
{
    [Migration(29022024100000)]
    public class Migration_29022024100000 : Migration
    {
        
        public override void Down()
        {
            Delete.Table("BookAddress");
        }

        public override void Up()
        {
            Create.Table("BookAddress")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("LibraryName").AsString().NotNullable()
                .WithColumn("Institute").AsString().NotNullable()
                .WithColumn("BookName").AsString().NotNullable();
        }
    }
}
