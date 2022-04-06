namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrazione1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            DropIndex("dbo.Pages", new[] { "AuthorId" });
            DropIndex("dbo.Posts", new[] { "AuthorId" });
            AlterColumn("dbo.Comments", "AuthorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Pages", "AuthorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Posts", "AuthorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comments", "AuthorId");
            CreateIndex("dbo.Pages", "AuthorId");
            CreateIndex("dbo.Posts", "AuthorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Posts", new[] { "AuthorId" });
            DropIndex("dbo.Pages", new[] { "AuthorId" });
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            AlterColumn("dbo.Posts", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Pages", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Comments", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Posts", "AuthorId");
            CreateIndex("dbo.Pages", "AuthorId");
            CreateIndex("dbo.Comments", "AuthorId");
        }
    }
}
