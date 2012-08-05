namespace Data.Migration
{
    using System.Data.Entity.Migrations;
    
    public partial class HappyLinkDateTimeIsNowNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("HappyLinks", "LastAccessed", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("HappyLinks", "LastAccessed", c => c.DateTime(nullable: false));
        }
    }
}
