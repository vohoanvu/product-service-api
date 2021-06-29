# AllSop Food Service

[![GitHub Actions Status](https://github.com/Username/Project/workflows/Build/badge.svg?branch=main)](https://github.com/Username/Project/actions)

[![GitHub Actions Build History](https://buildstats.info/github/chart/Username/Project?branch=main&includeBuildsFromPullRequest=false)](https://github.com/Username/Project/actions)

Project Description

I’ve been asked to develop a RESTful API for a B2B foodservice customer, to be consumed by both an Angular web app and a Xamarin mobile app.

The customer sells six different categories of items: Meat & Poultry, Fruit & Vegetables, Drinks, Confectionary & Desserts, Baking/Cooking Ingredients and Miscellaneous Items.

The API that you develop should provide sufficient endpoints so that the clients are able to easily display all of the available products, as well as provide shopping cart functionality to which the products can be added, removed or have quantities updated.


There are also promotions that are currently running. These are as follows:

• Get 10% off bulk drinks – any drinks are 10% off the listed price (including already reduced items) when buying 10 or more

• £5.00 off your order when spending £50.00 or more on Baking/Cooking Ingredients

• £20.00 off your total order value when spending £100.00 or more and using the code “20OFFPROMO”


The prioritised list of user stories is as follows:
1. As a User I can view the products and their category, price and availability information.
2. As a User I can add a product to my shopping cart.
3. As a User I can remove a product from my shopping cart.
4. As a User I can view the total price for the products in my shopping cart.
5. As a User I can apply a voucher to my shopping cart.
6. As a User I can view the total price for the products in my shopping cart with discounts applied.
7. As a User I am alerted when I apply an invalid voucher to my shopping cart.
8. As a User I am unable to Out of Stock products to the shopping cart.



I created this project template with .NET Core API Boxed template. These API are not yet published online since i'm still refactoring them.

List of possible code improvement:

- Repositories Layers
- Refactor according to .net core api Boxed template
- Use GetModelDto/AddModelDto for view models and possibly use AutoMapper() (not effective)
- User Authentication (User Model and its equivalent relationships)
- Eager Loading and Explicit Loading for optimized DB queries:
