using System;

namespace Conzole
{
    /// <summary>
    /// The available options for the RepeatAction function.
    /// </summary>
    public class RepeatUntilSuccessOptions
    {
        /// <summary>
        /// The prompt to display to continue repeating the action.
        /// </summary>
        public string ContinuePrompt { get; set; } = "Continue?";

        /// <summary>
        /// The positive response to the ContinuePrompt.
        /// </summary>
        public string PositiveResponse { get; set; } = "y";

        /// <summary>
        /// The negative response to the ContinuePrompt.
        /// </summary>
        public string NegativeResponse { get; set; } = "n";

        /// <summary>
        /// If true, ignores the case of the response to the ContinuePrompt.
        /// </summary>
        public bool IgnoreCase { get; set; } = true;
    }
}