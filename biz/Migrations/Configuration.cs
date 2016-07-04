namespace male.services.biz.Migrations
{
  using System.Data.Entity.Migrations;

  internal sealed class Configuration : DbMigrationsConfiguration<male.services.biz.Model1>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = true;
      AutomaticMigrationDataLossAllowed = true;
      ContextKey = "male.services.biz.Model1";
    }

    protected override void Seed(male.services.biz.Model1 context)
    {
      //  This method will be called after migrating to the latest version.

      //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
      //  to avoid creating duplicate seed data. E.g.
      //
      //    context.People.AddOrUpdate(
      //      p => p.FullName,
      //      new Person { FullName = "Andrew Peters" },
      //      new Person { FullName = "Brice Lambson" },
      //      new Person { FullName = "Rowan Miller" }
      //    );
      //
    }
  }
}
