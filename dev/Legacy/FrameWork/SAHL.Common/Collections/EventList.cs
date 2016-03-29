using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.Collections
{
    public class EventList<T1> : EventListBase<T1>, IEventList<T1>
    {
        public EventList()
            : base()
        {
        }

        public EventList(IEnumerable<T1> collection)
            : base(collection)
        {
        }

        public EventList(string hybridKeyProperty, string hybridDataProperty)
            : base(hybridKeyProperty, hybridDataProperty)
        {
        }

        public EventList(IEnumerable<T1> collection, string hybridKeyProperty, string hybridDataProperty)
            : base(collection, hybridKeyProperty, hybridDataProperty)
        {
        }

        #region IEventList<T1> Members

        public event EventListHandler BeforeAdd;
        public event EventListHandler BeforeRemove;
        public event EventListHandler AfterAdd;
        public event EventListHandler AfterRemove;

        public new IDictionary<string, string> BindableDictionary
        {
            get
            {
                return base.BindableDictionary;
            }
        }

        public new IDictionary<string, T1> ObjectDictionary
        {
            get
            {
                return base.ObjectDictionary;
            }
        }

        public bool Add(IDomainMessageCollection Messages, T1 item)
        {
            if (null != BeforeAdd)
            {
                CancelDomainArgs args = new CancelDomainArgs(Messages, EventListAction.Add);
                BeforeAdd(args, item);
                if (args.Cancel)
                    return false;
            }
            _listInternal.Add(item);

            // check if we are in hybrid mode
            if (PopulateHybrids)
            {
                string Key = GetHybridKey(item);
                string Data = GetHybridData(item);
                // add to the object dictionary
                base.ObjectDictionary.Add(Key, item);
                // add to the binding dictionary
                base.BindableDictionary.Add(Key, Data);
            }

            if (null != AfterAdd)
                AfterAdd(new CancelDomainArgs(Messages, EventListAction.Add), item);

            return true;
        }

        public bool Insert(IDomainMessageCollection Messages, int index, T1 item)
        {
            if (null != BeforeAdd)
            {
                CancelDomainArgs args = new CancelDomainArgs(Messages, EventListAction.Add);
                BeforeAdd(args, item);
                if (args.Cancel)
                    return false;
            }
            _listInternal.Insert(index, item);
            // check if we are in hybrid mode
            if (PopulateHybrids)
            {
                string Key = GetHybridKey(item);
                string Data = GetHybridData(item);
                // add to the object dictionary
                base.ObjectDictionary.Add(Key, item);
                // add to the binding dictionary
                base.BindableDictionary.Add(Key, Data);
            }

            if (null != AfterAdd)
                AfterAdd(new CancelDomainArgs(Messages, EventListAction.Add), item);

            return true;
        }

        public bool Remove(IDomainMessageCollection Messages, T1 item)
        {
            if (null != BeforeRemove)
            {
                CancelDomainArgs args = new CancelDomainArgs(Messages, EventListAction.Add);
                BeforeRemove(args, item);
                if (args.Cancel)
                    return false;
            }
            _listInternal.Remove(item);

            // check if we are in hybrid mode
            if (PopulateHybrids)
            {
                string Key = GetHybridKey(item);
                // remove from the object dictionary
                base.ObjectDictionary.Remove(Key);
                // remove from the bindable dictionary
                base.BindableDictionary.Remove(Key);
            }

            if (null != AfterRemove)
                AfterRemove(new CancelDomainArgs(Messages, EventListAction.Remove), item);

            return true;
        }

        public bool RemoveAt(IDomainMessageCollection Messages, int index)
        {
            if (null != BeforeRemove)
            {
                CancelDomainArgs args = new CancelDomainArgs(Messages, EventListAction.Add);
                BeforeRemove(args, _listInternal[index]);
                if (args.Cancel)
                    return false;
            }

            // check if we are in hybrid mode
            if (PopulateHybrids)
            {
                T1 item = _listInternal[index];
                string Key = GetHybridKey(item);
                // remove from the object dictionary
                base.ObjectDictionary.Remove(Key);
                // remove from the bindable dictionary
                base.BindableDictionary.Remove(Key);
            }

            _listInternal.RemoveAt(index);

            if (null != AfterRemove)
                AfterRemove(new CancelDomainArgs(Messages, EventListAction.Remove), _listInternal[index]);

            return true;
        }

        #endregion IEventList<T1> Members
    }

    public class CancelDomainArgs : ICancelDomainArgs
    {
        private EventListAction _Action = EventListAction.Add;

        public EventListAction Action { get { return _Action; } }

        private bool _Cancel = false;

        public bool Cancel { get { return _Cancel; } set { _Cancel = value; } }

        private IDomainMessageCollection _Messages;

        public IDomainMessageCollection Messages { get { return _Messages; } }

        public CancelDomainArgs(IDomainMessageCollection Messages, EventListAction Action)
        {
            this._Action = Action;
            this._Messages = Messages;
        }
    }
}