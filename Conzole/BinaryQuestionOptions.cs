using System;

namespace Conzole
{
    /// <summary>
    /// The available options for the PromptBinaryQuestion function.
    /// </summary>
    public class BinaryQuestionOptions
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BinaryQuestionOptions()
        {
            Formatter = prompt => $"{prompt} ({PositiveResponse}/{NegativeResponse})";
        }

        /// <summary>
        /// If true the prompt will repeat until and expected answer is provided, otherwise unexpected answers will result in a false result.
        /// </summary>
        public bool RepeatOnInvalid { get; set; } = true;
        
        /// <summary>
        /// The response that results in a true result. Default "y".
        /// </summary>
        public string PositiveResponse { get; set; } = "y";
        
        /// <summary>
        /// The response that results in a false result. Default "n".
        /// </summary>
        public string NegativeResponse { get; set; } = "n";
        
        /// <summary>
        /// If true, ignores the case of the responses.
        /// </summary>
        public bool IgnoreCase { get; set; } = true;

        /// <summary>
        /// The formatter of the binary question prompt.
        /// </summary>
        public Func<string, string> Formatter { get; set; }
    }
}