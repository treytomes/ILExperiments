using System.Data;
using System.Reflection;

namespace AOPTest.Aspect02
{
	public class ExternalFilter : AfterAttribute
	{
		#region Methods

		public override object Action(object target, MethodBase method, object[] parameters, object result)
		{
			DataRowCollection rows = (result as DataSet).Tables[0].Rows;

			for (int i = 0; i < rows.Count; i++)
			{
				if (!(bool)rows[i]["External"])
				{
					rows[i].Delete();
					i--;
				}
			}

			((DataSet)result).AcceptChanges();

			return result;
		}

		#endregion
	}
}