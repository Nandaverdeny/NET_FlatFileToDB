namespace NET_FlatFileToDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flatfiledbschemav13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UploadedData",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UploadedData");
        }
    }
}
