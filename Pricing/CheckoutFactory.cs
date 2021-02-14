using Pricing.Data;
namespace Pricing
{
    public static class CheckoutFactory
    {
        public static ICheckout Checkout() => new Checkout(new Prices(new Repository())); // normally done with a DI container, here assembled by hand
    }
}