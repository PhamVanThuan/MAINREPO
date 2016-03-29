using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.DomainObjects
{
    internal class StubEventList<T> : List<T>, IEventList<T>
    {
        public StubEventList()
        {
        }

        public StubEventList(IEnumerable<T> toAdd)
        {
            base.AddRange(toAdd);
        }

        public bool Add(IDomainMessageCollection Messages, T value)
        {
            base.Add(value);
            return true;
        }

        public event EventListHandler AfterAdd;

        public event EventListHandler AfterRemove;

        public event EventListHandler BeforeAdd;

        public event EventListHandler BeforeRemove;

        public IDictionary<string, string> BindableDictionary
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Insert(IDomainMessageCollection Messages, int index, T value)
        {
            base.Insert(index, value);
            return true;
        }

        private Dictionary<string, T> _objectDictionary = new Dictionary<string, T>(1);

        public IDictionary<string, T> ObjectDictionary
        {
            get
            {
                return _objectDictionary;
            }
        }

        public bool Remove(IDomainMessageCollection Messages, T value)
        {
            base.Remove(value);
            return true;
        }

        public bool RemoveAt(IDomainMessageCollection Messages, int index)
        {
            base.RemoveAt(index);
            return true;
        }
    }
}