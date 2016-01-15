using System.Collections.Generic;
using System.Reflection;

namespace DynamicAssemblyGenerator
{
	public class ILContext
	{
		#region Constructors

		public ILContext()
		{
			Fields = new Dictionary<string, FieldInfo>();
			Parameters = new Dictionary<string, int>();
			Locals = new Dictionary<string, int>();

			Parameters.Add("this", 0);
		}

		public ILContext(FieldSet fields)
			: this()
		{
			foreach (var field in fields.Fields)
			{
				Fields.Add(field.Name, field);
			}
		}

		#endregion

		#region Properties

		public Dictionary<string, FieldInfo> Fields { get; private set; }
		public Dictionary<string, int> Parameters { get; private set; }
		public Dictionary<string, int> Locals { get; private set; }

		#endregion
	}
}
