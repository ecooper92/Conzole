using System;

namespace Conzole
{
    /// <summary>
    /// The available options for the list function.
    /// </summary>
    public class ListOptions<T>
    {
        /// <summary>
        /// Formats an item in the list. Defaults to the ToString value of the item.
        /// </summary>
        public Func<T, string> ItemFormatter { get; set; } = item => item.ToString();

        /// <summary>
        /// Formats a line of the list. Default "<index>) <item>"
        /// </summary>
        public Func<string, string, string> LineFormatter { get; set; } = (index, item) => $"{index}) {item}";
    }
}