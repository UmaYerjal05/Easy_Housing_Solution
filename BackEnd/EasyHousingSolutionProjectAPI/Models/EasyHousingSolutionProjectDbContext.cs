using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EasyHousingSolutionProjectAPI.Models;

public partial class EasyHousingSolutionProjectDbContext : DbContext
{
    public EasyHousingSolutionProjectDbContext()
    {
    }

    public EasyHousingSolutionProjectDbContext(DbContextOptions<EasyHousingSolutionProjectDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Buyer> Buyers { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WishList> WishLists { get; set; }

    public virtual DbSet<PaymentRequest> PaymentRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=.\\sqlexpress;Database=EasyHousingSolutionProjectDB;Integrated security=true;Trust server certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Buyer>(entity =>
        {
            entity.HasKey(e => e.BuyerId).HasName("PK__Buyer__4B81C62AD1D2ECA1");

            // Added the userId as foreign key
            entity.HasOne(e=>e.User).WithOne().HasForeignKey<Buyer>(e=>e.UserId).OnDelete(DeleteBehavior.Cascade);

            entity.ToTable("Buyer");

            entity.Property(e => e.EmailId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(10)
                .IsUnicode(false);
        });



        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__F2D21B76456059A8");

            entity.ToTable("City");

            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false);

            //entity.HasOne(d => d.State).WithMany(p => p.Cities)
            //    .HasForeignKey(d => d.StateId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__City__StateId__3A81B327");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Images__7516F70CB9E80CE9");

            entity.Property(e => e.ImageName)
                .HasMaxLength(100000)
                .IsUnicode(false)
                .HasColumnName("Image");

            //entity.HasOne(d => d.Property).WithMany(p => p.Images)
            //    .HasForeignKey(d => d.PropertyId)
            //    .HasConstraintName("FK__Images__Property__46E78A0C");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.PropertyId).HasName("PK__Property__70C9A735C04F2E5E");

            entity.ToTable("Property");

            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.InitialDeposit).HasColumnType("money");
            entity.Property(e => e.Landmark)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PriceRange).HasColumnType("money");
            entity.Property(e => e.PropertyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PropertyOption)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PropertyType)
                .HasMaxLength(15)
                .IsUnicode(false);

            //entity.HasOne(d => d.Seller).WithMany(p => p.Properties)
            //    .HasForeignKey(d => d.SellerId)
            //    .HasConstraintName("FK__Property__Seller__440B1D61");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.SellerId).HasName("PK__Seller__7FE3DB81DA7FEE02");


            entity.HasOne(s=>s.User).WithOne().HasForeignKey<Seller>(s=>s.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.ToTable("Seller");

            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EmailId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(25)
                .IsUnicode(false);

            //entity.HasOne(d => d.City).WithMany(p => p.Sellers)
            //    .HasForeignKey(d => d.CityId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Seller__CityId__3F466844");

            //entity.HasOne(d => d.State).WithMany(p => p.Sellers)
            //    .HasForeignKey(d => d.StateId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Seller__StateId__3E52440B");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__State__C3BA3B3A69CB6464");

            entity.ToTable("State");

            entity.Property(e => e.StateId).ValueGeneratedNever();
            entity.Property(e => e.StateName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Users");

            entity.Property(e => e.UserName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.WishListId).HasName("PK__WishList__E41F8787E0E57C5E");

            entity.ToTable("WishList");

            //entity.HasOne(d => d.Buyer).WithMany(p => p.WishLists)
            //    .HasForeignKey(d => d.BuyerId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__WishList__BuyerI__4D94879B");

            //entity.HasOne(d => d.Property).WithMany(p => p.WishLists)
            //    .HasForeignKey(d => d.PropertyId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__WishList__Proper__4E88ABD4");
        });
        modelBuilder.Entity<PaymentRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentRequests__3214EC07");

            entity.Property(e => e.CardNumber)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDate)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CVV)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
