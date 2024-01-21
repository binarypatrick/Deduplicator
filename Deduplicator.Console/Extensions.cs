using System;
using System.Collections.Generic;

namespace Cli.Deduplicator
{
    internal static class Extensions
    {
        public static void AddOrCreate<T>(this Dictionary<string, T> dictionary, string key, Func<T, T> addFunction) where T : new()
        {
            if (dictionary.ContainsKey(key))
            {
                T? value = dictionary[key];
                value = addFunction(value);
                dictionary[key] = value;
            }
            {
                T value = new();
                addFunction(value);
                dictionary.Add(key, value);
            }
        }
    }
}
