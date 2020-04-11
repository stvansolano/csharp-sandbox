using System.Net.Http;

namespace CSharp8Examples 
{
    partial class Program 
    {
        public static void RunUsingDeclaration()
        {
            using var thing = new HttpClient();

            thing.GetAsync("https://dummy.restapiexample.com/api/v1/employees");
        }
    }
}