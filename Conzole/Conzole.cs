﻿using System;
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
        public static string PromptString(string prompt)
        {
            _console.WriteLine(prompt);
            return _console.ReadLine();
        }

        /// <summary>
        /// Prompts the user to input an integer value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        /// <exception cref="System.ArgumentException">Thrown when a non integer value is input.</exception>
        public static int PromptInt(string prompt)
        {
            var input = PromptString(prompt);
            if (int.TryParse(input, out var result))
            {
                return result;
            }
            
            throw new ArgumentException($"The value '{input}' could not be converted to an integer.");
        }

        /// <summary>
        /// Prompts the user to input an double value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        /// <exception cref="System.ArgumentException">Thrown when a non double value is input.</exception>
        public static double PromptDouble(string prompt)
        {
            var input = PromptString(prompt);
            if (double.TryParse(input, out var result))
            {
                return result;
            }
            
            throw new ArgumentException($"The value '{input}' could not be converted to a double.");
        }

        /// <summary>
        /// Prompts the user to input a binary response (true/false, yes/no, etc..).
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        /// <param name="loopOnInvalid">If true, the user will be prompted until a valid response is provided, otherwise returns false after one failure.</param>
        /// <param name="positiveResponse">The response required for a positive result.</param>
        /// <param name="negativeResponse">The response required for a negative result.</param>
        /// <param name="ignoreCase">If true, responses can be case insensitive and still match.</param>
        public static bool PromptBinaryQuestion(string prompt, bool loopOnInvalid = true, string positiveResponse = "y", string negativeResponse = "n", bool ignoreCase = true)
        {
            var answer = string.Empty;
            var isPositive = false;
            var isNegative = false;
            var comparisonType = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            do
            {
                _console.WriteLine(prompt + $" ({positiveResponse}/{negativeResponse})");
                answer = _console.ReadLine();
                isPositive = string.Equals(answer, positiveResponse, comparisonType);
                isNegative = string.Equals(answer, negativeResponse, comparisonType);
            }
            while (loopOnInvalid && !isPositive && !isNegative);

            return isPositive;
        }

        /// <summary>
        /// Lists a collection of items for display.
        /// </summary>
        /// <param name="items">The items to display.</param>
        public static void List<T>(T[] items) => List(items, i => i.ToString());

        /// <summary>
        /// Lists a collection of items for display.
        /// </summary>
        /// <param name="items">The items to display.</param>
        /// <param name="stringConverter">Function that specifies how the item is displayed as a string.</param>
        public static void List<T>(T[] items, Func<T, string> stringConverter)
        {
            _console.WriteLine();

            for (int i = 0; i < items.Length; i++)
            {
                _console.WriteLine($"{i + 1}) {stringConverter(items[i])}");
            }
            
            _console.WriteLine();
        }

        /// <summary>
        /// Counts a collection of items for display.
        /// </summary>
        /// <param name="items">The items to display.</param>
        public static void Count<T>(T[] items, string format = "{0} result(s)")
        {
            _console.WriteLine();
            _console.WriteLine(string.Format(format, items.Length));
            _console.WriteLine();
        }
        
        public static async Task ListMenuAsync(string title, params ConzoleMenuItem[] actions)
        {
            _console.WriteLine();

            // Add back option to list of actions.
            var extendedActions = new ConzoleMenuItem[actions.Length + 1];
            extendedActions[0] = new ConzoleMenuItem("Back", () => Task.FromResult(false));
            actions.CopyTo(extendedActions, 1);

            // Loop across actions.
            var continueLooping = true;
            while (continueLooping)
            {
                // Display menu.
                _console.WriteLine(title);
                for (int i = 1; i < actions.Length; i++)
                {
                    _console.WriteLine($"{i}) {actions[i].Title}");
                }
                _console.WriteLine($"{0}) {actions[0].Title}");

                var action = _console.ReadLine();
                if (int.TryParse(action, out var result) && result >= 0 && result <= actions.Length)
                {
                    await actions[result - 1].ActionAsync();
                }
                else
                {
                    _console.WriteLine("Invalid action!");
                    _console.WriteLine();
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
                if (success || !PromptBinaryQuestion(prompt))
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