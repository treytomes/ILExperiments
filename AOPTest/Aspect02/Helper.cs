using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AOPTest.Aspect02
{
	public class Helper
	{
		#region Methods

		public static Type[] GetParameterTypes(MethodInfo method)
		{
			if (method == null)
			{
				return null;
			}

			return
				(from info in method.GetParameters()
				 select info.ParameterType).ToArray();
		}

		public static MethodInfo GetMethodFromType(Type type, MethodBase methodBase)
		{
			MethodInfo method = type.GetMethod(methodBase.Name);
			return method;
		}

		public static AspectAttribute[] AspectUnion(object[] obj)
		{
			AspectAttribute[] aAC = new AspectAttribute[obj.Length];

			int i = 0;
			foreach (AspectAttribute aA in obj)
			{
				aAC[i] = aA;
				i++;
			}
			return aAC;
		}

		public static void SaveToFile(string name, string message, string audit, string path)
		{
			try
			{
				FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
				StreamWriter sw = new StreamWriter(file);

				sw.Write(audit);
				sw.Write("\n\n-------------------------------------\n\n");
				sw.Write(name + " - " + System.DateTime.Now.ToLocalTime());
				sw.Write(sw.NewLine);
				sw.Write(message);
				sw.Write(sw.NewLine);
				sw.Close();

				// Close file
				file.Close();

			}
			catch (Exception ex)
			{
				string s = ex.Message;
				throw;
			}
		}

		public static string ReadFile(string path)
		{
			try
			{
				return File.ReadAllText(path);
			}
			catch
			{
				return string.Empty;
			}
		}

		#endregion
	}
}