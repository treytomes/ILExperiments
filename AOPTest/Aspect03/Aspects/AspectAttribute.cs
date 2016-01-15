using System;
using System.Reflection;

namespace AOPTest.Aspect03.Aspects
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Interface, Inherited = true)]
    public abstract class AspectAttribute : Attribute
    {
        public virtual void Before(object target, MethodBase method, object[] parameters)
        {
        }

        public virtual void After(object target, MethodBase method, object[] parameters, object result)
        {
        }

        /// <returns>True if the exception has been handled, false to pass it on.</returns>
        public virtual bool Exception(object target, MethodBase method, object[] parameters, Exception ex)
        {
            return false;
        }
    }
}