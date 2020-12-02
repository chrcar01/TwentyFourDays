using System;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.Write("Input(q or quit to exit):");
                var input = Console.ReadLine();
                if (input.Equals("quit", StringComparison.OrdinalIgnoreCase) || input.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                try
                {
                    Console.WriteLine(Calculator.Evaluate(input));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
