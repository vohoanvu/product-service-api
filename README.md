# Vu Product Shopping Cart Service

[![GitHub Actions Status](https://github.com/Username/Project/workflows/Build/badge.svg?branch=main)](https://github.com/Username/Project/actions)

[![GitHub Actions Build History](https://buildstats.info/github/chart/Username/Project?branch=main&includeBuildsFromPullRequest=false)](https://github.com/Username/Project/actions)

Project Description

This personal project is RESTful API service for any eCommerce store application, it also serves as my learning playground to explore .NET Core API development. 
This project was developed using .NET 5 technology/Boxed API template, and deployed to Azure App Service.
The sample database design was meant for a food service store database, but it can be customized and used with any type of commercial products by modifying Products and Categories entities.


There are six pre-defined different categories of items: 
- Meat & Poultry, Fruit & Vegetables, Drinks, Confectionary & Desserts, Baking/Cooking Ingredients, and Miscellaneous Items.





You can also perform CRUD operations on the Categories entity to create new category as needed.

My set of API should provide sufficient endpoints so that the clients are able to easily display all of the available products, as well as provide shopping cart functionality to which the products can be added, removed or have quantities updated.

This project also implemented User Authentication mechanism that allows User to register and login.

Upon successful login, authenticated user can obtain an JWT strings, which can be used to authorize users to perform Shopping Cart related feature, such as: adding/removing a product to your cart, applying a discount voucher to your cart.


Additionally, There are also promotions that are currently running. These are as follows:

• Get 10% off bulk drinks – any drinks are 10% off the listed price (including already reduced items) when buying 10 or more using the code "10OFFPROMODRI"

• £5.00 off your order when spending £50.00 or more on Baking/Cooking Ingredients using the code "5OFFPROMOALL"

• £20.00 off your total order value when spending £100.00 or more and using the code “20OFFPROMOAA”


The prioritised list of user stories is as follows:
1. As a User I can view the products and their category, price and availability information.
2. As a User I can add a product to my shopping cart.
3. As a User I can remove a product from my shopping cart.
4. As a User I can view the total price for the products in my shopping cart.
5. As a User I can apply a voucher to my shopping cart.
6. As a User I can view the total price for the products in my shopping cart with discounts applied.
7. As a User I am alerted when I apply an invalid voucher to my shopping cart.
8. As a User I am unable to Out of Stock products to the shopping cart.
9. As a User I can perform CRUD operations on new/existing Categories.
10. As a User I can perform CRUD operations on new/existing Products database.
11. As a User I can perform CRUD operations on new/existing Promotions/Voucher deals.
12. As A User I can register a new User account credentials.
13. As A User I can login with my User account credentials.
14. As An authorized User I can delete an existing User Account




List of possible code improvement:

- Repositories Layers
- Refactor according to .net core api Boxed template
