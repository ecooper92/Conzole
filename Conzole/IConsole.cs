namespace Conzole
{
    /// <summary>
    /// Represents a console where data can be read and written.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Write text with no termination.
        /// </summary>
        void Write(string text);

        /// <summary>
        /// Write line terminator.
        /// </summary>
        void WriteLine();

        /// <summary>
        /// Write text with a line terminator.
        /// </summary>
        void WriteLine(string text);

        /// <summary>
        /// Read text before a line terminator.
        /// </summary>
        string ReadLine();

        /// <summary>
        /// Gets the arguments passed with the application on the command line.
        /// </summary>
        string[] GetCommandLineArgs();
    }
}