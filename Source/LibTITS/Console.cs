using System;

namespace TITS
{
    /// <summary>
    /// Provides additional functionality to the <see cref="System.Console"/> class.
    /// </summary>
    public static class Console2
    {
        /// <summary>
        /// Writes the specified string value to the standard output stream, in the specified color.
        /// </summary>
        /// <param name="color">The foreground color to display the value in.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(ConsoleColor color, string value)
        {
            System.Console.ForegroundColor = color;
            System.Console.Write(value);
            System.Console.ResetColor();
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the standard output 
        /// stream using the specified format information, in the specified color.
        /// </summary>
        /// <param name="color">The foreground color to display the value in.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using <paramref name="format"/>.</param>
        public static void Write(ConsoleColor color, string format, params object[] arg)
        {
            System.Console.ForegroundColor = color;
            System.Console.Write(format, arg);
            System.Console.ResetColor();
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the 
        /// standard output stream, in the specified color.
        /// </summary>
        /// <param name="color">The foreground color to display the value in.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(ConsoleColor color, string value)
        {
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(value);
            System.Console.ResetColor();
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects, followed by the current 
        /// line terminator, to the standard output stream using the specified format information, in 
        /// the specified color.
        /// </summary>
        /// <param name="color">The foreground color to display the value in.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using <paramref name="format"/>.</param>
        public static void WriteLine(ConsoleColor color, string format, params object[] arg)
        {
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(format, arg);
            System.Console.ResetColor();
        }
    }
}