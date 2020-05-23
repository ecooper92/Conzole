using System;
using System.Collections.Generic;

namespace Conzole
{
    /// <summary>
    /// The available options for the prompt function.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Gets the value at TKey from the dictionary or creates it if it doesn't exist then returns the new value.
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new() => GetOrAdd(dictionary, key, k => new TValue());

        /// <summary>
        /// Gets the value at TKey from the dictionary or creates it if it doesn't exist then returns the new value.
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> createValue)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = createValue(key);
                dictionary[key] = value;
            }

            return value;
        }
    }
}