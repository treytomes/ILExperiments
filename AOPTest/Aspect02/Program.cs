using System;

namespace AOPTest.Aspect02
{
	/// <summary>
	/// Aspect-oriented programming via code injection.
	/// Aspects are applied to an interface, which is then duck-typed to a class.
	/// </summary>
	public class Program
    {
        public static void Main()
        {
            var test = CodeInjection.Create<ITestCatcher>(new Test2());

            int starttime = Environment.TickCount;
            for (int i = 0; i < 100; i++)
            {
                test.Go();
            }
            int endtime = Environment.TickCount;

            Console.WriteLine("Total time is: {0}", endtime - starttime);
        }
    }
}