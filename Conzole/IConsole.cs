namespace Conzole
{
    public interface IConsole
    {
        void Write(string text);

        void WriteLine();

        void WriteLine(string text);

        string ReadLine();
    }
}