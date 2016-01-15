namespace Mixins.Method1
{
    public class HasRegion
    {
        private long m_nRegion = 0;

        public long Region
        {
            get
            {
                return m_nRegion;
            }
            set
            {
                m_nRegion = value;
            }
        }
    }
}