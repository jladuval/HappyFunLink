namespace Data.Migration
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTextToWord : DbMigration
    {
        public override void Up()
        {
            AddColumn("HappyLinks", "Word", c => c.String());
            DropColumn("HappyLinks", "Text");
        }
        
        public override void Down()
        {
            AddColumn("HappyLinks", "Text", c => c.String());
            DropColumn("HappyLinks", "Word");
        }
    }
}
