using System;

namespace Conzole
{
    /// <summary>
    /// The available options for the prompt function.
    /// </summary>
    public class PromptOptions
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PromptOptions() {}

        /// <summary>
        /// Constructor
        /// </summary>
        public PromptOptions(string prompt)
        {
            Prompt = prompt;    
        }

        /// <summary>
        /// The default prompt to display.
        /// </summary>
        public string Prompt { get; set; } = string.Empty;

        /// <summary>
        /// If true, a newline will be added after the prompt has finished printing. Default true.
        /// </summary>
        public bool PostNewLine { get; set; } = true;

    }
}