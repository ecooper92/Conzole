using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static string Prompt(string prompt) => Prompt(new PromptOptions(prompt));

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
        /// <param name="result">The result of the user input.</param>
        public static bool PromptInt(out int result) => PromptInt(string.Empty, out result);

        /// <summary>
        /// Prompts the user to input an integer value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        /// <param name="result">The result of the user input.</param>
        public static bool PromptInt(string prompt, out int result) => PromptInt(new PromptOptions(prompt), out result);

        /// <summary>
        /// Prompts the user to input an integer value.
        /// </summary>
        /// <param name="options">The prompt options to provide to the user for input.</param>
        /// <param name="result">The result of the user input.</param>
        public static bool PromptInt(PromptOptions options, out int result) => int.TryParse(Prompt(options), out result);

        /// <summary>
        /// Prompts the user to input an double value.
        /// </summary>
        /// <param name="result">The result of the user input.</param>
        public static bool PromptDouble(out double result) => PromptDouble(string.Empty, out result);

        /// <summary>
        /// Prompts the user to input an double value.
        /// </summary>
        /// <param name="prompt">The prompt to provide to the user for input.</param>
        /// <param name="result">The result of the user input.</param>
        public static bool PromptDouble(string prompt, out double result) => PromptDouble(new PromptOptions(prompt), out result);

        /// <summary>
        /// Prompts the user to input an double value.
        /// </summary>
        /// <param name="options">The prompt options to provide to the user for input.</param>
        /// <param name="result">The result of the user input.</param>
        public static bool PromptDouble(PromptOptions options, out double result) => double.TryParse(Prompt(options), out result);

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
            // Get default options if none are provided.
            var countOptions = options ?? new CountOptions();

            // Display count of items.
            var line = countOptions.ResultFormatter(items.Count());
            _console.WriteLine(line);

            // Add extra newline for spacing if requested.
            if (countOptions.PostNewLine)
            {
                _console.WriteLine();
            }
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
        /// Provides a single level interactive menu for navigation within the user's console window.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="items">The menu items to display.</param>
        /// <param name="options">Optional parameters to modify menu.</param>
        public static T Menu<T>(string title, IEnumerable<T> items, MenuOptions<T> options = null)
        {
            var result = default(T);

            int key = 0;
            var menuOptions = options ?? new MenuOptions<T>();
            var menu = new Menu(title);
            menu.MenuItemFormatter = menuOptions.MenuItemFormatter;
            menu.InputPrompt = menuOptions.InputPrompt;
            menu.InvalidInputPrompt = menuOptions.InvalidInputPrompt;
            foreach (var item in items)
            {
                menu.AddMenuItem(menuOptions.IndexGenerator(key++), new MenuItem(menuOptions.TitleFormatter(item), () =>
                {
                    result = item;
                    return Task.FromResult(false);
                }));
            }

            ConzoleUtils.MenuAsync(menu).Wait(); // Wait is a hack for the moment, this will work because we control the menu inputs :\

            return result;
        }

        /// <summary>
        /// Repeats an action until it returns true or the user declines to continue.
        /// </summary>
        /// <param name="action">The action to repeat.</param>
        /// <param name="options">Optional parameters to modify repeat.</param>
        [Obsolete("Replaced by RepeatUntilSuccessAsync")]
        public static Task<bool> RepeatUntilSuccess(Func<Task<bool>> action, RepeatUntilSuccessOptions options = null) => RepeatUntilSuccessAsync(action, options);

        /// <summary>
        /// Repeats an action until it returns true or the user declines to continue.
        /// </summary>
        /// <param name="action">The action to repeat.</param>
        /// <param name="options">Optional parameters to modify repeat.</param>
        public static async Task<bool> RepeatUntilSuccessAsync(Func<Task<bool>> action, RepeatUntilSuccessOptions options = null)
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
        /// Gets the parsed the command line args as ordered and switched values.
        /// </summary>
        public static T GetCommandLineData<T>()
            where T : new()
        {
            var data = new T();

            var parameters = GetCommandLineParameters();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var orderedAttributes = property.GetCustomAttributes<OrderedParameterAttribute>().ToArray();
                var switchedAttributes = property.GetCustomAttributes<SwitchedParameterAttribute>().ToArray();
                if (orderedAttributes.Length == 0 && switchedAttributes.Length == 0)
                {
                    continue; // This is not a property configured to be read.
                }
                else if (orderedAttributes.Length > 1 || switchedAttributes.Length > 1 || (orderedAttributes.Length > 0 && switchedAttributes.Length > 0))
                {
                    throw new ArgumentException($"There must be only one OrderedParameter or SwitchedParameter attribute on property {property.Name}.");
                }

                // Get the property parser for an argument.
                var parser = GetCommandLineArgumentParser(property);
                if (parser == null)
                {
                    throw new ArgumentException($"Unhandled property type: {property.PropertyType.Name}");
                }

                var values = orderedAttributes.Length > 0 ? GetOrderedAttributeValues(orderedAttributes.First(), parameters) : GetSwitchedAttributeValues(switchedAttributes.First(), parameters);
                SetProperyValues(data, property, parser, values);
            }

            return data;
        }

        /// <summary>
        /// Gets the parsed the command line args as ordered and switched values.
        /// </summary>
        public static CommandLineParameters GetCommandLineParameters()
        {
            var args = _console.GetCommandLineArgs();

            var order = 0;
            var lastWasSwitch = false;
            var orderedParams = new List<OrderedParameter>();
            var switchedParams = new Dictionary<string, List<string>>();
            for (int i = 0; i < args.Length; i++)
            {
                var isSwitch = args[i].StartsWith("-") || args[i].StartsWith("/");
                if (lastWasSwitch)
                {
                    var switchName = args[i - 1].Substring(1);
                    var lastValue = string.Empty;
                    if (!isSwitch)
                    {
                        lastValue = args[i];
                        lastWasSwitch = false;
                    }

                    var switchValues = switchedParams.GetOrAdd(switchName);
                    switchValues.Add(lastValue);
                }
                else
                {
                    if (isSwitch)
                    {
                        lastWasSwitch = true;
                    }
                    else
                    {
                        orderedParams.Add(new OrderedParameter(order++, args[i]));
                    }
                }
            }

            return new CommandLineParameters(orderedParams, switchedParams.Select(p => new SwitchedParameter(p.Key, p.Value)));
        }

        /// <summary>
        /// Sets the console used for writing and writing.
        /// </summary>
        /// <param name="console">The console to use for IO.</param>
        internal static void SetConsole(IConsole console) => _console = console;

        /// <summary>
        /// Sets a property to the values provided on the given data object.
        /// </summary>
        private static string[] GetOrderedAttributeValues(OrderedParameterAttribute orderedAttribute, CommandLineParameters parameters)
        {
            var parameter = parameters.OrderedParameters.FirstOrDefault(p => p.Order == orderedAttribute.Order);
            if (parameter == null)
            {
                throw new ArgumentException($"Expected argument at position {orderedAttribute.Order} was not provided.");
            }

            return new string[] { parameter.Value };
        }

        /// <summary>
        /// Sets a property to the values provided on the given data object.
        /// </summary>
        private static string[] GetSwitchedAttributeValues(SwitchedParameterAttribute switchedAttribute, CommandLineParameters parameters)
        {            
            var parameter = parameters.SwitchedParameters.FirstOrDefault(p => p.Switch == switchedAttribute.Switch);
            if (parameter != null)
            {
                return parameter.Values.ToArray();
            }

            return new string[0];
        }

        /// <summary>
        /// Gets a string parser for the given property.
        /// </summary>
        private static Func<string, object> GetCommandLineArgumentParser(PropertyInfo property)
        {
            var type = property.PropertyType.IsArray ? property.PropertyType.GetElementType() : property.PropertyType;
            if (type == typeof(string))
            {
                return s => s;
            }
            else if (type == typeof(int))
            {
                return s => int.Parse(s);
            }
            else if (type == typeof(double))
            {
                return s => double.Parse(s);
            }
            else if (type == typeof(bool))
            {
                return s => string.Empty.Equals(s) || bool.Parse(s);
            }

            return null;
        }

        /// <summary>
        /// Sets a property to the values provided on the given data object.
        /// </summary>
        private static void SetProperyValues<T>(T data, PropertyInfo property, Func<string, object> parser, string[] values)
        {
            if (property.PropertyType.IsArray)
            {
                var newValues = (IList)Activator.CreateInstance(property.PropertyType, values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    newValues[i] = parser(values[i]); 
                }

                property.SetValue(data, newValues);
            }
            else
            {
                var firstValue = values.FirstOrDefault();
                if (firstValue != null)
                {
                    property.SetValue(data, parser(firstValue));
                }
            }
        }
    }
}