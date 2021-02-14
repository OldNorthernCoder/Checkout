namespace Pricing
{
    public static class CheckoutFactory
    {
        public static ICheckout Checkout() => new Checkout();
    }
}