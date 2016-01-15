using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;

namespace AOPTest.Aspect01
{
	public class RegExMethNameMatcher : IMessageMatcher
	{
		private Regex _r = null;

		public RegExMethNameMatcher()
			: this("")
		{
		}

		public RegExMethNameMatcher(string filter)
		{
			_r = new Regex(filter);
		}

		public bool IsMatch(IMessage msg)
		{
			if (msg is IMethodMessage)
			{
				return _r.IsMatch(MethodMessageUtil.GetMethodName((IMethodMessage)msg));
			}
			return false;
		}
	}
}
