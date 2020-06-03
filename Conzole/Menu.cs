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
        private KeyedValue<MenuItem> _exitKeyedMenuItem;
        private readonly List<KeyedValue<MenuItem>> _keyedMenuItems;
        private readonly Dictionary<string, MenuItem> _menuItemsByKey;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Menu(string title)
            : base(title)
        {
            _keyedMenuItems = new List<KeyedValue<MenuItem>>();
            _menuItemsByKey = new Dictionary<string, MenuItem>();

            // Include exit menu item by default.
            SetExitMenuItem("Back", "0");
        }

        /// <summary>
        /// Performs the formating for a menu item. Default "<key>) <title>"
        /// </summary>
        public Func<string, MenuItem, string> MenuItemFormatter { get; set; } = (key, menuItem) => $"{key}) {menuItem.Title}";

        /// <summary>
        /// Provides a default key based on the entry number in the menu. Defaults to incrementing numbers beginning with 1.
        /// </summary>
        public Func<int, string> KeyGenerator { get; set; } = index => (index + 1).ToString();

        /// <summary>
        /// The text to display before the user inputs the menu selection.
        /// </summary>
        public string InputPrompt { get; set; } = "Enter selection:";

        /// <summary>
        /// The text to display if the menu selection is invalid.
        /// </summary>
        public string InvalidInputPrompt { get; set; } = "Invalid selection!";

        /// <summary>
        /// The child menu items owned by the menu.
        /// </summary>
        public IEnumerable<KeyedValue<MenuItem>> KeyedMenuItems
        {
            get
            {
                var keyedMenuItems = _keyedMenuItems.ToList();
                if (_exitKeyedMenuItem != null)
                {
                    keyedMenuItems.Add(_exitKeyedMenuItem);
                }

                return keyedMenuItems;
            }
        }

        /// <summary>
        /// Gets a menu item by its key.
        /// </summary>
        public MenuItem GetMenuItemByKey(string key)
        {
            if (_menuItemsByKey.TryGetValue(key, out var menuItem))
            {
                return menuItem;
            }

            return null;
        }

        /// <summary>
        /// Attempts to add a menu item to the menu.
        /// </summary>
        public bool AddMenuItem(string key, MenuItem menuItem) => AddMenuItem(new KeyedValue<MenuItem>(key, menuItem), true);

        /// <summary>
        /// Attempts to add a menu item to the menu.
        /// </summary>
        public bool AddMenuItem(MenuItem menuItem) => AddMenuItem(new KeyedValue<MenuItem>(KeyGenerator(_keyedMenuItems.Count), menuItem), true);

        /// <summary>
        /// Attempts to remove a menu item from the menu.
        /// </summary>
        public bool RemoveMenuItem(string key)
        {
            if (_menuItemsByKey.TryGetValue(key, out var menuItem))
            {
                _menuItemsByKey.Remove(key);
                _keyedMenuItems.RemoveAll(keyedItem => keyedItem.Key == key);
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds the exit menu item or replaces it if it already exists.
        /// </summary>
        public bool SetExitMenuItem(string exitText, string exitKey)
        {
            // Exclude any existing exit menu item.
            RemoveExitMenuItem();

            // Attempt to add the new item.
            var keyedMenuItem = new KeyedValue<MenuItem>(exitKey, new MenuItem(exitText, () => Task.FromResult(false)));
            var success = AddMenuItem(keyedMenuItem, false);
            if (success)
            {
                _exitKeyedMenuItem = keyedMenuItem;
            }
            
            return success;
        }

        /// <summary>
        /// Removes the exit menu item if it exists.
        /// </summary>
        public bool RemoveExitMenuItem()
        {
            var success = false;
            if (_exitKeyedMenuItem != null)
            {
                success = RemoveMenuItem(_exitKeyedMenuItem.Key);
            }

            _exitKeyedMenuItem = null;

            return success;
        }

        /// <summary>
        /// Attempts to add a menu item to the menu.
        /// </summary>
        private bool AddMenuItem(KeyedValue<MenuItem> keyedMenuItem, bool addToList)
        {
            try
            {
                _menuItemsByKey.Add(keyedMenuItem.Key, keyedMenuItem.Value);
                if (addToList)
                {
                    _keyedMenuItems.Add(keyedMenuItem);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}