using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conzole
{
    /// <summary>
    /// The available options for the menu function.
    /// </summary>
    public class MenuOptions<T>
    {
        /// <summary>
        /// Performs the formating for a title. Default ToString()
        /// </summary>
        public Func<T, string> TitleFormatter { get; set; } = item => item.ToString();

        /// <summary>
        /// Given the zero-based index of the item in the list, converts that to the index to display. Defaults to incrementing numbers starting at 1.
        /// </summary>
        public Func<int, string> IndexGenerator { get; set; } = index => (index + 1).ToString();

        /// <summary>
        /// Performs the formating for a menu item. Default "<key>) <title>"
        /// </summary>
        public Func<string, MenuItem, string> MenuItemFormatter { get; set; } = (key, menuItem) => $"{key}) {menuItem.Title}";

        /// <summary>
        /// The text to display before the user inputs the menu selection.
        /// </summary>
        public string InputPrompt { get; set; } = "Enter selection:";

        /// <summary>
        /// The text to display if the menu selection is invalid.
        /// </summary>
        public string InvalidInputPrompt { get; set; } = "Invalid selection!";
    }
}