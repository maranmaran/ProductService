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
}
