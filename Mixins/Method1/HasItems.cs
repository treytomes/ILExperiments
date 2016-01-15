using System;
using System.Collections;
using System.Collections.Generic;

namespace Mixins.Method1
{
    public class HasItems : IEnumerable<long>
    {
        private List<long> m_oItems = new List<long>();

        public void AddItem(long id)
        {
            if (m_oItems.Contains(id))
            {
                throw new Exception(string.Format("Item id {0} already exists.", id));
            }
            m_oItems.Add(id);
        }

        public void RemoveItem(long id)
        {
            if (!m_oItems.Contains(id))
            {
                throw new Exception(string.Format("Item id {0} does not exist.", id));
            }
            m_oItems.Remove(id);
        }

        public int Count
        {
            get
            {
                return m_oItems.Count;
            }
        }

        public IEnumerator<long> GetEnumerator()
        {
            foreach (long id in m_oItems)
            {
                yield return id;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (long id in m_oItems)
            {
                yield return id;
            }
        }
    }
}
