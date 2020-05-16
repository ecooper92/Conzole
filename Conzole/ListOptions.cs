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

        /// <summary>
        /// Given the zero-based index of the item in the list, converts that to the index to display. Defaults to incrementing numbers starting at 1.
        /// </summary>
        public Func<int, string> IndexGenerator { get; set; } = index => (index + 1).ToString();

        /// <summary>
        /// If true, a newline will be added after the list has finished printing. Default true.
        /// </summary>
        public bool PostNewLine { get; set; } = true;
    }
}