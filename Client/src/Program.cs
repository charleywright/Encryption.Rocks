using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                switch (Statics.ProgramState)
                {
                    case 0:
                        Menu.Start().Wait();
                        break;
                    case 1:
                        Console.Clear();
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
