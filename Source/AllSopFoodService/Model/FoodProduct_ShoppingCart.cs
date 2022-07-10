namespace AllSopFoodService.Model
{
    // This Model Entity is used to demonstrate the many-many relationships between ShoppingCart and FoodProduct
    public class FoodProductShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product FoodProduct { get; set; } = default!;
        public int QuantityInCart { get; set; }
        public int CartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; } = default!;
    }
}
