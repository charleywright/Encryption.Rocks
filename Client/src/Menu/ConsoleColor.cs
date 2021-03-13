public static partial class Menu
{
    private static void ConsoleForegroundColor(int state, int target)
    {
        if (state == target)
        {
            System.Console.ForegroundColor = System.ConsoleColor.DarkRed;
        }
        else System.Console.ForegroundColor = System.ConsoleColor.White;
    }
}