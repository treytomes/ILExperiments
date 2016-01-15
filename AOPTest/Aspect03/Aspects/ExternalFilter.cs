using System.Data;
using System.Reflection;

namespace AOPTest.Aspect03.Aspects
{
    public class ExternalFilter : AspectAttribute
    {
        #region Methods

        public override void After(object target, MethodBase method, object[] parameters, object result)
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
        }

        #endregion
    }
}