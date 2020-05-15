﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Conzole
{
    /// <summary>
    /// Provides helper functions for managing console applications.
    /// </summary>
    public static class ConzoleUtils
    {
        internal const string DEFAULT_COUNT_FORMAT = "{0} result(s)";
        internal static readonly Func<string, string, string> DEFAULT_LIST_FORMATTER = (index, value) => $"{index}) {value}";

        // Start with the default console.
        private static IConsole _console = new DefaultConsole();

        /// <summary>
        /// Prompts the user to input a string value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        public static string Prompt(string prompt)
        {
            _console.WriteLine(prompt);
            var result = _console.ReadLine();
            Console.WriteLine();
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
        public static void List(string[] items) => List(items, DEFAULT_LIST_FORMATTER);

        /// <summary>
        /// Lists a collection of items for display.
        /// </summary>
        /// <param name="items">The items to display.</param>
        /// <param name="customFormatter">Function takes the index and string and returns the display value.</param>
        public static void List(string[] items, Func<string, string, string> customFormatter)
        {
            _console.WriteLine();

            for (int i = 0; i < items.Length; i++)
            {
                _console.WriteLine(customFormatter((i + 1).ToString(), items[i]));
            }
            
            _console.WriteLine();
        }

        /// <summary>
        /// Counts a collection of items for display.
        /// </summary>
        /// <param name="items">The items to display.</param>
        public static void Count<T>(T[] items) => Count(items, DEFAULT_COUNT_FORMAT);

        /// <summary>
        /// Counts a collection of items for display.
        /// </summary>
        /// <param name="items">The items to display.</param>
        /// <param name="format">Custom format for count.</param>
        public static void Count<T>(T[] items, string format)
        {
            _console.WriteLine(string.Format(format, items.Length));
            _console.WriteLine();
        }
        
        /// <summary>
        /// Counts a collection of items for display.
        /// </summary>
        /// <param name="title">The items to display.</param>
        /// <param name="format">Custom format for count.</param>
        public static async Task ListMenuAsync(Menu menu)
        {
            var continueLooping = true;
            while (continueLooping)
            {
                WriteMenu(menu);

                if (PromptInt("Enter action#", out var result)
                    && result <= menu.MenuItems.Length
                    && (menu.IncludeExit && result >= 0) || (result > 0))
                {
                    var selectedMenuItem = menu.MenuItems[result];
                    var selectedMenu = selectedMenuItem as Menu;
                    if (selectedMenu != null)
                    {
                        await ListMenuAsync(menu);
                    }
                    else if (!await selectedMenuItem.AsyncAction())
                    {
                        continueLooping = false;
                    }
                }
                else
                {
                    _console.WriteLine("Invalid action!");
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

        /// <summary>
        /// Write menu text to console.
        /// </summary>
        /// <param name="menu">The menu to write.</param>
        private static void WriteMenu(Menu menu)
        {
            // Write title.
            _console.WriteLine(menu.Title);

            // Write main cases.
            for (int i = 1; i < menu.MenuItems.Length; i++)
            {
                _console.WriteLine($"{i}) {menu.MenuItems[i].Title}");
            }

            // Write exit case if supported.
            if (menu.IncludeExit)
            {
                _console.WriteLine($"{0}) {menu.MenuItems[0].Title}");
            }
            
            _console.WriteLine();
        }
    }
}