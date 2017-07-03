namespace NET_FlatFileToDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flatfiledbschemav11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlatFiles", "FilePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FlatFiles", "FilePath");
        }
    }
}
