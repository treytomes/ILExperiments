namespace Mixins.Method1
{
    public class LogicEntity : Entity
    {
        private LogicCollection m_oLogic = new LogicCollection();

        public bool AddLogic(string logic)
        {
            return false;
        }

        public bool AddExistingLogic(Logic logic)
        {
            return false;
        }

        public bool DelLogic(string logic)
        {
            return false;
        }

        public Logic GetLogic(string logic)
        {
            return null;
        }

        public bool HasLogic(string logic)
        {
            return false;
        }

        public int DoAction(Action action)
        {
            return 0;
        }

        public int DoAction(string act, long data1, long data2, long data3, long data4, string data)
        {
            return 0;
        }
        
        public int DoAction(string act, long data1, long data2, long data3, long data4)
        {
            return DoAction(act, data1, data2, data3, data4, "");
        }

        public int DoAction(string act, long data1, long data2, long data3)
        {
            return DoAction(act, data1, data2, data3, 0);
        }

        public int DoAction(string act, long data1, long data2)
        {
            return DoAction(act, data1, data2, 0);
        }

        public int DoAction(string act, long data1)
        {
            return DoAction(act, data1, 0);
        }

        public int DoAction(string act)
        {
            return DoAction(act, 0);
        }
    }
}
