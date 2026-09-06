using System;

namespace HwidChecker
{
    static class ConsoleHelper
    {
        const int Width = 62;

        public static void PrintTitleBanner(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔" + new string('═', Width - 2) + "╗");
            Console.WriteLine("║" + Center(title, Width - 2) + "║");
            Console.WriteLine("╚" + new string('═', Width - 2) + "╝");
            Console.ResetColor();
        }

        public static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"┌─ {title} " + new string('─', Math.Max(0, Width - title.Length - 5)));
            Console.ResetColor();
        }

        public static void PrintKeyValue(string key, string value, ConsoleColor? valueColor = null)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"  {key,-20}: ");
            Console.ForegroundColor = valueColor ?? ConsoleColor.White;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void PrintRow(params (string text, int width)[] columns)
        {
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var (text, width) in columns)
            {
                string trimmed = text.Length > width ? text.Substring(0, width - 1) + "…" : text;
                Console.Write(trimmed.PadRight(width));
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void PrintColumnTitles(params (string text, int width)[] columns)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            foreach (var (text, width) in columns)
                Console.Write(text.PadRight(width));
            Console.WriteLine();
            foreach (var (_, width) in columns)
                Console.Write(new string('-', width - 1).PadRight(width));
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  ! {message}");
            Console.ResetColor();
        }

        public static void PrintInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  {message}");
            Console.ResetColor();
        }

        static string Center(string text, int width)
        {
            if (text.Length >= width) return text.Substring(0, width);
            int leftPad = (width - text.Length) / 2;
            int rightPad = width - text.Length - leftPad;
            return new string(' ', leftPad) + text + new string(' ', rightPad);
        }
    }
}
