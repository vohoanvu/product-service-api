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
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Promotion> CouponCodes { get; set; }
        // Many-to-many relationship version of ShoppingCartItems
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<FoodProduct_ShoppingCart> FoodProducts_Carts { get; set; }

        public virtual DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configuring DB relations through Fluent API
            modelBuilder.Entity<Product>()
                .HasOne(fp => fp.Category)
                .WithMany(c => c.FoodProducts)
                .HasForeignKey(fp => fp.CategoryId);

            modelBuilder.Entity<FoodProduct_ShoppingCart>()
                .HasOne(foodCart => foodCart.FoodProduct)
                .WithMany(foodItem => foodItem.FoodProduct_Carts)
                .HasForeignKey(foodCartId => foodCartId.ProductId);

            modelBuilder.Entity<FoodProduct_ShoppingCart>()
                .HasOne(foodCart => foodCart.ShoppingCart)
                .WithMany(cart => cart.FoodProduct_Carts)
                .HasForeignKey(foodCartId => foodCartId.CartId);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(p => p.VoucherCode)
                .WithMany(c => c.DiscountedCarts)
                .HasForeignKey(p => p.VoucherId);
            //Possible to configure the Composite Key for Products_Carts table, using fluent api, with a combination of ProductId + CartId

            //Seeding Category Data, need a solution to check if Categories table exist?
            modelBuilder.Entity<Category>().HasData(
                    new Category()
                    {
                        Id = 1,
                        Label = "Meat & Poultry",
                        IsAvailable = true
                    },
                    new Category()
                    {
                        Id = 2,
                        Label = "Fruit & Vegetables",
                        IsAvailable = true
                    },
                    new Category()
                    {
                        Id = 3,
                        Label = "Drinks",
                        IsAvailable = true
                    }, new Category()
                    {
                        Id = 4,
                        Label = "Confectionary & Desserts",
                        IsAvailable = true
                    },
                    new Category()
                    {
                        Id = 5,
                        Label = "Baking/Cooking Ingredients",
                        IsAvailable = true
                    },
                    new Category()
                    {
                        Id = 6,
                        Label = "Miscellaneous Items",
                        IsAvailable = true
                    }
                );
        }

    }
}
