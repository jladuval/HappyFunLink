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
            
        }
        
        public override void Down()
        {
            DropIndex("UsersInRoles", new[] { "RoleId" });
            DropIndex("UsersInRoles", new[] { "UserId" });
            DropIndex("SiteRegistrations", new[] { "Id" });
            DropForeignKey("UsersInRoles", "RoleId", "Roles");
            DropForeignKey("UsersInRoles", "UserId", "Users");
            DropForeignKey("SiteRegistrations", "Id", "Users");
            DropTable("UsersInRoles");
            DropTable("SiteRegistrations");
            DropTable("Activations");
            DropTable("Roles");
            DropTable("Users");
        }
    }
}
