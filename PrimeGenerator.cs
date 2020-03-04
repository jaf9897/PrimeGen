using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PrimeGen
{
    /*
     * Class that does the actual work for the program
     * Generates and checks prime numbers.
     */
    public class PrimeGenerator
    {
        private static RNGCryptoServiceProvider RNGCSP = new RNGCryptoServiceProvider();

        private int count;
        private int primesGenerated = 0;
        private object _lock = new object();

        public PrimeGenerator()
        {
            this.count = 1;
        }        
        
        public PrimeGenerator(int count)
        {
            this.count = count;
        }

        /*
         * Main driver function, loops continuously using a Parallel implementation of a while loop
         * Generates random numbers and checks if they are prime.
         *
         * bits - the size of the number to generate, in bits
         */
        public void GeneratePrimes(int bits) {
            var clock = new Stopwatch();
            Console.WriteLine("BitLength: " + bits + " bits");
            clock.Start();
            ParallelWhile(new ParallelOptions(), () => primesGenerated < count, () => GeneratePrimeNumber(bits));
            Console.WriteLine("Time to Generate: " + clock.Elapsed);
        }

        private void GeneratePrimeNumber(int bits) {
            var bi = GenerateBigInt(bits);
            if (bi.IsProbablyPrime())
            {
                lock (_lock)
                {
                    if (primesGenerated < count)
                    {
                        primesGenerated++;
                        Console.WriteLine(primesGenerated + ": " + bi);
                        // To avoid extra new line before the time to generate gets printed out
                        if (primesGenerated != count)
                        {
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
        
        private BigInteger GenerateBigInt(int bits)
        {
            byte[] randomBits = new byte[bits / 8];
            RNGCSP.GetNonZeroBytes(randomBits);
            return new BigInteger(randomBits, true);
        }

        /*
         * While loop implemented using Parallel.ForEach
         * The collection is an IEnumerable<bool> that yields another entry until the condition is false
         * Terminates when the condition fails
         *
         * parallelOptions - configuration for the ForEach to use
         * condition - a function representing the while condition
         * body - the "body" of the while loop, or the code to execute in parallel
         */
        public void ParallelWhile(ParallelOptions parallelOptions, Func<bool> condition, Action body)
        {
            Parallel.ForEach(IterateUntilFalse(condition), parallelOptions,
                ignored => body());
        }
        

        private IEnumerable<bool> IterateUntilFalse(Func<bool> condition)
        {
            while (condition()) yield return true;
        }
    }
}