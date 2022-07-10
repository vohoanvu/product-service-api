namespace AllSopFoodService.Repositories
{
    using System;
    using System.Linq;
    using Model;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<FoodDbContext>();

                //seeding Products data
                if (context != null && !context.Products.Any())
                {
                    context.Products.AddRange(new Product()
                    {
                        Name = "Chicken Fillets, 6 x 100g",
                        Price = Convert.ToDecimal(4.50),
                        Quantity = 12,
                        CategoryId = 1
                    },
                    new Product()
                    {
                        Name = "Sirloin Steaks, 4 x 6-8oz",
                        Price = Convert.ToDecimal(45.70),
                        Quantity = 6,
                        CategoryId = 1
                    },
                    new Product()
                    {
                        Name = "Whole Free-Range Turkey, 1 x 16-18lbs",
                        Price = Convert.ToDecimal(43.20),
                        Quantity = 8,
                        CategoryId = 1
                    },
                    new Product()
                    {
                        Name = "Granny Smith Apples, 4 x 16 each",
                        Price = Convert.ToDecimal(3.75),
                        Quantity = 0,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "Loose Carrots, 4 x 20 each",
                        Price = Convert.ToDecimal(2.67),
                        Quantity = 2,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "Mandarin Oranges, 6 x 10 x 12 ",
                        Price = Convert.ToDecimal(12.23),
                        Quantity = 8,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "Cauliflower Florets, 10 x 500g",
                        Price = Convert.ToDecimal(5.00),
                        Quantity = 5,
                        CategoryId = 2
                    },
                    new Product()
                    {
                        Name = "Coca-Cola, 6 x 2L",
                        Price = Convert.ToDecimal(8.30),
                        Quantity = 6,
                        CategoryId = 3
                    },
                    new Product()
                    {
                        Name = "Still Mineral Water, 6 x 24 x 500ml",
                        Price = Convert.ToDecimal(21.75),
                        Quantity = 9,
                        CategoryId = 3
                    },
                    new Product()
                    {
                        Name = "Sparkling Mineral Water, 6 x 24 x 500ml",
                        Price = Convert.ToDecimal(25.83),
                        Quantity = 16,
                        CategoryId = 3
                    },
                    new Product()
                    {
                        Name = "Mars Bar, 6 x 24 x 50g",
                        Price = Convert.ToDecimal(42.82),
                        Quantity = 4,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Peppermint Chewing Gum, 6 x 50 x 30g ",
                        Price = Convert.ToDecimal(35.70),
                        Quantity = 6,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Strawberry Cheesecake, 4 x 12 portions",
                        Price = Convert.ToDecimal(8.52),
                        Quantity = 0,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Vanilla Ice Cream, 6 x 4L ",
                        Price = Convert.ToDecimal(3.80),
                        Quantity = 2,
                        CategoryId = 4
                    },
                    new Product()
                    {
                        Name = "Plain Flour, 10 x 1kg",
                        Price = Convert.ToDecimal(6.21),
                        Quantity = 4,
                        CategoryId = 5
                    },
                    new Product()
                    {
                        Name = "Icing Sugar, 12 x 500g",
                        Price = Convert.ToDecimal(9.38),
                        Quantity = 6,
                        CategoryId = 5
                    },
                    new Product()
                    {
                        Name = "Free-Range Eggs, 10 x 12 each",
                        Price = Convert.ToDecimal(9.52),
                        Quantity = 9,
                        CategoryId = 5
                    },
                    new Product()
                    {
                        Name = "Caster Sugar, 16 x 750g",
                        Price = Convert.ToDecimal(12.76),
                        Quantity = 13,
                        CategoryId = 5
                    },
                    new Product()
                    {
                        Name = "Kitchen Roll, 100 x 300 sheets",
                        Price = Convert.ToDecimal(43.92),
                        Quantity = 19,
                        CategoryId = 6
                    },
                    new Product()
                    {
                        Name = "Paper Plates, 10 x 200 each",
                        Price = Convert.ToDecimal(16.19),
                        Quantity = 7,
                        CategoryId = 6
                    }
                    );

                    context.SaveChanges();
                }


                // seeding Promotion Data
                if (context != null && !context.CouponCodes.Any())
                {
                    context.CouponCodes.AddRange(
                        new Promotion
                        {
                            CouponCode = "10OFFPROMODRI",
                            Description = "Get 10% off bulk drinks – any drinks are 10% off the listed price (including already reduced items) when buying 10 or more",
                            IsClaimed = false
                        },
                        new Promotion
                        {
                            CouponCode = "5OFFPROMOALL",
                            Description = "$5.00 off your order when spending $50.00 or more on Baking/Cooking Ingredients",
                            IsClaimed = false
                        },
                        new Promotion
                        {
                            CouponCode = "20OFFPROMOALL",
                            Description = "$20.00 off your total order value when spending $100.00 or more and using the code 20OFFPROMOALL",
                            IsClaimed = false
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
