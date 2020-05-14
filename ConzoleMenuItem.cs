using System;
using System.Threading.Tasks;

namespace Conzole
{
    /// <summary>
    /// An action that can be selected in a menu to be performed on the console.
    /// </summary>
    public class ConzoleMenuItem
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ConzoleMenuItem(string title, Func<Task> action)
        {
            Title = title;
            ActionAsync = async () =>
            {
                await action();
                return true;
            };
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        public ConzoleMenuItem(string title, Func<Task<bool>> action)
        {
            Title = title;
            ActionAsync = action;
        }

        /// <summary>
        /// The title tp be displayed for the menu item.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The action that is performed when the item is selected.
        /// If the task returns false, the menu will return to its parent.
        /// </summary>
        public Func<Task<bool>> ActionAsync { get; }
    }
}