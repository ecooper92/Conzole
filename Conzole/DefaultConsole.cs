using System;

namespace Conzole
{
    /// <summary>
    /// Wraps the system console functions.
    /// </summary>
    public class DefaultConsole : IConsole
    {
        /// <summary>
        /// Write text with no termination.
        /// </summary>
        public void Write(string text) => Console.Write(text);

        /// <summary>
        /// Write line terminator.
        /// </summary>
        public void WriteLine() => Console.WriteLine();

        /// <summary>
        /// Write text with a line terminator.
        /// </summary>
        public void WriteLine(string text) => Console.WriteLine(text);

        /// <summary>
        /// Read text before a line terminator.
        /// </summary>
        public string ReadLine() => Console.ReadLine();

        /// <summary>
        /// Gets the arguments passed with the application on the command line.
        /// </summary>
        public string[] GetCommandLineArgs() => Environment.GetCommandLineArgs();
    }
}