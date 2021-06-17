#nullable disable

namespace AllSopFoodService.Model
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

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
