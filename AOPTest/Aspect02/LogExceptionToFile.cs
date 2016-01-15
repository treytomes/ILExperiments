using System;
using System.Reflection;
using System.Text;
using System.Threading;

namespace AOPTest.Aspect02
{
	public class LoggerExceptionToFile : LogExceptionAttribute
	{
		#region Variables

		private string _pathInternal = @"c:\logException.txt";

		#endregion

		#region Constructors

		public LoggerExceptionToFile(string path)
		{
			_pathInternal = path;
		}

		public LoggerExceptionToFile()
		{
		}

		#endregion

		#region Methods

		public override object Action(object target, MethodBase method, object[] parameters, object result)
		{
			string namePrincipal = Thread.CurrentPrincipal.Identity.Name;
			if (namePrincipal == string.Empty)
			{
				namePrincipal = "Anonymous User";
			}

			namePrincipal = "User: " + namePrincipal;

			string text = new StringBuilder()
				.AppendFormat("Assambly: {0}", target).AppendLine()
				.AppendFormat("Method: {0}", method.Name).AppendLine().AppendLine()
				.AppendFormat("InnerException: {0}" + (result as Exception).InnerException)
				.ToString();

			string content = Helper.ReadFile(_pathInternal);

			try
			{
				Helper.SaveToFile(namePrincipal, text, content, _pathInternal);
			}
			catch
			{
				throw;
			}

			return null;
		}

		#endregion
	}
}