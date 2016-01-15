using System;
using System.Reflection;

namespace DynamicAssemblyGenerator
{
	public static class TypeExtensions
	{
		public static MethodInfo GetMethod<TArg>(this Type @this, string name)
		{
			return @this.GetMethod(name, new Type[] { typeof(TArg) });
		}

		public static MethodInfo GetMethod<TArg0, TArg1>(this Type @this, string name)
		{
			return @this.GetMethod(name, new Type[] { typeof(TArg0), typeof(TArg1) });
		}
	}
}