using System;
using System.Threading.Tasks;

namespace CSharp8Examples
{
    partial class Program
    {
        async static Task Main(string[] args)
        {
            Console.WriteLine($"Hello World! NET {Environment.Version.ToString()}");

            await Task.Delay(1000);
        }
    }
}








 //var result = RockPaperScissors("rock", "scissors");

 // 8-Async-Streams
/*await foreach (var number in GenerateSequence())
{
    Console.WriteLine(number);
}*/
