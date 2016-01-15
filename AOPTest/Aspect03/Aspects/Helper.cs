using System;
using System.IO;

namespace AOPTest.Aspect03.Aspects
{
	public class Helper
    {
        #region Methods

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