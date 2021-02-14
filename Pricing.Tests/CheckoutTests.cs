using System;
using Moq;
using NUnit.Framework;
using Pricing.Data;
using Pricing.Models;

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
       
        [TestCase("A", 0, 0)]
        [TestCase("A", 2, 100)]
        [TestCase("A", 3, 130)]
        [TestCase("A", 6, 260)]
        [TestCase("A", 7, 310)]
        [TestCase("A", 8, 360)]
        [TestCase("A", 9, 390)]
        [TestCase("B", 0, 0)]
        [TestCase("B", 1, 30)]
        [TestCase("B", 2, 45)]
        [TestCase("B", 4, 90)]
        [TestCase("B", 7, 165)]
        [TestCase("B", 8, 180)]
        [TestCase("C", 0, 0)]
        [TestCase("C", 2, 40)]
        [TestCase("C", 4, 80)]
        [TestCase("C", 7, 140)]
        [TestCase("D", 0, 0)]
        [TestCase("D", 2, 30)]
        [TestCase("D", 4, 60)]
        [TestCase("D", 9, 135)]
        public void CalculateActualPriceSingleItems(string item, int quantityPurchased, int expectedPrice)
        {
            ScanQuantityOfItem(item, quantityPurchased);
            Assert.That(_checkout.GetTotalPrice(), Is.EqualTo(expectedPrice));
        }

        [TestCase(6, 4, 1, 1, 385)]
        [TestCase(7, 4, 1, 1, 435)]
        [TestCase(7, 3, 1, 0, 405)]
        [TestCase(3, 2, 2, 4, 275)]
        public void CalculateActualCombinationOfItems(int quantityA, int quantityB, int quantityC, int quantityD, int expectedPrice)
        {
            ScanQuantityOfItem("A", quantityA);
            ScanQuantityOfItem("B", quantityB);
            ScanQuantityOfItem("C", quantityC);
            ScanQuantityOfItem("D", quantityD);
            Assert.That(_checkout.GetTotalPrice(), Is.EqualTo(expectedPrice));
        }

        [Test]
        public void VerifyRepositoryIsCalledOnlyWhenNewItemIsRequired()
        {
            var repository = new Mock<IRepository>();
            repository.Setup(r => r.GetPrice(It.IsAny<string>())).Returns(new Price("Z", 200));
            var checkout = new Checkout(new Prices(repository.Object));

            checkout.Scan("A");
            repository.Verify(r => r.GetPrice("A"), Times.Once());

            checkout.Scan("A");
            repository.Verify(r => r.GetPrice("A"), Times.Once());

            checkout.Scan("B");
            repository.Verify(r => r.GetPrice("B"), Times.Once());
            repository.Verify(r => r.GetPrice(It.IsAny<string>()), Times.Exactly(2));
        }

        private void ScanQuantityOfItem(string item, int quantityPurchased)
        {
            for (int i = 0; i < quantityPurchased; i++)
            {
                _checkout.Scan(item);
            }
        } 
    }
}