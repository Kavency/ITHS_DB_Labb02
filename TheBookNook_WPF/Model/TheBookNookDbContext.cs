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

    public virtual DbSet<Format> Formats { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<StoreBook> StoreBook { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server SPN=localhost;Database=TheBookNookDB;Integrated Security=True;Trust Server Certificate=True").EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);

            entity.HasMany(d => d.BookIsbns).WithMany(p => p.Authors)
                .UsingEntity<AuthorBook>(
                    j => j.HasOne(pt => pt.Book).WithMany(t => t.AuthorBooks)
                        .HasForeignKey(pt => pt.BookIsbn)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_AuthorBook_Books"),
                    j => j.HasOne(pt => pt.Author).WithMany(p => p.AuthorBooks)
                        .HasForeignKey(pt => pt.AuthorId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_AuthorBook_Authors"),
                    j =>
                    {
                        j.HasKey(t => new { t.AuthorId, t.BookIsbn });
                        j.ToTable("AuthorBook");
                        j.Property(pt => pt.AuthorId).HasColumnName("AuthorID");
                        j.Property(pt => pt.BookIsbn).HasColumnName("BookISBN");
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
        });

        modelBuilder.Entity<AuthorBook>()
            .HasKey(ba => new { ba.AuthorId, ba.BookIsbn });
        
        modelBuilder.Entity<AuthorBook>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.AuthorBooks)
            .HasForeignKey(ba => ba.BookIsbn);
        
        modelBuilder.Entity<AuthorBook>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.AuthorBooks)
            .HasForeignKey(ba => ba.AuthorId);

        modelBuilder.Entity<StoreBook>()
            .HasKey(sb => new { sb.StoreId, sb.BookIsbn });

        modelBuilder.Entity<StoreBook>()
            .HasOne(s => s.Store)
            .WithMany(sb => sb.StoreBooks)
            .HasForeignKey(s => s.StoreId);

        modelBuilder.Entity<StoreBook>()
            .HasOne(b => b.Book)
            .WithMany(sb => sb.StoreBooks)
            .HasForeignKey(b => b.BookIsbn);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
