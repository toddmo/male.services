namespace male.services.biz
{
  using System.Data.Entity;

  public class Model1 : DbContext
  {
    // Your context has been configured to use a 'Model1' connection string from your application's 
    // configuration file (App.config or Web.config). By default, this connection string targets the 
    // 'male.services.biz.Model1' database on your LocalDb instance. 
    // 
    // If you wish to target a different database and/or database provider, modify the 'Model1' 
    // connection string in the application configuration file.
    public Model1()
        : base("name=Model1")
    {
      //Database.SetInitializer<Model1>(new DropCreateDatabaseIfModelChanges<Model1>());
      this.Configuration.LazyLoadingEnabled = true;      
    }

    // Add a DbSet for each entity type that you want to include in your model. For more information 
    // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.


    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Member> Members { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<Promise> Promises { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Fulfillment> Fulfillments { get; set; }

    public virtual DbSet<CustomMessage> CustomMessages { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Member>()
          .HasMany(e => e.Packages)
          .WithRequired(e => e.Member)
          .HasForeignKey(e => e.Member_Id);

      modelBuilder.Entity<Member>()
          .HasMany(e => e.Products)
          .WithRequired(e => e.Member)
          .HasForeignKey(e => e.Member_Id);

      modelBuilder.Entity<Member>()
          .HasMany(e => e.Services)
          .WithRequired(e => e.Member)
          .HasForeignKey(e => e.Member_Id);

      modelBuilder.Entity<Package>()
          .HasMany(e => e.Promises)
          .WithRequired(e => e.Package)
          .HasForeignKey(e => new { e.Member_Id, e.Package_Id })
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Service>()
          .HasMany(e => e.Promises)
          .WithRequired(e => e.Service)
          .HasForeignKey(e => new { e.Member_Id, e.Service_Id })
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Product>()
          .HasMany(e => e.Promises)
          .WithRequired(e => e.Product)
          .HasForeignKey(e => new { e.Member_Id, e.Product_Id })
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Package>()
          .HasMany(e => e.Purchases)
          .WithRequired(e => e.Package)
          .HasForeignKey(e => new { e.Member_Id, e.Package_Id })
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Client>()
          .HasMany(e => e.Purchases)
          .WithRequired(e => e.Client)
          .HasForeignKey(e => e.Client_Id);

      modelBuilder.Entity<Promise>()
          .HasMany(e => e.Fulfillments)
          .WithRequired(e => e.Promise)
          .HasForeignKey(e => new { e.Member_Id, e.Package_Id, e.Promise_Id })
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Purchase>()
          .HasMany(e => e.Fulfillments)
          .WithRequired(e => e.Purchase)
          .HasForeignKey(e => new { e.Member_Id, e.Package_Id, e.Purchase_Id })
          .WillCascadeOnDelete(false);

    }

  }


}