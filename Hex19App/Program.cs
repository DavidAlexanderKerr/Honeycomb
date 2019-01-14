using Honeycomb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex19App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Creating honeycomb ...");
            Honeycomb<long> honeycomb = ReadyHoneycombs.Hex19;
            Console.WriteLine(" done.");

            int steps;
            Console.Write("How many steps? ");
            string userSteps = Console.ReadLine();
            bool numEnetered = int.TryParse(userSteps, out steps);
            while(!numEnetered)
            {
                Console.WriteLine("Not a number.");
                Console.Write("How many steps? ");
                userSteps = Console.ReadLine();
                numEnetered = int.TryParse(userSteps, out steps);
            }

            Console.Write("Creating walker ...");
            var walker = new Walker(honeycomb, steps);
            walker.CacheingData += PrintCacheingMessage;
            Console.WriteLine(" done.");

            Console.WriteLine($"Walking {steps} steps ...");
            Cell<long> mostLikeley = walker.Walk();
            Console.WriteLine(" done.");

            Console.WriteLine($"Most likely destination: {mostLikeley.Key}");
            Console.WriteLine($"Number of hits: {mostLikeley.Data}");

            Console.Write("Saving results ...");
            string filepath=SaveResults(walker.Honeycomb);
            Console.WriteLine($" saved in {filepath}.");
            Console.ReadLine();
        }

        private static string SaveResults(Honeycomb<long> honeycomb)
        {
            var now = DateTime.Now;
            string filename = $"Hex19Results_{now:yyyyMMdd}_{now:HHmmss}.txt";
            string folder = @"C:\Users\lotop_000\Documents\Quizzes";
            string filepath = Path.Combine(folder, filename);
            honeycomb.Save(filepath);
            return filepath;
        }

        private static void PrintCacheingMessage(object sender,Walker.CacheingEventArgs cea)
        {
            Console.WriteLine(cea.Message);
        }
    }
}
