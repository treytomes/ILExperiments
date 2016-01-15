using System;

namespace Mixins.Method1
{
    public class Entity
    {
        private string m_sName = "";
        private string m_sDescription = "";
        private long m_nID = 0;
        private long m_nRefCount = 0;

        public string Name
        {
            get
            {
                return m_sName;
            }
            set
            {
                m_sName = value;
            }
        }

        public string Description
        {
            get
            {
                return m_sDescription;
            }
            set
            {
                m_sDescription = value;
            }
        }

        public long ID
        {
            get
            {
                return m_nID;
            }
            set
            {
                m_nID = value;
            }
        }

        public void AddRef()
        {
            m_nRefCount++;
        }

        public void DelRef()
        {
            m_nRefCount--;
        }

        public long Ref
        {
            get
            {
                return m_nRefCount;
            }
        }
    }
}