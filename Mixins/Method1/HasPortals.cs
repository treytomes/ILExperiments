using System;
using System.Collections;
using System.Collections.Generic;

namespace Mixins.Method1
{
    public class HasPortals : IEnumerable<long>
    {
        private List<long> m_oPortals = new List<long>();

        public void AddPortal(long id)
        {
            if (m_oPortals.Contains(id))
            {
                throw new Exception(string.Format("Portal id {0} already exists.", id));
            }
            m_oPortals.Add(id);
        }

        public void RemovePortal(long id)
        {
            if (!m_oPortals.Contains(id))
            {
                throw new Exception(string.Format("Portal id {0} does not exist.", id));
            }
            m_oPortals.Remove(id);
        }

        public int Count
        {
            get
            {
                return m_oPortals.Count;
            }
        }

        public IEnumerator<long> GetEnumerator()
        {
            foreach (long id in m_oPortals)
            {
                yield return id;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (long id in m_oPortals)
            {
                yield return id;
            }
        }
    }
}
