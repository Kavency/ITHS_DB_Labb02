using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TheBookNook_WPF.DBModel;

public partial class TheBookNookDbContext : DbContext
{
    public TheBookNookDbContext()
    {
    }

    public TheBookNookDbContext(DbContextOptions<TheBookNookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Format> Formats { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server SPN=KavencyDevTop;Database=TheBookNookDB;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);

            entity.HasMany(d => d.BookIsbns).WithMany(p => p.Authors)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("BookIsbn")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AuthorBook_Books"),
                    l => l.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AuthorBook_Authors"),
                    j =>
                    {
                        j.HasKey("AuthorId", "BookIsbn");
                        j.ToTable("AuthorBook");
                        j.IndexerProperty<int>("AuthorId").HasColumnName("AuthorID");
                        j.IndexerProperty<string>("BookIsbn")
                            .HasMaxLength(25)
                            .HasColumnName("BookISBN");
                    });
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn);

            entity.Property(e => e.Isbn)
                .HasMaxLength(25)
                .HasColumnName("ISBN");
            entity.Property(e => e.FormatId).HasColumnName("FormatID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

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
                .HasConstraintName("FK_Customers_Stores");

            entity.HasMany(d => d.Stores).WithMany(p => p.CustomersNavigation)
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
            entity.Property(e => e.Isbn)
                .HasMaxLength(25)
                .HasColumnName("ISBN");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
