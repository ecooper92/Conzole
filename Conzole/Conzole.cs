using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Conzole
{
    /// <summary>
    /// Provides helper functions for managing console applications.
    /// </summary>
    public static class ConzoleUtils
    {
        // Start with the default console.
        private static IConsole _console = new DefaultConsole();

        /// <summary>
        /// Prompts the user to input a string value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        public static string Prompt(string prompt = "")
        {
            // Prompt if provided.
            if (!string.IsNullOrEmpty(prompt))
            {
                _console.WriteLine(prompt);
            }

            // Read new input.
            var result = _console.ReadLine();
            _console.WriteLine();
            return result;
        }

        /// <summary>
        /// Prompts the user to input an integer value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        /// <param name="result">The result of the user input.</param>
        public static bool PromptInt(string prompt, out int result) => int.TryParse(Prompt(prompt), out result);

        /// <summary>
        /// Prompts the user to input an double value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        /// <param name="result">The result of the user input.</param>
        public static bool PromptDouble(string prompt, out double result) => double.TryParse(Prompt(prompt), out result);

        /// <summary>
        /// Prompts the user to input a binary response (true/false, yes/no, etc..).
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        /// <param name="options">Optional parameters to modify binary question.</param>
        public static bool BinaryQuestion(string prompt, BinaryQuestionOptions options = null)
        {
            var binaryOptions = options ?? new BinaryQuestionOptions();
            var answer = string.Empty;
            var isPositive = false;
            var isNegative = false;
            var comparisonType = binaryOptions.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            do
            {
                answer = Prompt(binaryOptions.Formatter(prompt));
                isPositive = string.Equals(answer, binaryOptions.PositiveResponse, comparisonType);
                isNegative = string.Equals(answer, binaryOptions.NegativeResponse, comparisonType);
            }
            while (binaryOptions.RepeatOnInvalid && !isPositive && !isNegative);

            return isPositive;
        }

        /// <summary>
        /// Lists a collection of items for display.
        /// </summary>
        /// <param name="items">The items to display.</param>
        /// <param name="options">Optional parameters to modify list.</param>
        public static void List<T>(IEnumerable<T> items, ListOptions<T> options = null)
        {
            var listOptions = options ?? new ListOptions<T>();

            // Display title if provided.
            if (!string.IsNullOrEmpty(listOptions.Title))
            {
                _console.WriteLine(listOptions.Title);
            }

            // Display items in list.
            int index = 0;
            foreach (var item in items)
            {
                var displayIndex = listOptions.IndexGenerator(index++);
                var line = listOptions.LineFormatter(displayIndex, item);
                _console.WriteLine(line);
            }
            
            // Add extra newline for spacing if requested.
            if (listOptions.PostNewLine)
            {
                _console.WriteLine();
            }
        }

        /// <summary>
        /// Counts a collection of items for display.
        /// </summary>
        /// <param name="items">The items to display.</param>
        /// <param name="options">Optional parameters to modify count.</param>
        public static void Count<T>(IEnumerable<T> items, CountOptions options = null)
        {
            var countOptions = options ?? new CountOptions();

            _console.WriteLine(countOptions.ResultFormatter(items.Count()));
            _console.WriteLine();
        }
        
        /// <summary>
        /// Counts a collection of items for display.
        /// </summary>
        /// <param name="title">The items to display.</param>
        /// <param name="format">Custom format for count.</param>
        public static async Task MenuAsync(Menu menu)
        {
            var continueLooping = true;
            while (continueLooping)
            {
                // List nested menu items.
                var options = new ListOptions<KeyedValue<MenuItem>>();
                options.Title = menu.Title;
                options.LineFormatter = (index, keyedMenuItem) => menu.MenuItemFormatter(keyedMenuItem.Key, keyedMenuItem.Value);
                List(menu.KeyedMenuItems, options);

                // Perform menu item selection.
                var selectedMenuItem = menu.GetMenuItemByKey(Prompt(menu.InputPrompt));
                var selectedMenu = selectedMenuItem as Menu;
                if (selectedMenu != null)
                {
                    await MenuAsync(selectedMenu);
                }
                else if (selectedMenuItem != null)
                {
                    continueLooping = await selectedMenuItem.AsyncAction();
                }
                else
                {
                    _console.WriteLine(menu.InvalidInputPrompt);
                }
            }
        }

        public static async Task<bool> LoopActionPrompt(string prompt, Func<Task<bool>> action)
        {
            var success = false;

            var continueLooping = true;
            while (continueLooping)
            {
                success = await action();
                if (success || !BinaryQuestion(prompt))
                {
                    continueLooping = false;
                }
            }

            return success;
        }

        /// <summary>
        /// Sets the console used for writing and writing.
        /// </summary>
        /// <param name="console">The console to use for IO.</param>
        internal static void SetConsole(IConsole console) => _console = console;
    }
}