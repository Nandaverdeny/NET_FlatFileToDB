namespace NET_FlatFileToDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flatfiledbschemav12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlatFileWithData",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FilePath = c.String(),
                        Description = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.FileId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FlatFileWithData");
        }
    }
}
