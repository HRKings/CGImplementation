using System;

namespace CGImplementation
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using var game = new CGWindow(args);
            game.Run();
        }
    }
}
