using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace DynamicAssemblyGenerator
{
	/// <summary>
	/// Generate an assembly called QuoteOfTheDay, with a single namespace and class of the same name.
	/// 
	/// The generated class will read a text file for a list of quotes.
	/// It has one method, GetRandomQuote, that will pick a quote at random to print.
	/// </summary>
	public static class Program
	{
		public static void Main()
		{
			var an = new AssemblyName()
			{
				Version = new Version(1, 0, 0, 0),
				Name = "QuoteOfTheDay"
			};

			var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Save);
			var modBuilder = ab.DefineDynamicModule("QuoteOfTheDay", "QuoteOftheDay.dll");
			var tb = modBuilder.DefineType("QuoteOfTheDay.QuoteOfTheDay", TypeAttributes.Class | TypeAttributes.Public);
			var fields = new FieldSet(tb);
			fields.DefinePrivateField<ArrayList>("_quotes");
			fields.DefinePrivateField<Random>("_random");
			
			var ilgen = Generate_Constructor(tb, fields);

			//////////////////////////////////////////////////

			Generate_GetRandomQuote(ilgen, tb, fields);

			tb.CreateType();

			ab.Save("QuoteOfTheDay.dll");
		}

		private static ILGenerator Generate_Constructor(TypeBuilder tb, FieldSet fields)
		{
			var cb = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(string) });
			var ilgen = cb.GetILGenerator();
			ilgen.PushContext(new ILContext(fields));

			// --- DECLARE PARAMETERS ---
			ilgen.DeclareParameter("filename");
			// --- DECLARE LOCALS ---
			ilgen.DeclareLocal<TextReader>("tr");
			ilgen.DeclareLocal<string>("quote");

			ilgen.EmitCallToBaseConstructor();                                  // base();

			ilgen.ConstructVariable<ArrayList>("_quotes");                      // _quotes = new ArrayList();
			ilgen.ConstructVariable<Random>("_random");                         // _random = new Random();

			ilgen.EmitLoadVariable("filename");                                 // "filename" - first constructor parameter
			ilgen.EmitCall(typeof(File), "OpenText");                           // invoke File.OpenText with "filename" parameter
			ilgen.EmitStoreVariable("tr");

			// Mark the loop label
			ilgen.EmitLoop((gen, exit) =>
			{
				gen.EmitLoadVariable("tr");                                     // load local "tr"
				gen.EmitCallvirt<TextReader>("ReadLine");                       // call tr.ReadLine() (virtual)
				gen.EmitStoreVariable("quote");                                 // store the result in the local "quote"

				gen.EmitLoadVariable("quote");                                  // load the local "quote"
				gen.Emit(OpCodes.Brfalse_S, exit);                              // branch to the exit label if "quote" is null

				gen.EmitLoadVariable("_quotes");                                // load "_quotes" field
				gen.EmitLoadVariable("quote");                                  // load local "quote"
				gen.EmitCallvirt<ArrayList, object>("Add");                     // call _quotes.Add(object) (virtual)
				gen.Emit(OpCodes.Pop);                                          // pop the result of _quotes.Add(object) (unused)
			});

			ilgen.EmitLoadVariable("tr");                                       // load local "tr"
			ilgen.EmitCallvirt<TextReader>("Close");                            // call tr.Close() (virtual)

			ilgen.Emit(OpCodes.Ret);                                            // emit return opcode
			ilgen.PopContext();

			return ilgen;
		}

		private static void Generate_GetRandomQuote(ILGenerator ilgen, TypeBuilder tb, FieldSet fields)
		{
			var mb = tb.DefineMethod<string>("GetRandomQuote", MethodAttributes.Public, CallingConventions.Standard);
			ilgen = mb.GetILGenerator();
			ilgen.PushContext(new ILContext(fields));

			// --- DECLARE LOCALS ---
			ilgen.DeclareLocal<int>("count");

			ilgen.EmitLoadVariable("_quotes");                                  // load field "m_Quotes"
			ilgen.EmitCallvirt<ArrayList>("get_Count");                         // call _quotes.get_Count() (virtual)
			ilgen.EmitStoreVariable("count");                                   // store in local "count"

			ilgen.EmitLoadVariable("count");                                    // load local "count"
			ilgen.EmitIfFalse(gen =>
			{
				ilgen.Emit(OpCodes.Ldstr, "");                                  // load the string ""
				ilgen.Emit(OpCodes.Ret);                                        // return
			});

			ilgen.EmitLoadVariable("_quotes");                                  // load field "_quotes"
			ilgen.EmitLoadVariable("_random");                                  // load field "_random"
			ilgen.EmitLoadVariable("count");                                    // load local "count"
			ilgen.EmitCallvirt<Random, int>("Next");                            // call _random.Next(int) (virtual)
			ilgen.EmitCallvirt<ArrayList>("get_Item");                          // call _quotes.get_Item(int) (virtual)
			ilgen.Emit(OpCodes.Castclass, typeof(string));                      // cast the result to string

			ilgen.Emit(OpCodes.Ret);                                            // return
			ilgen.PopContext();
		}
	}
}
