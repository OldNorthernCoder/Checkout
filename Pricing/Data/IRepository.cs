using Pricing.Models;

namespace Pricing.Data
{
    internal interface IRepository
    {
        Price GetPrice(string sku);
    }
}