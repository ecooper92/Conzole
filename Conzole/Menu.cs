using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conzole
{
    /// <summary>
    /// A collections of menu items.
    /// </summary>
    public class Menu : MenuItem
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Menu(string title, IEnumerable<MenuItem> menuItems, bool includeExit = true, string exitText = "Back")
            : base(title)
        {
            IncludeExit = includeExit;
            MenuItems = new MenuItem[menuItems.Count() + 1];

            int index = 1;
            foreach (var menuItem in menuItems)
            {
                MenuItems[index++] = menuItem;
            }

            if (IncludeExit)
            {
                MenuItems[0] = new MenuItem(exitText, () => Task.FromResult(false));
            }
        }

        /// <summary>
        /// The child menu items owned by the menu.
        /// </summary>
        public MenuItem[] MenuItems { get; }

        /// <summary>
        /// Supports an exit option.
        /// </summary>
        public bool IncludeExit { get; }
    }
}