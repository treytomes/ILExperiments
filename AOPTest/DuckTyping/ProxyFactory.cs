using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace AOPTest.DuckTyping
{
	public class ProxyFactory
	{
		#region Constants

		private const string ASSEMBLY_NAME = "TempAssemblyInjection";
		private const string CLASS_NAME = "TempClassInjection";

		#endregion

		#region Variables

		private static TypeBuilder _typeBuilder;
		private static FieldBuilder _target;
		private static FieldBuilder _interface;

		#endregion

		#region Properties

		public static Func<object, MethodInfo, object[], object> InjectHandler
		{
			get
			{
				return (target, methodInfo, parameters) => methodInfo.Invoke(target, parameters);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Create a instance of our external type
		/// </summary>
		/// <param name="target">External type instance</param>
		/// <param name="interfaceType">Decorate interface methods with attributes</param>
		/// <returns>Intercepted type</returns>
		public static TInterface Create<TInterface>(object target)
			where TInterface : class
		{
			var proxyType = EmitProxyType(target.GetType(), typeof(TInterface));
			if (proxyType == null)
			{
				return null;
			}
			return (TInterface)Activator.CreateInstance(proxyType, new object[] { target, typeof(TInterface) });
		}

		/// <summary>
		/// Generate proxy type emiting IL code.
		/// </summary>
		/// <param name="targetType"></param>
		/// <param name="interfaceType"></param>
		/// <returns></returns>
		private static Type EmitProxyType(Type targetType, Type interfaceType)
		{
			// Get the current application domain for the current thread:
			var currentDomain = Thread.GetDomain();

			var assemblyName = new AssemblyName(ASSEMBLY_NAME);

			// Only save the custom-type dll while debugging:
#if SaveDLL && DEBUG
			var assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
			var moduleBuilder = assemblyBuilder.DefineDynamicModule(className, "Test.dll");
#else
			var assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
			var moduleBuilder = assemblyBuilder.DefineDynamicModule(CLASS_NAME);
#endif

			var type = moduleBuilder.GetType(ASSEMBLY_NAME + "__Proxy" + interfaceType.Name + targetType.Name);
			if (type == null)
			{
				_typeBuilder = moduleBuilder.DefineType(
					ASSEMBLY_NAME + "__Proxy" + interfaceType.Name + targetType.Name,
					TypeAttributes.Class | TypeAttributes.Public, targetType.BaseType,
					new Type[] { interfaceType });

				_target = _typeBuilder.DefineField("target", interfaceType, FieldAttributes.Private);
				_interface = _typeBuilder.DefineField("iface", typeof(Type), FieldAttributes.Private);

				EmitConstructor(_typeBuilder, _target, _interface);

				var methods = interfaceType.GetMethods();

				foreach (var m in methods)
				{
					if (targetType.GetMethod(m.Name) == null)
					{
						return null;
					}
					EmitProxyMethod(m, _typeBuilder);
				}

				type = _typeBuilder.CreateType();

			}

#if SaveDLL && DEBUG
			myAssemblyBuilder.Save("Test.dll");
#endif

			return type;
		}

		/// <summary>
		/// Generate the contructor of our proxy type
		/// </summary>
		/// <param name="typeBuilder">TypeBuilder needed to generate proxy type using IL code</param>
		/// <param name="target">Proxy type target</param>
		/// <param name="iface">Proxy type interface </param>
		private static void EmitConstructor(TypeBuilder typeBuilder, FieldBuilder target, FieldBuilder iface)
		{
			var objType = Type.GetType("System.Object");
			var objCtor = objType.GetConstructor(new Type[0]);

			var pointCtor = typeBuilder.DefineConstructor(
				MethodAttributes.Public,
				CallingConventions.Standard,
				new Type[] { typeof(object), typeof(Type) });
			var ctorIL = pointCtor.GetILGenerator();

			ctorIL.Emit(OpCodes.Ldarg_0);

			ctorIL.Emit(OpCodes.Call, objCtor);

			ctorIL.Emit(OpCodes.Ldarg_0);
			ctorIL.Emit(OpCodes.Ldarg_1);
			ctorIL.Emit(OpCodes.Stfld, target);

			ctorIL.Emit(OpCodes.Ldarg_0);
			ctorIL.Emit(OpCodes.Ldarg_2);
			ctorIL.Emit(OpCodes.Stfld, iface);

			ctorIL.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// Generate the method emiting IL Code 
		/// </summary>
		/// <param name="m">External method info</param>
		/// <param name="typeBuilder">TypeBuilder needed to generate proxy type using IL code</param>
		private static void EmitProxyMethod(MethodInfo methodInfo, TypeBuilder typeBuilder)
		{
			var paramTypes = (from info in methodInfo.GetParameters()
							  select info.ParameterType).ToArray();

			var mb = typeBuilder.DefineMethod(methodInfo.Name,
				MethodAttributes.Public | MethodAttributes.Virtual,
				methodInfo.ReturnType,
				paramTypes);

			var il = mb.GetILGenerator();

			// Create an array out of the method parameters:
			var parameters = il.DeclareLocal(typeof(object[]));
			il.Emit(OpCodes.Ldc_I4, paramTypes.Length);
			il.Emit(OpCodes.Newarr, typeof(object));
			il.Emit(OpCodes.Stloc, parameters);
			for (int i = 0; i < paramTypes.Length; i++)
			{
				il.Emit(OpCodes.Ldloc, parameters);
				il.Emit(OpCodes.Ldc_I4, i);
				il.Emit(OpCodes.Ldarg, i + 1);
				if (paramTypes[i].IsValueType)
				{
					il.Emit(OpCodes.Box, paramTypes[i]);
				}
				il.Emit(OpCodes.Stelem_Ref);
			}

			//
			// Parameter 1: object targetObject
			//

			// Get the MethodBase instance onto the stack:
			il.EmitCall(OpCodes.Call, typeof(ProxyFactory).GetProperty("InjectHandler").GetGetMethod(), null);
			// Get the first argument onto the stack:
			il.Emit(OpCodes.Ldarg_0);
			// Store the first argument into the _target field:
			il.Emit(OpCodes.Ldfld, (FieldInfo)_target);

			//
			// Parameter 2: MethodInfo methodInfo
			//

			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, (FieldInfo)_target);
			il.EmitCall(OpCodes.Call, typeof(object).GetMethod("GetType"), null);
			il.EmitCall(OpCodes.Call, typeof(MethodBase).GetMethod("GetCurrentMethod"), null);
			il.EmitCall(OpCodes.Call, typeof(ProxyFactory).GetMethod("GetMethodFromType"), null);

			//
			// Parameter 3 object[] parameters
			//

			il.Emit(OpCodes.Ldloc, parameters);

			// Invoke the method wrapper:
			il.EmitCall(OpCodes.Call, typeof(Func<object, MethodInfo, object[], object>).GetMethod("Invoke"), null);

			if (methodInfo.ReturnType == typeof(void))
			{
				il.Emit(OpCodes.Pop);
			}
			else if (methodInfo.ReturnType.IsValueType)
			{
				il.Emit(OpCodes.Unbox, methodInfo.ReturnType);
				il.Emit(OpCodes.Ldind_Ref);
			}
			il.Emit(OpCodes.Ret);
		}

		public static MethodInfo GetMethodFromType(Type type, MethodBase methodBase)
		{
			return type.GetMethod(methodBase.Name);
		}

		#endregion
	}
}
