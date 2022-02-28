using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Scripts.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T, U)> ZipIntoTuple<T, U>(this (IEnumerable<T>, IEnumerable<U>) _instance)
        {
            return _instance.Item1.Zip(_instance.Item2, (t, u) => (t, u));
        }
        
        public static IEnumerable<(T, U, V)> ZipIntoTuple<T, U, V>(this (IEnumerable<T>, IEnumerable<U>, V) _instance)
        {
            return _instance.Item1.Zip(_instance.Item2, (t, u) => (t, u, _instance.Item3));
        }

        public static int FindIndex<T>(this IEnumerable<T> _instance, T _value)
        {
            int i = 0;
            foreach (var element in _instance)
            {
                if (Equals(element, _value))
                {
                    return i;
                }

                ++i;
            }

            return -1;
        }

        public static bool TryGet<T>(this IEnumerable<T> _instance, Func<T, bool> _predicate, out T _value)
        {
            foreach (var element in _instance)
            {
                if (_predicate(element))
                {
                    _value = element;
                    return true;
                }
            }

            _value = default;
            return false;
        }
    }
}