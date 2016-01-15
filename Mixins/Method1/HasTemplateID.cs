namespace Mixins.Method1
{
    public class HasTemplateID
    {
        private long m_nTemplateID = 0;

        public long TemplateID
        {
            get
            {
                return m_nTemplateID;
            }
            set
            {
                m_nTemplateID = value;
            }
        }
    }
}