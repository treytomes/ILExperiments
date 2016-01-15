using System;
using System.Runtime.Remoting.Messaging;

namespace AOPTest.Aspect01
{
	public class MethodMessageUtil
	{
		public static string GetMethodName(IMethodMessage methodMessage)
		{
			string methodName = methodMessage.MethodName;
			switch (methodName)
			{
				case ".ctor":
					{
						return "Constructor";
					}
				case "FieldGetter":
				case "FieldSetter":
					{
						IMethodCallMessage methodCallMessage = (IMethodCallMessage)methodMessage;
						return (string)methodCallMessage.InArgs[1];
					}
			}
			if (methodName.EndsWith("Item"))
			{
				return methodName;
			}

			if (methodName.StartsWith("get_") || methodName.StartsWith("set_"))
			{
				return methodName.Substring(4);
			}
			if (methodName.StartsWith("add_"))
			{
				return methodName.Substring(4) + "+=";
			}
			if (methodName.StartsWith("remove_"))
			{
				return methodName.Substring(7) + "-=";
			}
			return methodName;
		}

		public static string GetTypeName(IMethodMessage methodMessage)
		{
			string fullTypeName = methodMessage.TypeName;
			string[] arr = fullTypeName.Split(new Char[] { ',', ',' });
			return arr[0];
		}

		public static string GetAssemblyName(IMethodMessage methodMessage)
		{
			string fullTypeName = methodMessage.TypeName;
			string[] arr = fullTypeName.Split(new Char[] { ',', ',' });
			return arr[1];
		}

		public static string GetExceptionName(IMethodReturnMessage returnedMessage)
		{
			if (returnedMessage.Exception != null)
			{
				return returnedMessage.Exception.GetType().ToString();
			}
			return "";
		}

		public static string GetExceptionMessage(IMethodReturnMessage returnedMessage)
		{
			if (returnedMessage.Exception != null)
			{
				return returnedMessage.Exception.Message;
			}
			return "";
		}
	}
}