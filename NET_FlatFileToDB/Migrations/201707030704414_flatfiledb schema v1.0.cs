namespace NET_FlatFileToDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flatfiledbschemav10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlatFiles",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                    })
                .PrimaryKey(t => t.FileId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FlatFiles");
        }
    }
}
