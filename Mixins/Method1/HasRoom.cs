using System;

namespace Mixins.Method1
{
    public class HasRoom
    {
        private long m_nRoom = 0;

        public long Room
        {
            get
            {
                return m_nRoom;
            }
            set
            {
                m_nRoom = value;
            }
        }
    }
}