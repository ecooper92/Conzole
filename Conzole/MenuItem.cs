using System;
using System.Threading.Tasks;

namespace Conzole
{
    /// <summary>
    /// An action that can be selected in a menu to be performed on the console.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuItem(string title, Func<Task> action)
            : this(title)
        {
            AsyncAction = async () =>
            {
                await action();
                return true;
            };
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuItem(string title, Func<Task<bool>> action)
            : this(title)
        {
            AsyncAction = action;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected internal MenuItem(string title)
        {
            Title = title;
        }

        /// <summary>
        /// The title to be displayed for the menu item.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The action for the menu item.
        /// </summary>
        public Func<Task<bool>> AsyncAction { get; }
    }
}