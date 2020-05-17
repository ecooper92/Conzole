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
        public static string Prompt() => Prompt(string.Empty);

        /// <summary>
        /// Prompts the user to input a string value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        public static string Prompt(string prompt) => Prompt(new PromptOptions { Prompt = prompt });

        /// <summary>
        /// Prompts the user to input a string value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        public static string Prompt(PromptOptions options)
        {
            // Prompt if provided.
            if (!string.IsNullOrEmpty(options.Prompt))
            {
                _console.WriteLine(options.Prompt);
            }

            // Read new input.
            var result = _console.ReadLine();
            if (options.PostNewLine)
            {
                _console.WriteLine();
            }

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
        /// Provides an interactive menu for navigation within the user's console window.
        /// </summary>
        /// <param name="menu">The menu to display.</param>
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

        /// <summary>
        /// Repeats an action until it returns true or the user declines to continue.
        /// </summary>
        /// <param name="action">The action to repeat.</param>
        /// <param name="options">Optional parameters to modify repeat.</param>
        public static async Task<bool> RepeatUntilSuccess(Func<Task<bool>> action, RepeatUntilSuccessOptions options = null)
        {
            var success = false;
            
            var repeatOptions = options ?? new RepeatUntilSuccessOptions();
            var binaryOptions = new BinaryQuestionOptions();
            binaryOptions.PositiveResponse = repeatOptions.PositiveResponse;
            binaryOptions.NegativeResponse = repeatOptions.NegativeResponse;
            binaryOptions.IgnoreCase = repeatOptions.IgnoreCase;
            binaryOptions.RepeatOnInvalid = true;

            do
            {
                success = await action();
            }
            while (!success && BinaryQuestion(repeatOptions.ContinuePrompt, binaryOptions));

            return success;
        }

        /// <summary>
        /// Sets the console used for writing and writing.
        /// </summary>
        /// <param name="console">The console to use for IO.</param>
        internal static void SetConsole(IConsole console) => _console = console;
    }
}