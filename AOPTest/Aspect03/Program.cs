using System;

namespace AOPTest.Aspect03
{
	/// <summary>
	/// Aspect-oriented programming via code injection.
	/// Aspects are applied to an interface, which is then duck-typed to a class.
	/// </summary>
	/// <remarks>
	/// This looks just like Aspect02.  Why did I write this?
	/// Maybe it's just a more organized version of Aspect02.
	/// </remarks>
	public class Program
    {
        public static void Main()
        {
            var test = AspectFactory.Create<ITestCatcher>(new Test2());

            int starttime = Environment.TickCount;
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(test.Go(i) == null);
            }
            int endtime = Environment.TickCount;

            Console.WriteLine("Total time is: {0}", endtime - starttime);
        }
    }
}