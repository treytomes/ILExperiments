using System;
using System.Reflection;
using System.Text;
using System.Threading;

namespace AOPTest.Aspect03.Aspects
{
    public class LoggerExceptionToFile : AspectAttribute
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

        public override bool Exception(object target, MethodBase method, object[] parameters, Exception ex)
        {
            string namePrincipal = Thread.CurrentPrincipal.Identity.Name;
            if (namePrincipal == string.Empty)
            {
                namePrincipal = "Anonymous User";
            }

            namePrincipal = "User: " + namePrincipal;

            string text = new StringBuilder()
                .AppendFormat("Assembly: {0}", target).AppendLine()
                .AppendFormat("Method: {0}", method.Name).AppendLine().AppendLine()
                .AppendFormat("InnerException: {0}" + ex.InnerException)
                .ToString();

            string content = Helper.ReadFile(_pathInternal);

            try
            {
                Helper.SaveToFile(namePrincipal, text, content, _pathInternal);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}