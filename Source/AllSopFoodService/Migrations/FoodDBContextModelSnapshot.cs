﻿// <auto-generated />
using System;
using AllSopFoodService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AllSopFoodService.Migrations
{
    [DbContext(typeof(FoodDbContext))]
    partial class FoodDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AllSopFoodService.Model.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsAvailable = true,
                            Label = "Meat & Poultry"
                        },
                        new
                        {
                            Id = 2,
                            IsAvailable = true,
                            Label = "Fruit & Vegetables"
                        },
                        new
                        {
                            Id = 3,
                            IsAvailable = true,
                            Label = "Drinks"
                        },
                        new
                        {
                            Id = 4,
                            IsAvailable = true,
                            Label = "Confectionary & Desserts"
                        },
                        new
                        {
                            Id = 5,
                            IsAvailable = true,
                            Label = "Baking/Cooking Ingredients"
                        },
                        new
                        {
                            Id = 6,
                            IsAvailable = true,
                            Label = "Miscellaneous Items"
                        });
                });

            modelBuilder.Entity("AllSopFoodService.Model.FoodProductInShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("QuantityInCart")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("FoodProductsInCarts");
                });

            modelBuilder.Entity("AllSopFoodService.Model.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Exception")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogEvent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageTemplate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Properties")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("AllSopFoodService.Model.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("AllSopFoodService.Model.Promotion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("VoucherCode");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsClaimed")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("AllSopFoodService.Model.ShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CartLabel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDiscounted")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("VoucherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("VoucherId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("AllSopFoodService.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AllSopFoodService.Model.FoodProductInShoppingCart", b =>
                {
                    b.HasOne("AllSopFoodService.Model.ShoppingCart", "ShoppingCart")
                        .WithMany("FoodProductCarts")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AllSopFoodService.Model.Product", "FoodProduct")
                        .WithMany("FoodProductInCarts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoodProduct");

                    b.Navigation("ShoppingCart");
                });

            modelBuilder.Entity("AllSopFoodService.Model.Product", b =>
                {
                    b.HasOne("AllSopFoodService.Model.Category", "Category")
                        .WithMany("FoodProducts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("AllSopFoodService.Model.ShoppingCart", b =>
                {
                    b.HasOne("AllSopFoodService.Model.User", "CartUser")
                        .WithOne("Cart")
                        .HasForeignKey("AllSopFoodService.Model.ShoppingCart", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AllSopFoodService.Model.Promotion", "VoucherCode")
                        .WithMany("DiscountedCarts")
                        .HasForeignKey("VoucherId");

                    b.Navigation("CartUser");

                    b.Navigation("VoucherCode");
                });

            modelBuilder.Entity("AllSopFoodService.Model.Category", b =>
                {
                    b.Navigation("FoodProducts");
                });

            modelBuilder.Entity("AllSopFoodService.Model.Product", b =>
                {
                    b.Navigation("FoodProductInCarts");
                });

            modelBuilder.Entity("AllSopFoodService.Model.Promotion", b =>
                {
                    b.Navigation("DiscountedCarts");
                });

            modelBuilder.Entity("AllSopFoodService.Model.ShoppingCart", b =>
                {
                    b.Navigation("FoodProductCarts");
                });

            modelBuilder.Entity("AllSopFoodService.Model.User", b =>
                {
                    b.Navigation("Cart");
                });
#pragma warning restore 612, 618
        }
    }
}
