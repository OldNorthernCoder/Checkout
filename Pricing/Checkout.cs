using System.Collections.Generic;
using System.Linq;
using Pricing.Models;

namespace Pricing
{
    internal class Checkout : ICheckout
    {
        private Prices _prices;

        private Dictionary<string, BasketItem> _basket = new Dictionary<string, BasketItem>();
        
        internal Checkout(Prices prices) 
        {
            _prices = prices;
        }
        
        public void Scan(string item) => AddOne(_prices.GetPrice(item.ThrowIfNull()));

        private void AddOne(Price price) 
        {
            if (!_basket.ContainsKey(price.Sku)) 
            {
                _basket.Add(price.Sku, new BasketItem(price));
            }
            
            _basket[price.Sku].IncrementQuantity();            
        }

        public int GetTotalPrice() => _basket.Values.Sum(b => b.TotalPrice());
    }
}
