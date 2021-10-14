using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CSCI251_Project_2
{
    class PrimeGen
    {
        /// <summary>
        /// Main function of the Prime Number generator
        /// </summary>
        /// <param name="args">String array of input parameters.
        /// args[0] represents bits and args[1] is optional number of primes to be generated
        /// </param>
        static void Main(string[] args)
        {
            int bit = 0;
            try
            {
                bit = Convert.ToInt32(args[0]);
                if (bit % 8 != 0 || bit < 32)
                {
                    Console.WriteLine("Bit input must be bigger or equal to 32 and a multiple of 8.");
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine("Invalid Bit Input");
            }

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            
            rng.GetBytes(new byte[bit/8]);
        }
        /// <summary>
        /// Method responsible for checking if the BigInteger is prime
        /// </summary>
        /// <param name="source">BigInteger to be checked for prime</param>
        /// <param name="certainty">int representing the amount of times to check for the BigInteger (default 10)</param>
        /// <returns>bool representing if source is prime</returns>
        public static bool IsProbablePrime(BigInteger source, int certainty=10)
        {
            if(source == 2 || source == 3)
                return true;
            if(source < 2 || source % 2 == 0)
                return false;
 
            BigInteger d = source - 1;
            int s = 0;
 
            while(d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }
                
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[source.ToByteArray().LongLength];
            BigInteger a;
 
            for(int i = 0; i < certainty; i++)
            {
                do
                {
                    rng.GetBytes(bytes);
                    a = new BigInteger(bytes);
                }
                while(a < 2 || a >= source - 2);
 
                BigInteger x = BigInteger.ModPow(a, d, source);
                if(x == 1 || x == source - 1)
                    continue;
 
                for(int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if(x == 1)
                        return false;
                    if(x == source - 1)
                        break;
                }
 
                if(x != source - 1)
                    return false;
            }
 
            return true;
        }
    }
}