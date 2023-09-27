using EcomerceEs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomercePart2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (InterfacciaUtente.VisualizzaMenu()) { }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n[System] Fine programma\n");
            Console.ReadKey();
            Console.ResetColor();
        }
    }
}
