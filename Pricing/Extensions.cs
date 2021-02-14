using System;

namespace Pricing
{
    public static class Extensions
    {
        public static T ThrowIfNull<T>(this T input, string message = "Value cannot be null") 
            => (input == null) ? throw new Exception(message) : input;
    }
}