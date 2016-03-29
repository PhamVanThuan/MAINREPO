using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAHL.X2.Common
{
    [Serializable]
    public class X2MessageCollection : IX2MessageCollection
    {
        List<IX2Message> _all;
        List<IX2Message> _error;
        List<IX2Message> _warning;

        public X2MessageCollection()
        {
            _all = new List<IX2Message>();
            _error = new List<IX2Message>();
            _warning = new List<IX2Message>();
        }

        public X2MessageCollection(IEnumerable<IX2Message> list)
        {
            _all = new List<IX2Message>(list);
            _error = new List<IX2Message>();
            _warning = new List<IX2Message>();
        }

        public bool HasErrorMessages
        {
            get { return (_error.Count > 0); }
        }

        public bool HasWarningMessages
        {
            get { return (_warning.Count > 0); }
        }

        public ReadOnlyCollection<IX2Message> ErrorMessages
        {
            get { return new ReadOnlyCollection<IX2Message>(_error); }
        }

        public ReadOnlyCollection<IX2Message> WarningMessages
        {
            get { return new ReadOnlyCollection<IX2Message>(_warning); }
        }

        public void Add(IX2Message item)
        {
            _all.Add(item);
            switch (item.MessageType)
            {
                case X2MessageType.Error:
                    _error.Add(item);
                    break;
                case X2MessageType.Warning:
                    _warning.Add(item);
                    break;
            }
        }

        public void Clear()
        {
            _all.Clear();
            _error.Clear();
            _warning.Clear();
        }

        public int Count
        {
            get { return _all.Count; }
        }

        public IEnumerator<IX2Message> GetEnumerator()
        {
            return _all.GetEnumerator();
        }

        #region IList<IX2DomainMessage> Members

        public int IndexOf(IX2Message item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, IX2Message item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IX2Message this[int index]
        {
            get
            {
                return _all[index];
            }
            set
            {
                _all[index] = value;
            }
        }

        #endregion IList<IX2DomainMessage> Members

        #region ICollection<IX2DomainMessage> Members

        public bool Contains(IX2Message item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(IX2Message[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(IX2Message item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion ICollection<IX2DomainMessage> Members

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion IEnumerable Members
    }
}