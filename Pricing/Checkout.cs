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
        
        public void Scan(string item) => GetOrCreateBasketItem(item.ThrowIfNull()).IncrementQuantity();

        public int GetTotalPrice() => _basket.Values.Sum(b => b.TotalPrice());

        private BasketItem GetOrCreateBasketItem(string item)
        {
            if (!_basket.ContainsKey(item)) 
            {
                _basket.Add(item, new BasketItem(_prices.GetPrice(item)));
            }
            
            return _basket[item];            
        }
    }
}
