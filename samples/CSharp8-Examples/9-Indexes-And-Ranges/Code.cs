using System;

namespace CSharp8Examples
{
    partial class Program
    {
        public static void GetRanges()
        {
            var words = new string[]
            {
                            // index from start    index from end
                "The",      // 0                   ^9
                "quick",    // 1                   ^8
                "brown",    // 2                   ^7
                "fox",      // 3                   ^6
                "jumped",   // 4                   ^5
                "over",     // 5                   ^4
                "the",      // 6                   ^3
                "lazy",     // 7                   ^2
                "dog"       // 8                   ^1
            };              // 9 (or words.Length) ^0

            // dog
            Console.WriteLine($"The last word is {words[^1]}");

            Console.WriteLine($"Last 3 words are {String.Join(",", words[5..9])}");

            var lazyDog = words[^2..^0];

            Console.WriteLine($"The {string.Join(" ", lazyDog)}");

        }
    }
}