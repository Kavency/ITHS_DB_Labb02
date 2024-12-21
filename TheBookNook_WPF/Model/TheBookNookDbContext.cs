using Microsoft.EntityFrameworkCore;

namespace TheBookNook_WPF.Model;

public partial class TheBookNookDbContext : DbContext
{
    public TheBookNookDbContext(){ }

    public TheBookNookDbContext(DbContextOptions<TheBookNookDbContext> options) : base(options){ }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorBook> AuthorBook { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Format> Formats { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<VwAuthorSummary> VwAuthorSummaries { get; set; }

    public virtual DbSet<VwStoreService> VwStoreServices { get; set; }

    public virtual DbSet<VwTopCustomer> VwTopCustomers { get; set; }

    public virtual DbSet<VwTotalInStock> VwTotalInStocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server SPN=localhost;Database=TheBookNookDB;Integrated Security=True;Trust Server Certificate=True").EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Genererad av EF Tools Scaffolding

        //modelBuilder.Entity<Author>(entity =>
        //{
        //    entity.Property(e => e.Id).HasColumnName("ID");
        //    entity.Property(e => e.FirstName).HasMaxLength(255);
        //    entity.Property(e => e.LastName).HasMaxLength(255);

        //    entity.HasMany(d => d.BookIsbns).WithMany(p => p.Authors)
        //        .UsingEntity<Dictionary<string, object>>(
        //            "AuthorBook",
        //            r => r.HasOne<Book>().WithMany()
        //                .HasForeignKey("BookIsbn")
        //                .OnDelete(DeleteBehavior.Cascade)
        //                .HasConstraintName("FK_AuthorBook_Books"),
        //            l => l.HasOne<Author>().WithMany()
        //                .HasForeignKey("AuthorId")
        //                .OnDelete(DeleteBehavior.Cascade)
        //                .HasConstraintName("FK_AuthorBook_Authors"),
        //            j =>
        //            {
        //                j.HasKey("AuthorId", "BookIsbn");
        //                j.ToTable("AuthorBook");
        //                j.IndexerProperty<int>("AuthorId").HasColumnName("AuthorID");
        //                j.IndexerProperty<long>("BookIsbn").HasColumnName("BookISBN");
        //            });
        //});


        // Egen
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);

            entity.HasMany(d => d.BookIsbns).WithMany(p => p.Authors)
                .UsingEntity<AuthorBook>(
                    j => j.HasOne(pt => pt.Book).WithMany(t => t.AuthorBooks)
                        .HasForeignKey(pt => pt.Isbn)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_AuthorBook_Books"),
                    j => j.HasOne(pt => pt.Author).WithMany(p => p.AuthorBooks)
                        .HasForeignKey(pt => pt.AuthorId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_AuthorBook_Authors"),
                    j =>
                    {
                        j.HasKey(t => new { t.AuthorId, t.Isbn });
                        j.ToTable("AuthorBook");
                        j.Property(pt => pt.AuthorId).HasColumnName("AuthorID");
                        j.Property(pt => pt.Isbn).HasColumnName("BookISBN");
                    });
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn);

            entity.Property(e => e.Isbn)
                .ValueGeneratedNever()
                .HasColumnName("ISBN");
            entity.Property(e => e.FormatId).HasColumnName("FormatID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.LanguageId).HasColumnName("LanguageID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Format).WithMany(p => p.Books)
                .HasForeignKey(d => d.FormatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Formats");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Genres");

            entity.HasOne(d => d.Language).WithMany(p => p.Books)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Languages");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasIndex(e => e.Country1, "UQ_Countries").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Country1)
                .HasMaxLength(50)
                .HasColumnName("Country");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.StreetName).HasMaxLength(255);

            entity.HasOne(d => d.Country).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Customers_Countries");

            entity.HasMany(d => d.Stores).WithMany(p => p.Customers)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerStore",
                    r => r.HasOne<Store>().WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CustomerStore_Stores"),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CustomerStore_Customers"),
                    j =>
                    {
                        j.HasKey("CustomerId", "StoreId");
                        j.ToTable("CustomerStore");
                        j.IndexerProperty<int>("CustomerId").HasColumnName("CustomerID");
                        j.IndexerProperty<int>("StoreId").HasColumnName("StoreID");
                    });
        });

        modelBuilder.Entity<Format>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Format1)
                .HasMaxLength(50)
                .HasColumnName("Format");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasIndex(e => e.Genre1, "UQ_Genres").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Genre1)
                .HasMaxLength(50)
                .HasColumnName("Genre");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Language__3214EC27B3763B34");

            entity.HasIndex(e => e.Language1, "UQ__Language__C3D5925006E8F51D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Language1)
                .HasMaxLength(255)
                .HasColumnName("Language");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFF098376C");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__Customer__3C34F16F");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Orders_Stores");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C58902331");

            entity.ToTable(tb => tb.HasTrigger("trg_UpdateTotalAmount"));

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__3F115E1A");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasIndex(e => e.Service1, "UQ_Services").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Service1)
                .HasMaxLength(50)
                .HasColumnName("Service");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.Isbn });

            entity.ToTable("Stock");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.Isbn).HasColumnName("ISBN");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Books");

            entity.HasOne(d => d.Store).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stock_Stores");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Stores)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stores_Countries");

            entity.HasMany(d => d.Services).WithMany(p => p.Stores)
                .UsingEntity<Dictionary<string, object>>(
                    "StoreService",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_StoreService_Services"),
                    l => l.HasOne<Store>().WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_StoreService_Stores"),
                    j =>
                    {
                        j.HasKey("StoreId", "ServiceId");
                        j.ToTable("StoreService");
                        j.IndexerProperty<int>("StoreId").HasColumnName("StoreID");
                        j.IndexerProperty<int>("ServiceId").HasColumnName("ServiceID");
                    });
        });

        modelBuilder.Entity<VwAuthorSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_AuthorSummary");

            entity.Property(e => e.AuthorName)
                .HasMaxLength(511)
                .HasColumnName("Author name");
            entity.Property(e => e.OfBookTitles).HasColumnName("# of book titles");
            entity.Property(e => e.ValueInStockPerAuthor).HasColumnName("Value in stock per author");
        });

        modelBuilder.Entity<VwStoreService>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_StoreServices");

            entity.Property(e => e.Service).HasMaxLength(50);
            entity.Property(e => e.StoreName)
                .HasMaxLength(50)
                .HasColumnName("Store name");
        });

        modelBuilder.Entity<VwTopCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TopCustomers");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(511)
                .HasColumnName("Customer name");
            entity.Property(e => e.Discount).HasColumnName("% Discount");
            entity.Property(e => e.TotalSpent).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<VwTotalInStock>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TotalInStock");

            entity.Property(e => e.Format).HasMaxLength(50);
            entity.Property(e => e.Genre).HasMaxLength(50);
            entity.Property(e => e.InStock).HasColumnName("# in stock");
            entity.Property(e => e.Isbn).HasColumnName("ISBN");
            entity.Property(e => e.Language).HasMaxLength(255);
            entity.Property(e => e.NeedsRestock)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("Needs restock");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<AuthorBook>()
            .HasKey(ba => new { ba.AuthorId, ba.Isbn });
        
        modelBuilder.Entity<AuthorBook>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.AuthorBooks)
            .HasForeignKey(ba => ba.Isbn);
        
        modelBuilder.Entity<AuthorBook>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.AuthorBooks)
            .HasForeignKey(ba => ba.AuthorId);



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
