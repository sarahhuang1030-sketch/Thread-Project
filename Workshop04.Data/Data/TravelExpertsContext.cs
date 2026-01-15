using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Workshop04.Data.Models;

namespace Workshop04.Data.Data;

public class TravelExpertsContext : IdentityDbContext<Customer>
{
    //public TravelExpertsContext()
    //{
    //}

    public TravelExpertsContext(DbContextOptions<TravelExpertsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agent> Agents { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingDetail> BookingDetails { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerPreferences> CustomerPreferences { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackagesProductsSupplier> PackagesProductsSuppliers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Agency> Agencies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Configuration is handled through dependency injection in Program.cs
            // This method intentionally left empty as a safeguard
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ✅ Make dbo the default schema (matches your pgloader-imported tables)
        modelBuilder.HasDefaultSchema("dbo");

        // ✅ REQUIRED for Identity
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<Agent>().ToTable("Agents", t => t.ExcludeFromMigrations());
        //modelBuilder.Entity<Booking>().ToTable("Bookings", t => t.ExcludeFromMigrations());
        //modelBuilder.Entity<BookingDetail>().ToTable("BookingDetails", t => t.ExcludeFromMigrations());
        //modelBuilder.Entity<Class>().ToTable("Classes", t => t.ExcludeFromMigrations());
        //modelBuilder.Entity<CustomerPreferences>().ToTable("CustomerPreferences", t => t.ExcludeFromMigrations());
        //modelBuilder.Entity<Package>().ToTable("Packages", t => t.ExcludeFromMigrations());
        //modelBuilder.Entity<PackagesProductsSupplier>().ToTable("Packages_Products_Suppliers", t => t.ExcludeFromMigrations());
        //modelBuilder.Entity<Product>().ToTable("Products", t => t.ExcludeFromMigrations());
        //modelBuilder.Entity<Supplier>().ToTable("Suppliers", t => t.ExcludeFromMigrations());
        //modelBuilder.Entity<Agency>().ToTable("Agencies", t => t.ExcludeFromMigrations());

        //to match psql
        modelBuilder.Entity<Agent>().ToTable("agents", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<Booking>().ToTable("bookings", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<BookingDetail>().ToTable("bookingdetails", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<Class>().ToTable("classes", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<CustomerPreferences>().ToTable("customerpreferences", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<Package>().ToTable("packages", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<PackagesProductsSupplier>().ToTable("packages_products_suppliers", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<Product>().ToTable("products", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<Supplier>().ToTable("suppliers", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<Agency>().ToTable("agencies", t => t.ExcludeFromMigrations());



        modelBuilder.Entity<Booking>()
      .HasOne(b => b.Customer)
      .WithMany(c => c.Bookings)
      .HasForeignKey(b => b.CustomerId)
      .HasPrincipalKey(c => c.Id);

        modelBuilder.Entity<BookingDetail>(entity =>
        {
            entity.HasKey(e => e.BookingDetailId)
                .HasName("aaaaaBookingDetails_PK")
                .IsClustered(false);

            entity.Property(e => e.BookingId)
                .HasDefaultValue(0)
                .HasAnnotation("Relational:DefaultConstraintName", "DF__BookingDe__Booki__7C8480AE");
            entity.Property(e => e.ProductSupplierId)
                .HasDefaultValue(0)
                .HasAnnotation("Relational:DefaultConstraintName", "DF__BookingDe__Produ__7D78A4E7");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingDetails).HasConstraintName("FK_BookingDetails_Bookings");

            entity.HasOne(d => d.Class).WithMany(p => p.BookingDetails).HasConstraintName("FK_BookingDetails_Classes");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId)
                .HasName("aaaaaClasses_PK")
                .IsClustered(false);
        });

        // ✅ Only relationships, NO KEYS
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Agent)
            .WithMany(a => a.Customers);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustEmail)
                  .IsRequired()
                  .HasMaxLength(50);
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.PackageId)
                .HasName("aaaaaPackages_PK")
                .IsClustered(false);

            entity.Property(e => e.PkgAgencyCommission)
                .HasDefaultValue(0m)
                .HasAnnotation("Relational:DefaultConstraintName", "DF__Packages__PkgAge__77BFCB91");
            entity.Property(e => e.PkgBasePrice).HasAnnotation("Relational:DefaultConstraintName", "DF__Packages__PkgBas__76CBA758");
        });

        modelBuilder.Entity<PackagesProductsSupplier>(entity =>
        {
            entity.HasKey(e => e.PackageProductSupplierId).HasName("PK__Packages__53E8ED99A6893451");

            entity.Property(e => e.PackageId).HasAnnotation("Relational:DefaultConstraintName", "DF__Packages___Packa__239E4DCF");
            entity.Property(e => e.ProductSupplierId).HasAnnotation("Relational:DefaultConstraintName", "DF__Packages___Produ__24927208");

            entity.HasOne(d => d.Package).WithMany(p => p.PackagesProductsSuppliers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Packages_Products_Supplie_FK00");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId)
                .HasName("aaaaaProducts_PK")
                .IsClustered(false);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId)
                .HasName("aaaaaSuppliers_PK")
                .IsClustered(false);
        });

     //   OnModelCreatingPartial(modelBuilder);
    }
}