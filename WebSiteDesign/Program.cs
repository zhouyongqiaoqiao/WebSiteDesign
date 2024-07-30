namespace WebSiteDesign
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int.TryParse("0123", out int result);

            int curentId =1;
            Console.WriteLine(curentId.ToString("D4"));
            Console.WriteLine("Hello, World!");
            Console.ReadLine();
        }
    }
}
