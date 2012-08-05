namespace Data.Migration
{
    using System.Data.Entity.Migrations;
    
    public partial class Adjectives : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Activations", "UserId", "Users");
            DropIndex("Activations", new[] { "UserId" });
            CreateTable(
                "Adjectives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Word = c.String(),
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
            
            DropTable("Adjectives");
            CreateIndex("Activations", "UserId");
            AddForeignKey("Activations", "UserId", "Users", "Id", cascadeDelete: true);
        }
    }
}
