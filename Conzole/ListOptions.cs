using System;

namespace Conzole
{
    /// <summary>
    /// The available options for the list function.
    /// </summary>
    public class ListOptions<T>
    {
        /// <summary>
        /// Formats a line of the list. Default "<index>) <item>"
        /// </summary>
        public Func<string, T, string> LineFormatter { get; set; } = (index, item) => $"{index}) {item.ToString()}";
    }
}