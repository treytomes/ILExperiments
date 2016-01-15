using System;
using System.Runtime.Remoting.Messaging;

namespace AOPTest.Aspect01
{
	public class TestAspect : AspectBase
	{
		private void Output(string s)
		{
			Console.WriteLine(s);
		}

		public override void Begin(object o, IMessage msg)
		{
			Output("Begin");
		}

		public override void End(object o, IMessage msg)
		{
			Output("End");
		}

		public override bool Error(Exception ex, object o, IMessage msg)
		{
			Output(ex.Message);
			return true;
		}
	}
}