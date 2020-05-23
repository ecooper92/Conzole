using System;

namespace Conzole
{
    /// <summary>
    /// The available options for the count function.
    /// </summary>
    public class CountOptions
    {
        /// <summary>
        /// Formats the result of count. Default "<count> result(s)"
        /// </summary>
        public Func<int, string> ResultFormatter { get; set; } = count => $"{count} result(s)";

        /// <summary>
        /// If true, a newline will be added after the list has finished printing. Default true.
        /// </summary>
        public bool PostNewLine { get; set; } = true;
    }
}