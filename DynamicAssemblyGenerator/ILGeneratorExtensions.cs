using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace DynamicAssemblyGenerator
{
	public static class ILGeneratorExtensions
	{
		#region Variables

		private static Dictionary<ILGenerator, Stack<ILContext>> _contexts;

		#endregion

		#region Constructors

		static ILGeneratorExtensions()
		{
			_contexts = new Dictionary<ILGenerator, Stack<ILContext>>();
		}

		#endregion

		#region Methods

		public static void PushContext(this ILGenerator @this, ILContext context)
		{
			if (!_contexts.ContainsKey(@this))
			{
				_contexts.Add(@this, new Stack<ILContext>());
			}
			_contexts[@this].Push(context);
		}

		public static void PopContext(this ILGenerator @this)
		{
			if (_contexts.ContainsKey(@this))
			{
				_contexts[@this].Pop();
			}
		}

		public static ILContext GetContext(this ILGenerator @this)
		{
			return _contexts[@this].Peek();
		}

		public static void EmitLoop(this ILGenerator @this, Action<ILGenerator, Label> action)
		{
			var label = @this.DefineLabel();
			var exit = @this.DefineLabel();
			@this.MarkLabel(label);
			action(@this, exit);
			@this.Emit(OpCodes.Br_S, label); // unconditional branch to the label
			@this.MarkLabel(exit);
		}

		/// <summary>
		/// Perform action if the value is false, zero, or null.
		/// </summary>
		public static void EmitIfFalse(this ILGenerator @this, Action<ILGenerator> action)
		{
			var lbl = @this.DefineLabel();
			@this.Emit(OpCodes.Brtrue_S, lbl);
			action(@this);
			@this.MarkLabel(lbl);
		}

		/// <summary>
		/// Perform action if the value is true, not zero, or not null.
		/// </summary>
		public static void EmitIfTrue(this ILGenerator @this, Action<ILGenerator> action)
		{
			var lbl = @this.DefineLabel();
			@this.Emit(OpCodes.Brfalse_S, lbl);
			action(@this);
			@this.MarkLabel(lbl);
		}

		public static void EmitCallToBaseConstructor(this ILGenerator @this)
		{
			@this.EmitLoadVariable("this");
			// Call the base constructor (no args):
			@this.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
		}

		#region Parameter Manipulation

		public static void DeclareParameter(this ILGenerator @this, string name)
		{
			if (@this.GetParameter(name) >= 0)
			{
				throw new Exception(string.Format("{0} is already defined.", name));
			}
			@this.GetContext().Parameters.Add(name, @this.GetContext().Parameters.Count);
		}

		public static int GetParameter(this ILGenerator @this, string name)
		{
			var context = @this.GetContext();
			if (!context.Parameters.ContainsKey(name))
			{
				return -1;
			}
			return context.Parameters[name];
		}

		public static void EmitLoadParameter(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Ldarg, @this.GetParameter(name));
		}

		public static void EmitStoreParameter(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Starg, @this.GetParameter(name));
		}

		#endregion

		#region Local Variable Manipulation

		public static LocalBuilder DeclareLocal<T>(this ILGenerator @this)
		{
			return @this.DeclareLocal(typeof(T));
		}

		public static LocalBuilder DeclareLocal<T>(this ILGenerator @this, string name)
		{
			if ((@this.GetParameter(name) >= 0) || (@this.GetLocal(name) >= 0))
			{
				throw new Exception(string.Format("{0} is already defined."));
			}
			var lb = @this.DeclareLocal<T>();
			@this.GetContext().Locals.Add(name, lb.LocalIndex);
			return lb;
		}

		public static int GetLocal(this ILGenerator @this, string name)
		{
			var context = @this.GetContext();
			if (!context.Locals.ContainsKey(name))
			{
				return -1;
			}
			return context.Locals[name];
		}

		public static void EmitLoadLocal(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Ldloc, @this.GetLocal(name));
		}

		public static void EmitStoreLocal(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Stloc, @this.GetLocal(name));
		}

		#endregion

		#region Field Manipulation

		public static void DeclareField(this ILGenerator @this, FieldInfo field)
		{
			@this.GetContext().Fields.Add(field.Name, field);
		}

		public static void DeclareFields(this ILGenerator @this, FieldSet fields)
		{
			foreach (var field in fields.Fields)
			{
				@this.DeclareField(field);
			}
		}

		public static FieldInfo GetField(this ILGenerator @this, string name)
		{
			var context = @this.GetContext();
			if (!context.Fields.ContainsKey(name))
			{
				return null;
			}
			return context.Fields[name];
		}

		public static void EmitLoadField(this ILGenerator @this, string name)
		{
			@this.EmitLoadParameter("this");
			@this.Emit(OpCodes.Ldfld, @this.GetField(name));
		}

		public static void EmitStoreField(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Stfld, @this.GetField(name));
		}

		public static void SetVariable(this ILGenerator @this, string name, ConstructorInfo constructor)
		{
			@this.EmitLoadVariable("this");
			@this.Emit(OpCodes.Newobj, constructor);
			@this.EmitStoreVariable(name);
		}

		public static void ConstructVariable<T>(this ILGenerator @this, string name)
		{
			@this.SetVariable(name, typeof(T).GetConstructor(Type.EmptyTypes));
		}

		public static void ConstructVariable<T, TArg>(this ILGenerator @this, string name)
		{
			@this.SetVariable(name, typeof(T).GetConstructor(new Type[] { typeof(TArg) }));
		}

		public static void ConstructVariable<T, TArg0, TArg1>(this ILGenerator @this, string name)
		{
			@this.SetVariable(name, typeof(T).GetConstructor(new Type[] { typeof(TArg0), typeof(TArg1) }));
		}

		#endregion

		#region Variable Manipulation

		public static void EmitLoadVariable(this ILGenerator @this, string name)
		{
			if (@this.GetLocal(name) >= 0)
			{
				@this.EmitLoadLocal(name);
			}
			else if (@this.GetParameter(name) >= 0)
			{
				@this.EmitLoadParameter(name);
			}
			else if (@this.GetField(name) != null)
			{
				@this.EmitLoadField(name);
			}
			else
			{
				throw new Exception(string.Format("{0} is not defined.", name));
			}
		}

		public static void EmitStoreVariable(this ILGenerator @this, string name)
		{
			if (@this.GetLocal(name) >= 0)
			{
				@this.EmitStoreLocal(name);
			}
			else if (@this.GetParameter(name) >= 0)
			{
				@this.EmitStoreParameter(name);
			}
			else if (@this.GetField(name) != null)
			{
				@this.EmitStoreField(name);
			}
			else
			{
				throw new Exception(string.Format("{0} is not defined.", name));
			}
		}

		#endregion

		#region Methods Call Emission

		public static void EmitCallvirt<T>(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Callvirt, typeof(T).GetMethod(name));
		}

		public static void EmitCallvirt<T, TArg>(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Callvirt, typeof(T).GetMethod<TArg>(name));
		}

		public static void EmitCallvirt<T, TArg0, TArg1>(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Callvirt, typeof(T).GetMethod<TArg0, TArg1>(name));
		}

		public static void EmitCall(this ILGenerator @this, Type type, string name)
		{
			@this.Emit(OpCodes.Call, type.GetMethod(name));
		}

		public static void EmitCall<T>(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Call, typeof(T).GetMethod(name));
		}

		public static void EmitCall<T, TArg>(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Call, typeof(T).GetMethod<TArg>(name));
		}

		public static void EmitCall<T, TArg0, TArg1>(this ILGenerator @this, string name)
		{
			@this.Emit(OpCodes.Call, typeof(T).GetMethod<TArg0, TArg1>(name));
		}

		#endregion

		#endregion
	}
}
