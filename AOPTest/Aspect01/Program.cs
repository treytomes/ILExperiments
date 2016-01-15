using System;

namespace AOPTest.Aspect01
{
	/// <summary>
	/// Aspect-oriented programming using the message sink.
	/// It's a bit slow.
	/// </summary>
	public class Program
    {
        public static void Main()
        {
            AspectConfiguration.Instance.SetAssociation<Test1, TestAspect>();

			// Test1.Go will throw an exception, triggering TestAspect.Error.
			// TestAspect.End will be called regardless of the exception.
            var test = new Test1();

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