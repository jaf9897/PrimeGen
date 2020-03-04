/*
 * Jorge Flores
 * CSCI.251.02
 * Project 2 - PrimeGen
 * Generates large prime numbers using the C# parallel libraries.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;


namespace PrimeGen
{
    class Program
    {
        private static void Main(string[] args)
        {
            var clock = new Stopwatch();
            if (args.Length == 1)
            {
                var bits = 0;
                try
                {
                    bits = int.Parse(args[0]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Bits must be an integer.");
                    PrintConsoleHelp();
                    Environment.Exit(0);
                }

                var work = new PrimeGenerator();

                if (bits < 32 || bits % 8 != 0)
                {
                    Console.WriteLine("Invalid arguments.");
                    PrintConsoleHelp();
                    Environment.Exit(0);
                }
                
                clock.Start();
                work.GeneratePrimes(bits);
            }
            
            else if (args.Length == 2)
            {
                var bits = 0;
                var count = 0;
                try
                {
                    bits = int.Parse(args[0]);
                    count = int.Parse(args[1]);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Arguments must be integers.");
                    PrintConsoleHelp();
                    Environment.Exit(0);
                }

                var work = new PrimeGenerator(count);
                if (bits < 32 || bits % 8 != 0 || count <= 0)
                {
                    Console.WriteLine("Invalid arguments.");
                    PrintConsoleHelp();
                    Environment.Exit(0);
                }
                work.GeneratePrimes(bits);
            }

        }
        
        private static void PrintConsoleHelp()
        {
            Console.WriteLine("dotnet run PrimeGen <bits> <count=1>");
            Console.WriteLine("    - bits - the number of bits of the prime number, this must be a multiple of 8, and at least 32 bits.");
            Console.WriteLine("    - count - the number of prime numbers to generate, defaults to 1");
        }

    }

}
