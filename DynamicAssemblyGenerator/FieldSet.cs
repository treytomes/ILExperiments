using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DynamicAssemblyGenerator
{
	public class FieldSet
	{
		#region Variables

		private TypeBuilder _builder;

		#endregion

		#region Constructors

		public FieldSet(TypeBuilder tb)
		{
			_builder = tb;
			Fields = new List<FieldBuilder>();
		}

		#endregion

		#region Properties

		public FieldBuilder this[string name]
		{
			get
			{
				return (from field in Fields where field.Name == name select field).First();
			}
		}

		public List<FieldBuilder> Fields { get; private set; }

		#endregion

		#region Methods

		public void DefinePublicField<T>(string name)
		{
			Fields.Add(_builder.DefineField<T>(name, FieldAttributes.Public));
		}

		public void DefinePrivateField<T>(string name)
		{
			Fields.Add(_builder.DefineField<T>(name, FieldAttributes.Private));
		}

		#endregion
	}
}