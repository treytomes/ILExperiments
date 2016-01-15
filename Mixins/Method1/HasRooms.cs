using System;
using System.Collections;
using System.Collections.Generic;

namespace Mixins.Method1
{
    public class HasRooms : IEnumerable<long>
    {
        private List<long> m_oRooms = new List<long>();

        public void AddRoom(long id)
        {
            if (m_oRooms.Contains(id))
            {
                throw new Exception(string.Format("Room id {0} already exists.", id));
            }
            m_oRooms.Add(id);
        }

        public void RemoveRoom(long id)
        {
            if (!m_oRooms.Contains(id))
            {
                throw new Exception(string.Format("Room id {0} does not exist.", id));
            }
            m_oRooms.Remove(id);
        }

        public int Count
        {
            get
            {
                return m_oRooms.Count;
            }
        }

        public IEnumerator<long> GetEnumerator()
        {
            foreach (long id in m_oRooms)
            {
                yield return id;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (long id in m_oRooms)
            {
                yield return id;
            }
        }
    }
}
