namespace Data.Migration
{
    using System.Data.Entity.Migrations;
    
    public partial class Everything : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Activations", "UserId", "Users");
            DropIndex("Activations", new[] { "UserId" });
            CreateTable(
                "Nouns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Word = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Adjectives",
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
            
            DropTable("Activations");
        }
        
        public override void Down()
        {
            CreateTable(
                "Activations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ConfirmationToken = c.String(maxLength: 128),
                        ActivatedDate = c.DateTime(storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("Links", new[] { "HappyLink_Id" });
            DropForeignKey("Links", "HappyLink_Id", "HappyLinks");
            DropTable("HappyLinks");
            DropTable("Links");
            DropTable("Adjectives");
            DropTable("Nouns");
            CreateIndex("Activations", "UserId");
            AddForeignKey("Activations", "UserId", "Users", "Id", cascadeDelete: true);
        }
    }
}
