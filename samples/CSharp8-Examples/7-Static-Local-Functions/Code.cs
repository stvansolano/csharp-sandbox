namespace CSharp8Examples
{
    partial class Code
    {
        int M()
        {
            int y = 5;
            int x = 7;
            int a = 0;

            return Add(x, y);

            //static
            int Add(int left, int right) => a + left + right;
        }
    }
}