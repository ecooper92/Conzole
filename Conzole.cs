using System;
using System.Threading.Tasks;

namespace Conzole
{
    /// <summary>
    /// Provides helper functions for managing console applications.
    /// </summary>
    public static class Conzole
    {
        /// <summary>
        /// Prompts the user to input a string value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        public static string PromptString(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
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
                Console.WriteLine(prompt + $" ({positiveResponse}/{negativeResponse})");
                answer = Console.ReadLine();
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
            Console.WriteLine();

            for (int i = 0; i < items.Length; i++)
            {
                Console.WriteLine($"{i + 1}) {stringConverter(items[i])}");
            }

            Console.WriteLine();
            Console.WriteLine(items.Length + " result(s) found!");
            Console.WriteLine();
        }
        
        public static async Task ListMenuAsync(string title, params (string Title, Func<Task> Action)[] actions)
        {
            Console.WriteLine();

            var continueLooping = true;
            while (continueLooping)
            {
                Console.WriteLine(title);
                for (int i = 0; i < actions.Length; i++)
                {
                    Console.WriteLine($"{i + 1}) {actions[i].Title}");
                }
                Console.WriteLine("0) Back");

                var action = Console.ReadLine();
                if (int.TryParse(action, out var result) && result > 0 && result <= actions.Length)
                {
                    await actions[result - 1].Action();
                }
                else
                {
                    Console.WriteLine("Invalid action!");
                    Console.WriteLine();
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
    }
}