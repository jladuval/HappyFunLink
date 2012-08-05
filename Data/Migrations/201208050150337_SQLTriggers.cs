namespace Data.Migration
{
    using System.Data.Entity.Migrations;
    
    public partial class SQLTriggers : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE TRIGGER HappyLinkGenAdj On Adjectives AFTER INSERT AS MERGE INTO HappyLinks As HL USING (SELECT Adjectives.Word + Copy.Word + Nouns.Word FROM Adjectives INNER JOIN Adjectives AS [Copy] ON Adjectives.Word != Copy.Word CROSS JOIN Nouns) AS Source(Word) ON (HL.Word) = (Source.Word) WHEN NOT MATCHED THEN INSERT (Word, LastAccessed) VALUES (Source.Word, NULL);");
            Sql("CREATE TRIGGER HappyLinkGenNoun On Nouns AFTER INSERT AS MERGE INTO HappyLinks As HL USING (SELECT Adjectives.Word + Copy.Word +  Nouns.Word FROM Adjectives INNER JOIN Adjectives AS [Copy] ON Adjectives.Word != Copy.Word CROSS JOIN Nouns) AS Source(Word) ON (HL.Word) = (Source.Word) WHEN NOT MATCHED THEN INSERT (Word, LastAccessed) VALUES (Source.Word, NULL);");
        }
        
        public override void Down()
        {
            Sql("Drop trigger HappyLinkGenAdj");
            Sql("Drop trigger HappyLinkGenNoun");
        }
    }
}
