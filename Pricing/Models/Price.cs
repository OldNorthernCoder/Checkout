using System;

namespace Pricing.Models
{
    internal class Price
    {
        private int _unitPrice;
        
        private MultiBuyPrice _multiBuyPrice;

        internal Price(string sku, int unitPrice)
        {
            Sku = sku;
            _unitPrice = unitPrice;
        }

        internal Price(string sku, int unitPrice, MultiBuyPrice multiBuyPrice) : this(sku, unitPrice)
        {
            _multiBuyPrice = multiBuyPrice;
        }

        public string Sku { get; }
        
        internal int TotalPrice(int quantity) => (_multiBuyPrice == null) ? quantity * _unitPrice : CalculateMultiBuyPrice(quantity);

        private int CalculateMultiBuyPrice(int quantity)
        {
            var multibuys = Math.DivRem(quantity, _multiBuyPrice.Quantity, out int remainder);
            return (multibuys * _multiBuyPrice.Price) + (remainder * _unitPrice);
        }  
    }
}