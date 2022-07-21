namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public decimal TotalPrice
        {
            get { return Items.Sum(c=>c.Quantity*c.Price); }
        }
    }
}
