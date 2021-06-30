#nullable disable
namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;
    using Boxed.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CartsService : ICartsService
    {
        private readonly FoodDBContext _db;
        private readonly IMapper<ShoppingCart, CartVM> cartMapper;
        private readonly IMapper<Product, FoodProductVM> productMapper;

        public CartsService(FoodDBContext dbcontext, IMapper<ShoppingCart, CartVM> mapper, IMapper<Product, FoodProductVM> mapper1)
        {
            this._db = dbcontext;
            this.cartMapper = mapper;
            this.productMapper = mapper1;
        }

        public ServiceResponse<List<CartVM>> CreateShoppingCart(CartSaves cart)
        {
            var newcart = new ShoppingCart()
            {
                CartLabel = cart.CartLabel,
                UserName = cart.User,
                IsDiscounted = cart.IsDiscounted
            };

            this._db.ShoppingCarts.Add(newcart);
            this._db.SaveChanges();
            // I cannot believe this works LOL, without using: db.Entry(MyNewObject).GetDatabaseValues();
            var newCartId = newcart.Id;
            //perhaps should use it in GetCart() api
            var response = new ServiceResponse<List<CartVM>>
            {
                Data = this._db.ShoppingCarts.Include(c => c.FoodProduct_Carts.Where(joint => joint.CartId == newcart.Id))
                                            .Select(cart => this.cartMapper.Map(cart)).ToList()
            };

            return response;
        }

        public async Task<ServiceResponse<List<CartVM>>> GetAllCarts()
        {
            var response = new ServiceResponse<List<CartVM>>();

            try
            {
                var allUserCarts = await this._db.ShoppingCarts.Include(c => c.FoodProduct_Carts)
                                                .ThenInclude(u => u.FoodProduct)
                                                .Select(cart => this.cartMapper.Map(cart))
                                                .ToListAsync().ConfigureAwait(true);
                //var allUserCarts = await this._db.ShoppingCarts.Include(c => c.FoodProduct_Carts).Select(cart => this.cartMapper.Map(cart)).ToListAsync().ConfigureAwait(true);

                response.Data = allUserCarts;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceResponse<ShoppingCart> GetCartById(int cartId)
        {
            var response = new ServiceResponse<ShoppingCart>();
            var cart = this._db.ShoppingCarts.Include(c => c.FoodProduct_Carts).FirstOrDefault(c => c.Id == cartId);
            if (cart != null)
            {
                response.Data = cart;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "Cannot find a Cart with this ID";
            }

            return response;
        }

        public ServiceResponse<CartVM> GetCartWithProducts(int cartId)
        {
            var response = new ServiceResponse<CartVM>();
            //querying ShoppingCart including related Products List
            var cart = this._db.ShoppingCarts.Include(c => c.FoodProduct_Carts)
                                                .ThenInclude(u => u.FoodProduct)
                                                .FirstOrDefault(c => c.Id == cartId);

            if (cart != null)
            {
                // mapping from ShoppingCart to CartVM
                var cartView = this.cartMapper.Map(cart);
                // mapping from list of FoodProduct_Carts into list of FoodProduct, overriding the default ProductsInCart
                cartView.ProductsInCart = cart.FoodProduct_Carts.Select(pc => new ProductsInCartsVM()
                {
                    ProductDescription = pc.FoodProduct.Name,
                    QuantityInCart = pc.QuantityInCart,
                    OriginalPrice = pc.FoodProduct.Price,
                    CartId = pc.CartId
                }).ToList();

                response.Data = cartView;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "Cannot find a Cart with this ID";
            }

            return response;
        }

    }
}
