using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace AOPTest.Aspect02
{
	public class CodeInjection
	{
		#region Constants

		public const string ASSEMBLY_NAME = "TempAssemblyInjection";
		public const string CLASS_NAME = "TempClassInjection";

		#endregion

		#region Variables

		private static TypeBuilder _typeBuilder;
		private static FieldBuilder _target;
		private static FieldBuilder _interface;

		#endregion

		#region Properties

		public static MethodCall InjectHandler
		{
			get
			{
				return new MethodCall(InjectHandlerMethod);
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
		{
			Type proxyType = EmitProxyType(target.GetType(), typeof(TInterface));
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
			AssemblyBuilder assemblyBuilder;

			// Get the current application domain for the current thread:
			AppDomain currentDomain = Thread.GetDomain();

			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = ASSEMBLY_NAME;

			// Only save the custom-type dll while debugging:
#if SaveDLL && DEBUG
			assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
			ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(className, "Test.dll");
#else
			assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
			ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(CLASS_NAME);
#endif

			Type type = moduleBuilder.GetType(ASSEMBLY_NAME + "__Proxy" + interfaceType.Name + targetType.Name);
			if (type == null)
			{
				_typeBuilder = moduleBuilder.DefineType(
					ASSEMBLY_NAME + "__Proxy" + interfaceType.Name + targetType.Name,
					TypeAttributes.Class | TypeAttributes.Public, targetType.BaseType,
					new Type[] { interfaceType });

				_target = _typeBuilder.DefineField("target", interfaceType, FieldAttributes.Private);
				_interface = _typeBuilder.DefineField("iface", typeof(Type), FieldAttributes.Private);

				EmitConstructor(_typeBuilder, _target, _interface);

				MethodInfo[] methods = interfaceType.GetMethods();

				foreach (MethodInfo m in methods)
				{
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
		/// Generate the method emiting IL Code 
		/// </summary>
		/// <param name="m">External method info</param>
		/// <param name="typeBuilder">TypeBuilder needed to generate proxy type using IL code</param>
		private static void EmitProxyMethod(MethodInfo m, TypeBuilder typeBuilder)
		{
			Type[] paramTypes = Helper.GetParameterTypes(m);

			MethodBuilder mb = typeBuilder.DefineMethod(m.Name,
				MethodAttributes.Public | MethodAttributes.Virtual,
				m.ReturnType,
				paramTypes);

			ILGenerator il = mb.GetILGenerator();

			LocalBuilder parameters = il.DeclareLocal(typeof(object[]));
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

			il.EmitCall(OpCodes.Call, typeof(CodeInjection).GetProperty("InjectHandler").GetGetMethod(), null);

			// Parameter 1  object targetObject
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, (FieldInfo)_target);

			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, (FieldInfo)_target);

			il.EmitCall(OpCodes.Call, typeof(object).GetMethod("GetType"), null);
			il.EmitCall(OpCodes.Call, typeof(MethodBase).GetMethod("GetCurrentMethod"), null);
			//Parameter 2 MethodBase method
			il.EmitCall(OpCodes.Call, typeof(Helper).GetMethod("GetMethodFromType"), null);
			//Parameter 3  object[] parameters
			il.Emit(OpCodes.Ldloc, parameters);

			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, (FieldInfo)_interface);
			il.EmitCall(OpCodes.Call, typeof(MethodBase).GetMethod("GetCurrentMethod"), null);
			il.EmitCall(OpCodes.Call, typeof(Helper).GetMethod("GetMethodFromType"), null);

			il.Emit(OpCodes.Ldtoken, typeof(AspectAttribute));
			il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));

			il.Emit(OpCodes.Ldc_I4, 1);
			il.EmitCall(OpCodes.Callvirt, typeof(MethodInfo).GetMethod("GetCustomAttributes", new Type[] { typeof(Type), typeof(bool) }), null);

			//Parameter 4  AspectAttribute[] aspects
			il.EmitCall(OpCodes.Call, typeof(Helper).GetMethod("AspectUnion"), null);

			il.EmitCall(OpCodes.Call, typeof(MethodCall).GetMethod("Invoke"), null);

			if (m.ReturnType == typeof(void))
			{
				il.Emit(OpCodes.Pop);
			}
			else if (m.ReturnType.IsValueType)
			{
				il.Emit(OpCodes.Unbox, m.ReturnType);
				il.Emit(OpCodes.Ldind_Ref);
			}
			il.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// Generate the contructor of our proxy type
		/// </summary>
		/// <param name="typeBuilder">TypeBuilder needed to generate proxy type using IL code</param>
		/// <param name="target">Proxy type target</param>
		/// <param name="iface">Proxy type interface </param>
		private static void EmitConstructor(TypeBuilder typeBuilder, FieldBuilder target, FieldBuilder iface)
		{
			Type objType = Type.GetType("System.Object");
			ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);

			ConstructorBuilder pointCtor = typeBuilder.DefineConstructor(
				MethodAttributes.Public,
				CallingConventions.Standard,
				new Type[] { typeof(object), typeof(Type) });
			ILGenerator ctorIL = pointCtor.GetILGenerator();

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
		/// Injection handler
		/// </summary>
		/// <param name="target">Target type which will be intercepted</param>
		/// <param name="method">Methot to intercept</param>
		/// <param name="parameters">Addtional parameters</param>
		/// <param name="attributes">Attributes decore</param>
		/// <returns></returns>
		public static object InjectHandlerMethod(object target, MethodBase method, object[] parameters, AspectAttribute[] attributes)
		{
			object returnValue = null;

			foreach (AspectAttribute b in attributes)
			{
				if (b is BeforeAttribute)
				{
					b.Action(target, method, parameters, null);
				}
			}

			try
			{
				returnValue = target.GetType().GetMethod(method.Name).Invoke(target, parameters);
			}
			catch (Exception ex)
			{
				foreach (AspectAttribute b in attributes)
				{
					if (b is LogExceptionAttribute)
					{
						b.Action(target, method, parameters, ex);
					}
				}
				throw;
			}

			foreach (AspectAttribute a in attributes)
			{
				if (a is AfterAttribute)
				{
					a.Action(target, method, parameters, returnValue);
				}
			}

			return returnValue;
		}

		#endregion
	}
}