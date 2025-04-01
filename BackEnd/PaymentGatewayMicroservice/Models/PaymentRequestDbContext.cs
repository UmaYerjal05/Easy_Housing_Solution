using EasyHousingSolutionProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PaymentGatewayMicroservice.Models
{
    public partial class PaymentRequestDbContext : DbContext
    {
        public PaymentRequestDbContext()
        {

        }
        public PaymentRequestDbContext(DbContextOptions<PaymentRequestDbContext> options) : base(options)
        {

        }
        public virtual DbSet<PaymentRequest> PaymentRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=.\\sqlexpress;Database=EasyHousingSolutionProjectDB;Integrated security=true;Trust server certificate=true");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
}
