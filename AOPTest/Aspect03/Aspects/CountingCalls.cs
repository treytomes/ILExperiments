using System.Collections.Generic;
using System.Reflection;

namespace AOPTest.Aspect03.Aspects
{
    public class CountingCalls : AspectAttribute
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

        public override void Before(object target, MethodBase method, object[] parameters)
        {
            if (!_calls.ContainsKey(method.Name))
            {
                _calls.Add(method.Name, 1);
            }
            else
            {
                _calls[method.Name]++;
            }
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