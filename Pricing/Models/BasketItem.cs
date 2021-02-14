namespace Pricing.Models
{
    internal class BasketItem
    {
        private Price _price;

        private int _quantity;

        internal BasketItem(Price price) 
        {
            _price = price;
            _quantity = 0;
        }

        internal void IncrementQuantity() => _quantity++;
        
        internal int TotalPrice() => _price.TotalPrice(_quantity);
    }
}
