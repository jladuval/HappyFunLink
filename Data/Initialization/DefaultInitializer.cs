namespace Data.Initialization
{
    using System.Data.Entity;
    using EntityFramework;
    using Migration;

    public class DefaultInitializer : MigrateDatabaseToLatestVersion<DataContext, DefaultConfiguration>
    {
    }
}
