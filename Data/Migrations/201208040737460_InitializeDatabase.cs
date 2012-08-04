namespace Data.Migration
{
    using System.Data.Entity.Migrations;
    
    public partial class InitializeDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "SiteRegistrations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Password = c.String(maxLength: 128),
                        LastPasswordChangedDate = c.DateTime(nullable: false, storeType: "datetime2"),
                        IsLockedOut = c.Boolean(nullable: false),
                        LastLockoutDate = c.DateTime(storeType: "datetime2"),
                        FailedPasswordAttemptCount = c.Int(nullable: false),
                        LastFailedPasswordAttemptDate = c.DateTime(storeType: "datetime2"),
                        FailedPasswordAttemptWindowStart = c.DateTime(storeType: "datetime2"),
                        PasswordResetToken = c.String(maxLength: 128),
                        PasswordResetTokenExpiredDate = c.DateTime(storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Users", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
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
            
            CreateTable(
                "UsersInRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            Sql("INSERT INTO Roles VALUES ('Users', 'Users registered in the system')");

            // Predefined admin user
            Sql("INSERT INTO Users VALUES ('', '','admin@happyfunlink.com', GETDATE())");
            Sql("INSERT INTO SiteRegistrations VALUES (1, 'AKS5gxhDzIoiRkb3k1zrnZL7xPPFW9klV8q8aYDsLIqFFVvWrH7WKRhyvVMlUoVk7g==', GETDATE(), 0, NULL, 0, NULL, NULL, NULL, NULL)");
            Sql("INSERT INTO UsersInRoles VALUES (1, 1)");
        }
        
        public override void Down()
        {
            DropIndex("UsersInRoles", new[] { "RoleId" });
            DropIndex("UsersInRoles", new[] { "UserId" });
            DropIndex("Links", new[] { "HappyLink_Id" });
            DropIndex("SiteRegistrations", new[] { "Id" });
            DropForeignKey("UsersInRoles", "RoleId", "Roles");
            DropForeignKey("UsersInRoles", "UserId", "Users");
            DropForeignKey("Links", "HappyLink_Id", "HappyLinks");
            DropForeignKey("SiteRegistrations", "Id", "Users");
            DropTable("UsersInRoles");
            DropTable("HappyLinks");
            DropTable("Links");
            DropTable("Adjectives");
            DropTable("Nouns");
            DropTable("SiteRegistrations");
            DropTable("Roles");
            DropTable("Users");
        }
    }
}
