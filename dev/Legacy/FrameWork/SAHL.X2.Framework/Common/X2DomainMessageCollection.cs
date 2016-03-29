using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using SAHL.X2.Framework.Common;

namespace SAHL.Common.Collections
{
    [Serializable]
    public class X2DomainMessageCollection : IX2DomainMessageCollection
    {
        List<IX2DomainMessage> _all;
        List<IX2DomainMessage> _error;
        List<IX2DomainMessage> _warning;

        public X2DomainMessageCollection()
        {
            _all = new List<IX2DomainMessage>();
            _error = new List<IX2DomainMessage>();
            _warning = new List<IX2DomainMessage>();
        }

        public X2DomainMessageCollection(IEnumerable<IX2DomainMessage> list)
        {
            _all = new List<IX2DomainMessage>(list);
            _error = new List<IX2DomainMessage>();
            _warning = new List<IX2DomainMessage>();
        }

        public bool HasErrorMessages
        {
            get { return (_error.Count > 0); }
        }

        public bool HasWarningMessages
        {
            get { return (_warning.Count > 0); }
        }

        public ReadOnlyCollection<IX2DomainMessage> ErrorMessages
        {
            get { return new ReadOnlyCollection<IX2DomainMessage>(_error); }
        }

        public ReadOnlyCollection<IX2DomainMessage> WarningMessages
        {
            get { return new ReadOnlyCollection<IX2DomainMessage>(_warning); }
        }


        public void Add(IX2DomainMessage item)
        {
            _all.Add(item);
            switch (item.MessageType)
            {
                case X2DomainMessageType.Error:
                    _error.Add(item);
                    break;
                case X2DomainMessageType.Warning:
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

        public IEnumerator<IX2DomainMessage> GetEnumerator()
        {
            return _all.GetEnumerator();
        }


        #region IList<IX2DomainMessage> Members

        public int IndexOf(IX2DomainMessage item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, IX2DomainMessage item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IX2DomainMessage this[int index]
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

        #endregion

        #region ICollection<IX2DomainMessage> Members


        public bool Contains(IX2DomainMessage item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(IX2DomainMessage[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(IX2DomainMessage item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
