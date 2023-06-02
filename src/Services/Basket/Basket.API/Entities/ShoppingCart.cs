using System.Diagnostics;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string Username { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        private decimal totalprice;

        public ShoppingCart()
        {
            
        }

        public ShoppingCart(string userName)
        {
            Username = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                if(totalprice == 0)
                {
                    foreach (var item in Items)
                    {
                        totalprice += item.Price;
                    }
                }
                return totalprice;
            }
            set
            {
                totalprice = value;
            }
        }
    }
}
