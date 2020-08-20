using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class ListExtensions
    {
        /// <summary>
        /// Returns whether or not given collection is empty or null
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

    }
    public static class HashSetExtensions
    {
        /// <summary>
        /// Adds range of values to hashset
        /// </summary>
        public static bool AddRange<T>(this HashSet<T> source, IEnumerable<T> items)
        {
            return items.Aggregate(true, (current, item) => current & source.Add(item));
        }
    }
}
