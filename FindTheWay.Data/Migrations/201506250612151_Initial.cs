namespace FindTheWay.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Edges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NodeFromId = c.Long(nullable: false),
                        NodeToId = c.Long(nullable: false),
                        Length = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nodes", t => t.NodeFromId)
                .ForeignKey("dbo.Nodes", t => t.NodeToId)
                .Index(t => t.Id)
                .Index(t => t.NodeFromId)
                .Index(t => t.NodeToId);
            
            CreateTable(
                "dbo.Nodes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Edges", "NodeToId", "dbo.Nodes");
            DropForeignKey("dbo.Edges", "NodeFromId", "dbo.Nodes");
            DropIndex("dbo.Nodes", new[] { "Id" });
            DropIndex("dbo.Edges", new[] { "NodeToId" });
            DropIndex("dbo.Edges", new[] { "NodeFromId" });
            DropIndex("dbo.Edges", new[] { "Id" });
            DropTable("dbo.Nodes");
            DropTable("dbo.Edges");
        }
    }
}
