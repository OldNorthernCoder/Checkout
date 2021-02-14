using System.Collections.Generic;
using System.Linq;
using Pricing.Models;

namespace Pricing.Data
{
    internal class Repository : IRepository
    {
        private IEnumerable<Price> _prices
        => new List<Price>
            {
                new Price("A", 50, new MultiBuyPrice(3, 130)),
                new Price("B", 30, new MultiBuyPrice(2, 45)),
                new Price("C", 20),
                new Price("D", 15),
            };

        // in practice this would be a call to our pricing data source
        // for now just return the hardcoded data in the problem
        // Normally, a supermarket checkout would support a large number of different products
        // so better to get single item prices as required rather than load all
        public Price GetPrice(string sku) 
            => _prices
                .Where(p => p.Sku == sku?.ToUpper()) //suppport case-insensitivity
                .SingleOrDefault();
    }
}