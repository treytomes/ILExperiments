using System;

namespace Mixins.Method1
{
	/// <summary>
	/// One way to do mixins.  I'm not really proud of this method, but it works.
	/// It would work better on a dynamic class.
	/// </summary>
	/// <remarks>
	/// The only other way I've found to do this in C# is using extension classes
	/// with a ConditionalWeakTable to manage instance state.
	/// </remarks>
	public class Program
    {
        public static void Main()
        {
            Character c = new Character();
            c.Call("DoAction", "push", 0L, 0L, 0L, 0L);
            c.Call("DoAction", new Action());
            c.Call("set_Name", "Bob");
            Console.WriteLine(c.Call("get_Name"));
            Console.WriteLine();
            
            foreach (string method in c.ListMethods())
            {
                Console.WriteLine(method);
            }

            c.Call("set_Room", 50L);
            Console.WriteLine(c.AsType<HasRoom>().Room);
        }
    }
}