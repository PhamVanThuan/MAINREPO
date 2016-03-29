using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.Collections
{
    [Serializable]
    public class DomainMessageCollection : MarshalByRefObject, IDomainMessageCollection, ISponsor
    {
        List<IDomainMessage> _all;
        List<IDomainMessage> _error;
        List<IDomainMessage> _info;
        List<IDomainMessage> _warning;

        public DomainMessageCollection()
            : this(new IDomainMessage[] {})
        {
        }

        ~DomainMessageCollection()
        {
            this.Dispose();
        }

        public DomainMessageCollection(IEnumerable<IDomainMessage> list)
        {
            _all = new List<IDomainMessage>(list);
            _error = new List<IDomainMessage>();
            _info = new List<IDomainMessage>();
            _warning = new List<IDomainMessage>();
        }

        public ILease Lease
        {
            get
            {
                return (ILease)RemotingServices.GetLifetimeService(this);
            }
        }

        public override object InitializeLifetimeService()
        {
            ILease lease = base.InitializeLifetimeService() as ILease;

            //Set lease properties
            lease.InitialLeaseTime = TimeSpan.FromSeconds(60);
            lease.RenewOnCallTime = TimeSpan.FromSeconds(30);
            lease.SponsorshipTimeout = TimeSpan.FromSeconds(60);

            lease.Register(this);
            return lease;
        }

        #region IDomainMessageCollection Members

        public bool HasErrorMessages
        {
            get { return (_error.Count > 0); }
        }

        public bool HasWarningMessages
        {
            get { return (_warning.Count > 0); }
        }

        public bool HasInfoMessages
        {
            get { return (_info.Count > 0); }
        }

        public ReadOnlyCollection<IDomainMessage> ErrorMessages
        {
            get { return new ReadOnlyCollection<IDomainMessage>(_error); }
        }

        public ReadOnlyCollection<IDomainMessage> WarningMessages
        {
            get { return new ReadOnlyCollection<IDomainMessage>(_warning); }
        }

        public ReadOnlyCollection<IDomainMessage> InfoMessages
        {
            get { return new ReadOnlyCollection<IDomainMessage>(_info); }
        }

        #endregion IDomainMessageCollection Members

        #region IList<IDomainMessage> Members

        public int IndexOf(IDomainMessage item)
        {
            return _all.IndexOf(item);
        }

        public void Insert(int index, IDomainMessage item)
        {
            _all.Insert(index, item);
            switch (item.MessageType)
            {
                case DomainMessageType.Error:
                    _error.Add(item);
                    break;
                case DomainMessageType.Info:
                    _info.Add(item);
                    break;
                case DomainMessageType.Warning:
                    _warning.Add(item);
                    break;
            }
        }

        public void RemoveAt(int index)
        {
            IDomainMessage item = _all[index];
            _all.RemoveAt(index);
            switch (item.MessageType)
            {
                case DomainMessageType.Error:
                    _error.Remove(item);
                    break;
                case DomainMessageType.Info:
                    _info.Remove(item);
                    break;
                case DomainMessageType.Warning:
                    _warning.Remove(item);
                    break;
            }
        }

        public IDomainMessage this[int index]
        {
            get
            {
                return _all[index];
            }
            set
            {
                IDomainMessage item = _all[index];
                _all[index] = value;
                // remove the olditem
                switch (item.MessageType)
                {
                    case DomainMessageType.Error:
                        _error.Remove(item);
                        break;
                    case DomainMessageType.Info:
                        _info.Remove(item);
                        break;
                    case DomainMessageType.Warning:
                        _warning.Remove(item);
                        break;
                }
                item = _all[index];
                // add the new item
                switch (item.MessageType)
                {
                    case DomainMessageType.Error:
                        _error.Add(item);
                        break;
                    case DomainMessageType.Info:
                        _info.Add(item);
                        break;
                    case DomainMessageType.Warning:
                        _warning.Add(item);
                        break;
                }
            }
        }

        #endregion IList<IDomainMessage> Members

        #region ICollection<IDomainMessage> Members

        public void Add(IDomainMessage item)
        {
            _all.Add(item);
            switch (item.MessageType)
            {
                case DomainMessageType.Error:
                    _error.Add(item);
                    break;
                case DomainMessageType.Info:
                    _info.Add(item);
                    break;
                case DomainMessageType.Warning:
                    _warning.Add(item);
                    break;
            }
        }

        public void Clear()
        {
            _all.Clear();
            _error.Clear();
            _info.Clear();
            _warning.Clear();
        }

        public bool Contains(IDomainMessage item)
        {
            return _all.Contains(item);
        }

        public void CopyTo(IDomainMessage[] array, int arrayIndex)
        {
            _all.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _all.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IDomainMessage item)
        {
            switch (item.MessageType)
            {
                case DomainMessageType.Error:
                    _error.Remove(item);
                    break;
                case DomainMessageType.Info:
                    _info.Remove(item);
                    break;
                case DomainMessageType.Warning:
                    _warning.Remove(item);
                    break;
            }
            return _all.Remove(item);
        }

        #endregion ICollection<IDomainMessage> Members

        #region IEnumerable<IDomainMessage> Members

        public IEnumerator<IDomainMessage> GetEnumerator()
        {
            return _all.GetEnumerator();
        }

        #endregion IEnumerable<IDomainMessage> Members

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _all.GetEnumerator();
        }

        #endregion IEnumerable Members

        public void Dispose()
        {
            ILease lease = this.Lease;
            if (lease != null)
            {
                lease.Renew(TimeSpan.Zero);
                lease.Unregister(this);
            }
        }

        public TimeSpan Renewal(ILease lease)
        {
            return TimeSpan.FromSeconds(10);
        }
    }
}