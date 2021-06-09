using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AllSopFoodService.Model
{
    public partial class FoodDBContext : DbContext
    {
        public FoodDBContext()
        {
        }

        public FoodDBContext(DbContextOptions<FoodDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<FoodProduct> FoodProducts { get; set; }
        public virtual DbSet<CartItem> ShoppingCartItems { get; set; }
        public virtual DbSet<Promotion> CouponCodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#pragma warning disable CS1030 // #warning directive
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-BJU462V;Database=FoodDB;Trusted_Connection=True;");
#pragma warning restore CS1030 // #warning directive
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            this.OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<Promotion>().HasData(
            new Promotion
            {
                Id = new Random().Next(1, 50),
                CouponCode = "10OFFPROMODRI",
                IsClaimed = false
            },
            new Promotion
            {
                Id = new Random().Next(1, 50),
                CouponCode = "5OFFPROMOALL",
                IsClaimed = false
            },
            new Promotion
            {
                Id = new Random().Next(1, 50),
                CouponCode = "20OFFPROMOALL",
                IsClaimed = false
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
