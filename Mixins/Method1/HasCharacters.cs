using System;
using System.Collections;
using System.Collections.Generic;

namespace Mixins.Method1
{
    public class HasCharacters : IEnumerable<long>
    {
        private List<long> m_oCharacters = new List<long>();

        public void AddCharacter(long id)
        {
            if (m_oCharacters.Contains(id))
            {
                throw new Exception(string.Format("Character id {0} already exists.", id));
            }
            m_oCharacters.Add(id);
        }

        public void RemoveCharacter(long id)
        {
            if (!m_oCharacters.Contains(id))
            {
                throw new Exception(string.Format("Character id {0} does not exist.", id));
            }
            m_oCharacters.Remove(id);
        }

        public int Count
        {
            get
            {
                return m_oCharacters.Count;
            }
        }

        public IEnumerator<long> GetEnumerator()
        {
            foreach (long id in m_oCharacters)
            {
                yield return id;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (long id in m_oCharacters)
            {
                yield return id;
            }
        }
    }
}
