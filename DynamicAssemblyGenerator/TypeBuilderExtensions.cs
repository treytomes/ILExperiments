using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DynamicAssemblyGenerator
{
	public static class TypeBuilderExtensions
	{
		public static FieldBuilder DefineField<T>(this TypeBuilder @this, string fieldName, FieldAttributes attributes)
		{
			return @this.DefineField(fieldName, typeof(T), attributes);
		}

		public static MethodBuilder DefineMethod<TResult>(this TypeBuilder @this, string name, MethodAttributes attributes, CallingConventions callingConvention)
		{
			return @this.DefineMethod(name, attributes, callingConvention, typeof(TResult), new Type[0]);
		}

		public static MethodBuilder DefineMethod<TArg, TResult>(this TypeBuilder @this, string name, MethodAttributes attributes, CallingConventions callingConvention)
		{
			return @this.DefineMethod(name, attributes, callingConvention, typeof(TResult), new Type[] { typeof(TArg) });
		}
	}
}
