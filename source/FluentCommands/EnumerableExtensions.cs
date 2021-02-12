using System;
using System.Collections.Generic;

namespace FluentCommands
{
    /// <summary>
    /// Extensions for <see cref="IEnumerable{T}"/>.
    /// </summary>
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Call given action for each element.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="action">Action to call for each element.</param>
        internal static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach(var element in enumerable)
            {
                action(element);
            }
        }
    }
}
