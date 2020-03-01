using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PrimeGen
{
    class Program
    {
        static void Main(string[] args)
        {

            var work = new Worker();
            work.GeneratePrimes(1024);
        }

    }

    public class Worker
    {
        
        private static RNGCryptoServiceProvider RNGCSP = new RNGCryptoServiceProvider();
        private int count = 2;
        private int primesGenerated;
        private object _lock = new object();
        private List<Task> tasks = new List<Task>();

        public void GeneratePrimes(int bytes) {
            while (true) {
                /*if (primesGenerated == count)
                {
                    break;
                }
                GeneratePosiblePrime(bytes);
                */
                if (primesGenerated == count) {
                        break;
                }
                var t = Task.Run(() => GeneratePosiblePrime(bytes));
                tasks.Add(t);
            }
        }

        private void GeneratePosiblePrime(int bytes) {
            var bi = generateRandomBigInt(bytes);
/*
            if (bi.IsProbablyPrime()) {
                if (primesGenerated < count) {
                    primesGenerated++;
                    Console.WriteLine(primesGenerated.ToString() + ": " + bi);

                    if (primesGenerated != count) {
                        Console.WriteLine();
                    }
                }
            }*/
            if (bi.IsProbablyPrime()) {
                lock(_lock) {
                    if (primesGenerated < count) {
                        primesGenerated++;
                        Console.WriteLine(primesGenerated.ToString() + ": " + bi);

                        if (primesGenerated != count) {
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
        
        
        private BigInteger generateRandomBigInt(int bytes)
        {
            byte[] randomBits = new byte[bytes];
            RNGCSP.GetBytes(randomBits);
            return new BigInteger(randomBits, true);
        }
    }
    
    
    public static class Primer
    {
        public static Boolean IsProbablyPrime(this BigInteger value, int witnesses = 10) {
            if (value <= 1) return false;
            if (witnesses <= 0) witnesses = 10;
            BigInteger d = value - 1;
            int s = 0;
            while (d % 2 == 0) {
                d /= 2;
                s += 1;
            }
            Byte[] bytes = new Byte[value.ToByteArray().LongLength];
            BigInteger a;
            for (int i = 0; i < witnesses; i++) {
                do {
                    var Gen = new Random();
                    Gen.NextBytes(bytes);
                    a = new BigInteger(bytes);
                } while (a < 2 || a >= value - 2);
                BigInteger x = BigInteger.ModPow(a, d, value);
                if (x == 1 || x == value - 1) continue;
                for (int r = 1; r < s; r++) {
                    x = BigInteger.ModPow(x, 2, value);
                    if (x == 1) return false;
                    if (x == value - 1) break;
                }
                if (x != value - 1) return false;
            }
            return true;
        }
        
    }
}
