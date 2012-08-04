namespace Data.Migration
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedLinksAndNouns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Nouns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Word = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OriginalLink = c.String(),
                        HappyLink_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("HappyLinks", t => t.HappyLink_Id, cascadeDelete: true)
                .Index(t => t.HappyLink_Id);
            
            CreateTable(
                "HappyLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        LastAccessed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("Links", new[] { "HappyLink_Id" });
            DropForeignKey("Links", "HappyLink_Id", "HappyLinks");
            DropTable("HappyLinks");
            DropTable("Links");
            DropTable("Nouns");
        }
    }
}
