using System.Collections.Generic;
using System.Reflection;

namespace AOPTest.Aspect02
{
	public class CountingCalls : BeforeAttribute
	{
		#region Variables

		private static Dictionary<string, int> _calls;

		#endregion

		#region Constructors

		static CountingCalls()
		{
			_calls = new Dictionary<string, int>();
		}

		#endregion

		#region Methods

		public override object Action(object target, MethodBase method, object[] parameters, object result)
		{
			if (!_calls.ContainsKey(method.Name))
			{
				_calls.Add(method.Name, 1);
			}
			else
			{
				_calls[method.Name]++;
			}

			return null;
		}

		public static int Calls(string methodName)
		{
			if (!_calls.ContainsKey(methodName))
			{
				return 0;
			}
			else
			{
				return _calls[methodName];
			}
		}

		#endregion
	}
}