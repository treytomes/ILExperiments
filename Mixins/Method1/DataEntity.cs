using System;
using System.Collections;

namespace Mixins.Method1
{
    public class DataEntity
    {
        public class AttributeNotFoundException : Exception
        {
            public AttributeNotFoundException(string name)
                : base(string.Format("Attribute {0} not found.", name))
            {
            }
        }

        public class AttributeAlreadyExistsException : Exception
        {
            public AttributeAlreadyExistsException(string name)
                : base(string.Format("Attribute {0} already exists.", name))
            {
            }
        }

        private Hashtable m_oAttributes = new Hashtable();
        
        public int GetAttribute(string name)
        {
            if (!m_oAttributes.ContainsKey(name))
            {
                throw new AttributeNotFoundException(name);
            }
            return (int)m_oAttributes[name];
        }

        public void SetAttribute(string name, int value)
        {
            if (!m_oAttributes.ContainsKey(name))
            {
                throw new AttributeNotFoundException(name);
            }
            m_oAttributes[name] = value;
        }

        public bool HasAttribute(string name)
        {
            return m_oAttributes.ContainsKey(name);
        }

        public void AddAttribute(string name, int initialValue)
        {
            if (m_oAttributes.ContainsKey(name))
            {
                throw new AttributeAlreadyExistsException(name);
            }
            m_oAttributes.Add(name, initialValue);
        }

        public void DelAttribute(string name)
        {
            if (!m_oAttributes.ContainsKey(name))
            {
                throw new AttributeNotFoundException(name);
            }
            m_oAttributes.Remove(name);
        }
    }
}
