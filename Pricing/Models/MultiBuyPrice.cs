namespace Pricing.Models
{
    internal class MultiBuyPrice 
    {
        internal MultiBuyPrice(int quantity, int price)
        {
            Quantity = quantity;
            Price = price;
        }

        public int Quantity { get; }

        public int Price { get; }
    }
}