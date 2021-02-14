using Pricing.Models;
using Pricing.Data;

namespace Pricing
{
    internal class Prices
    {
        private IRepository _repository;

        internal Prices(IRepository repository)
        {
            _repository = repository;
        }

        internal Price GetPrice(string item) => _repository.GetPrice(item).ThrowIfNull($"We do not sell {item}");
    }
}
