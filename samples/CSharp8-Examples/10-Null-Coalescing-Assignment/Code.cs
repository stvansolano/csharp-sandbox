using System;
using System.Collections.Generic;

namespace CSharp8Examples
{
    partial class Program
    {
        static void NullCoalescing()
        {
            List<int> numbers = null;
            int? i = null;

            numbers ??= new List<int>();
            numbers.Add(i ??= 1);

            i = null;
            numbers.Add(i ??= 2);

            i = null;
            numbers.Add(i ??= 3);

            Console.WriteLine(string.Join(" ", numbers));  // output: 17 17
            Console.WriteLine($"Last asigned was: {i}");  // output: 3
        }
    }
}