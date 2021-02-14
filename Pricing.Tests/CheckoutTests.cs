using System;
using NUnit.Framework;

namespace Pricing.Tests
{
    public class CheckoutTests
    {
        private ICheckout _checkout;

        [SetUp]
        public void Setup()
        {
            _checkout = CheckoutFactory.Checkout();
        }

        /* 
            We should only be able to scan items we actually sell.
            Since the pricing engine cannot price these, reject here with an exception
        */

        [TestCase("A")]
        [TestCase("B")]
        [TestCase("C")]
        [TestCase("D")]

        [TestCase("a")]
        [TestCase("b")]
        [TestCase("c")]
        [TestCase("d")]
        public void ScanAcceptsKnownItems(string item)
        {
            Assert.DoesNotThrow(() => _checkout.Scan(item));
        }
        
        [TestCase("")]
        [TestCase(null)]
        [TestCase("E")]
        [TestCase("z")]
        [TestCase("H")]
        [TestCase("CHIPS")]
        [TestCase("marmelade")]
        public void ScanThrowsUnknownItems(string item)
        {
            Assert.Throws<Exception>(() => _checkout.Scan(item));
        }

        [TestCase("A", 50)]
        [TestCase("B", 30)]
        [TestCase("C", 20)]
        [TestCase("D", 15)]
        public void CalculateUnitPriceSingleItems(string item, int expectedPrice)
        {
            _checkout.Scan(item);
            Assert.That(_checkout.GetTotalPrice(), Is.EqualTo(expectedPrice));
        }

        [TestCase("C,B,A", 100)]
        [TestCase("A,B,C", 100)]
        [TestCase("A,B,C,C,D", 135)]
        [TestCase("D,C,C,B,A", 135)]
        [TestCase("D,C,C,D", 70)]
        [TestCase("D,A,B,A,D,C,C", 200)]
        public void CalculateUnitPriceCombinationsOfItems(string items, int expectedPrice)
        {
            foreach(var item in items.Split(","))
            {
                _checkout.Scan(item);
            }
            Assert.That(_checkout.GetTotalPrice(), Is.EqualTo(expectedPrice));
        }
    }
}