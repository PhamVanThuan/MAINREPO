using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.Collections
{
    public class SortedDAOEventList<T, T1, T2> : IEventList<T1>
    {
        private class SortedDAOEventListEnumerator<Te, T1e, T2e> : IEnumerator<T1e>, IEnumerator
        {
            IList<Te> _List;
            T1e _current;
            int Pos = 0;
            SAHLDictionary<int, int> _sortedListDict;

            public SortedDAOEventListEnumerator(IList<Te> lst, SAHLDictionary<int, int> sortedListDict)
            {
                this._List = lst;
                this._sortedListDict = sortedListDict;
            }

            #region IEnumerator<T1e> Members

            public T1e Current
            {
                get
                {
                    return _current;
                }
            }

            #endregion IEnumerator<T1e> Members

            #region IDisposable Members

            public void Dispose()
            {
                _List = null;
            }

            #endregion IDisposable Members

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return _current;
                }
            }

            public bool MoveNext()
            {
                if (Pos < _List.Count)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    T1e newobj = BMTM.GetMappedType<T1e, Te>(_List[_sortedListDict[Pos]]);
                    //T1 newobj = (T1)Activator.CreateInstance(typeof(T2), _List[Pos]);
                    if (newobj == null)
                        throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
                    else
                        _current = newobj;

                    Pos++;
                    return true;
                }
                Pos = _List.Count + 1;
                _current = default(T1e);
                return false;
            }

            public void Reset()
            {
                Pos = -1;
            }

            #endregion IEnumerator Members
        }

        IList<T> _DAOObjectList;
        IDictionary<string, string> _bindingDictionary;
        IDictionary<string, T1> _objectDictionary;
        SAHLDictionary<int, int> _sortedDAOObjectDict;

        bool _populateHybrids;
        string _hybridKeyProperty;
        string _hybridDataProperty;
        string _hybridDataSortProperty;
        bool _ascending;

        public SortedDAOEventList(IList<T> DAOList, string hybridKeyProperty, string hybridDataSortProperty, bool ascending)
        {
            this._DAOObjectList = (IList<T>)DAOList;
            this._hybridKeyProperty = hybridKeyProperty;
            this._hybridDataSortProperty = hybridDataSortProperty;
            this._ascending = ascending;
            GenerateSortedDictionary();
        }

        public SortedDAOEventList(IList<T> DAOList, string hybridKeyProperty, string hybridDataProperty, string hybridDataSortProperty, bool ascending)
        {
            _populateHybrids = true;
            this._DAOObjectList = (IList<T>)DAOList;
            this._hybridDataProperty = hybridDataProperty;
            this._hybridKeyProperty = hybridKeyProperty;
            this._hybridDataSortProperty = hybridDataSortProperty;
            this._ascending = ascending;
            _bindingDictionary = new Dictionary<string, string>();
            _objectDictionary = new Dictionary<string, T1>();

            // Change
            GenerateSortedDictionary();

            // Change
            //foreach (T DAOObj in _DAOObjectList)
            //{
            //    AddHybrid(DAOObj);
            //}

            foreach (KeyValuePair<int, int> kv in _sortedDAOObjectDict)
            {
                AddHybrid(_DAOObjectList[kv.Value]);
            }
        }

        private T GetDAOFromItem(T1 Item)
        {
            IDAOObject iobj = Item as IDAOObject;
            if (iobj != null)
            {
                T obj = (T)iobj.GetDAOObject();
                return obj;
            }
            return default(T);
        }

        public IList<T> GetDAOList()
        {
            return _DAOObjectList;
        }

        private T1 GetInterfaceObject(T DAO)
        {
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return BMTM.GetMappedType<T1, T>(DAO);
        }

        #region IEventList

        public event EventListHandler AfterAdd;
        public event EventListHandler AfterRemove;
        public event EventListHandler BeforeAdd;
        public event EventListHandler BeforeRemove;

        public IDictionary<string, string> BindableDictionary
        {
            get
            {
                return _bindingDictionary;
            }
        }

        public IDictionary<string, T1> ObjectDictionary
        {
            get
            {
                return _objectDictionary;
            }
        }

        public int IndexOf(T1 item)
        {
            // Change
            T obj = GetDAOFromItem(item);
            if (obj != null)
            {
                return _sortedDAOObjectDict.GetKeyByValue(_DAOObjectList.IndexOf(obj));
                //return _DAOObjectList.IndexOf(obj);
            }
            return -1;
        }

        public T1 this[int index]
        {
            // Change
            get
            {
                //return GetInterfaceObject(_DAOObjectList[index]);
                return GetInterfaceObject(_DAOObjectList[_sortedDAOObjectDict[index]]);
            }
            set
            {
                T obj = GetDAOFromItem(value);
                //_DAOObjectList[index] = obj;
                _DAOObjectList[_sortedDAOObjectDict[index]] = obj;
            }
        }

        public bool Contains(T1 item)
        {
            T obj = GetDAOFromItem(item);
            return _DAOObjectList.Contains(obj);
        }

        public void CopyTo(T1[] array, int arrayIndex)
        {
            // Change
            //for (int i = 0; i < _DAOObjectList.Count; i++)
            //{
            //    T1 newobj = GetInterfaceObject(_DAOObjectList[i]);
            //    array[arrayIndex + 1] = newobj;
            //}

            foreach (KeyValuePair<int, int> kv in _sortedDAOObjectDict)
            {
                T1 newobj = GetInterfaceObject(_DAOObjectList[kv.Value]);
                array[arrayIndex + 1] = newobj;
            }
        }

        public int Count
        {
            // Change
            // get { return _DAOObjectList.Count; }
            get { return _sortedDAOObjectDict.Count; }
        }

        public bool Insert(IDomainMessageCollection Messages, int index, T1 item)
        {
            // Change
            if (null != BeforeAdd)
            {
                CancelDomainArgs args = new CancelDomainArgs(Messages, EventListAction.Add);
                BeforeAdd(args, item);
                if (args.Cancel)
                    return false;
            }

            T obj = GetDAOFromItem(item);
            if (obj != null)
            {
                //_DAOObjectList.Insert(index, obj);
                _DAOObjectList.Insert(_sortedDAOObjectDict[index], obj);
                GenerateSortedDictionary();

                // check if we are in hybrid mode
                if (_populateHybrids)
                {
                    AddHybrid(item);
                }
            }
            else
                throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");

            if (null != AfterAdd)
                AfterAdd(new CancelDomainArgs(Messages, EventListAction.Add), item);

            return true;
        }

        public bool RemoveAt(IDomainMessageCollection Messages, int index)
        {
            // Change
            T1 item = (T1)Activator.CreateInstance(typeof(T2), _DAOObjectList[index]);
            if (null != BeforeRemove)
            {
                CancelDomainArgs args = new CancelDomainArgs(Messages, EventListAction.Remove);
                BeforeRemove(args, item);
                if (args.Cancel)
                    return false;
            }

            //_DAOObjectList.RemoveAt(index);
            _DAOObjectList.RemoveAt(_sortedDAOObjectDict[index]);
            GenerateSortedDictionary();

            // check if we are in hybrid mode
            if (_populateHybrids)
            {
                RemoveHybrid(item);
            }

            if (null != AfterRemove)
                AfterRemove(new CancelDomainArgs(Messages, EventListAction.Remove), item);

            return true;
        }

        public bool Add(IDomainMessageCollection Messages, T1 item)
        {
            // Change
            if (null != BeforeAdd)
            {
                CancelDomainArgs args = new CancelDomainArgs(Messages, EventListAction.Add);
                BeforeAdd(args, item);
                if (args.Cancel)
                    return false;
            }
            T obj = GetDAOFromItem(item);
            _DAOObjectList.Add(obj);
            GenerateSortedDictionary();

            // check if we are in hybrid mode
            if (_populateHybrids)
            {
                AddHybrid(item);
            }

            if (null != AfterAdd)
                AfterAdd(new CancelDomainArgs(Messages, EventListAction.Add), item);

            return true;
        }

        public bool Remove(IDomainMessageCollection Messages, T1 item)
        {
            // Change
            if (null != BeforeRemove)
            {
                CancelDomainArgs args = new CancelDomainArgs(Messages, EventListAction.Remove);
                BeforeRemove(args, item);
                if (args.Cancel)
                    return false;
            }
            T obj = GetDAOFromItem(item);

            _DAOObjectList.Remove(obj);
            GenerateSortedDictionary();

            // check if we are in hybrid mode
            if (_populateHybrids)
            {
                RemoveHybrid(item);
            }

            if (null != AfterRemove)
                AfterRemove(new CancelDomainArgs(Messages, EventListAction.Remove), item);

            return true;
        }

        #endregion IEventList

        #region Sorting Methods

        /// <summary>
        /// Sorts the elements in the entire List using the default comparer.
        /// </summary>
        public void Sort()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts the elements in the entire List using the specified System.Comparison.
        /// </summary>
        /// <param name="comparison">The System.Comparison to use when comparing elements.</param>
        public void Sort(Comparison<T1> comparison)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts the elements in the entire List using the specified comparer.
        /// </summary>
        /// <param name="comparer">The IComparer implementation to use when comparing elements, or a null reference to use the default comparer Comparer.Default.</param>
        public void Sort(IComparer<T1> comparer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorts the elements in a range of elements in List using the specified comparer.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">The IComparer implementation to use when comparing elements, or a null reference to use the default comparer Comparer.Default.</param>
        public void Sort(int index, int count, IComparer<T1> comparer)
        {
            throw new NotImplementedException();
        }

        #endregion Sorting Methods

        #region IEnumerable<T1> Members

        public IEnumerator<T1> GetEnumerator()
        {
            // Change
            return new SortedDAOEventListEnumerator<T, T1, T2>(_DAOObjectList, _sortedDAOObjectDict);
        }

        #endregion IEnumerable<T1> Members

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            // Change
            return new SortedDAOEventListEnumerator<T, T1, T2>(_DAOObjectList, _sortedDAOObjectDict);
        }

        #endregion IEnumerable Members

        #region Hybrid Helpers

        private string GetHybridKey(T1 Object)
        {
            PropertyInfo PI = Object.GetType().GetProperty(_hybridKeyProperty);
            return PI.GetValue(Object, null).ToString();
        }

        private string GetHybridData(T1 Object)
        {
            PropertyInfo PI = Object.GetType().GetProperty(_hybridDataProperty);
            string Data = PI.GetValue(Object, null).ToString();
            return Data.Trim();
        }

        private void AddHybrid(T1 item)
        {
            string Key = GetHybridKey(item);
            string Data = GetHybridData(item);
            // add to the object dictionary
            _objectDictionary.Add(Key, item);
            // add to the binding dictionary
            _bindingDictionary.Add(Key, Data);
        }

        private void AddHybrid(T daoItem)
        {
            T1 item = GetInterfaceObject(daoItem);
            string Key = GetHybridKey(item);
            string Data = GetHybridData(item);
            // add to the object dictionary
            _objectDictionary.Add(Key, item);
            // add to the binding dictionary
            _bindingDictionary.Add(Key, Data);
        }

        private void RemoveHybrid(T1 item)
        {
            string Key = GetHybridKey(item);
            // remove from the object dictionary
            _objectDictionary.Remove(Key);
            // remove from the bindable dictionary
            _bindingDictionary.Remove(Key);
        }

        #endregion Hybrid Helpers

        #region Helper Methods

        private void GenerateSortedDictionary()
        {
            List<T> tmpDAOObjectList = new List<T>(_DAOObjectList);
            _sortedDAOObjectDict = new SAHLDictionary<int, int>();

            if (_ascending)
            {
                tmpDAOObjectList.Sort(delegate(T cmp1, T cmp2)
                {
                    return Comparer.DefaultInvariant.Compare(cmp1.GetType().GetProperty(_hybridDataSortProperty).GetValue(cmp1, null), cmp2.GetType().GetProperty(_hybridDataSortProperty).GetValue(cmp2, null));
                });
            }
            else
            {
                tmpDAOObjectList.Sort(delegate(T cmp1, T cmp2)
                {
                    return Comparer.DefaultInvariant.Compare(cmp2.GetType().GetProperty(_hybridDataSortProperty).GetValue(cmp2, null), cmp1.GetType().GetProperty(_hybridDataSortProperty).GetValue(cmp1, null));
                });
            }

            for (int i = 0; i < tmpDAOObjectList.Count; i++)
            {
                for (int j = 0; j < _DAOObjectList.Count; j++)
                {
                    if (tmpDAOObjectList[i].GetType().GetProperty(_hybridKeyProperty).GetValue(tmpDAOObjectList[i], null).Equals(_DAOObjectList[j].GetType().GetProperty(_hybridKeyProperty).GetValue(_DAOObjectList[j], null)))
                    {
                        _sortedDAOObjectDict.Add(i, j);
                        break;
                    }
                }
            }

            tmpDAOObjectList = null;
        }

        #endregion Helper Methods
    }
}