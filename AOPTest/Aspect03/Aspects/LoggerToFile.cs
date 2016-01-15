using System.Reflection;
using System.Threading;

namespace AOPTest.Aspect03.Aspects
{
    public class LoggerToFile : AspectAttribute
    {
        #region Variables

        private string _pathInternal = @"c:\log.txt";

        #endregion

        #region Constructors

        public LoggerToFile(string path)
        {
            _pathInternal = path;

        }

        public LoggerToFile()
        {
        }

        #endregion

        #region Methods

        public override void Before(object target, MethodBase method, object[] parameters)
        {
            string namePrincipal = Thread.CurrentPrincipal.Identity.Name;
            if (namePrincipal == string.Empty)
            {
                namePrincipal = "Anonymous User";
            }

            namePrincipal = "User: " + namePrincipal;

            string text = "Assembly: " + target.ToString() + "\nMethod: " + method.Name;

            string content = Helper.ReadFile(_pathInternal);

            try
            {
                Helper.SaveToFile(namePrincipal, text, content, _pathInternal);
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}