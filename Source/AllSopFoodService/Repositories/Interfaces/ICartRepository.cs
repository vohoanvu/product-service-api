namespace AllSopFoodService.Repositories.Interfaces
{
    using System.Linq;
    using Model;

    public interface ICartRepository : IGenericRepository<ShoppingCart>
    {
        //Check if there is any Shopping Cart associated with a specific User
        bool CheckUserCart(int userId);

        ShoppingCart GetShoppingCartByUserId(int userId);

        ShoppingCart GetCartWithProductDataByUserId(int userId);

        IQueryable<ShoppingCart> GetAllCartsWithProductData();

        ShoppingCart GetShoppingCartByCartId(int cartId);
    }
}
