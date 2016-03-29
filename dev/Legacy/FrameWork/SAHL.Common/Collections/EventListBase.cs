using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace SAHL.Common.Collections
{
    public class EventListBase<T1> : IEnumerable<T1>, IEnumerable
    {
        protected List<T1> _listInternal;
        private bool _populateHybrids;
        private string _hybridKeyProperty;
        private string _hybridDataProperty;
        private IDictionary<string, string> _bindableDictionary = null;
        private IDictionary<string, T1> _objectDictionary = null;

        public EventListBase()
        {
            _listInternal = new List<T1>();
        }

        public EventListBase(string hybridKeyProperty, string hybridDataProperty)
        {
            _populateHybrids = true;
            _listInternal = new List<T1>();
            _hybridDataProperty = hybridDataProperty;
            _hybridKeyProperty = hybridKeyProperty;
        }

        public EventListBase(IEnumerable<T1> collection)
        {
            _listInternal = new List<T1>(collection);
        }

        public EventListBase(IEnumerable<T1> collection, string hybridKeyProperty, string hybridDataProperty)
        {
            _populateHybrids = true;
            _listInternal = new List<T1>(collection);
            _hybridDataProperty = hybridDataProperty;
            _hybridKeyProperty = hybridKeyProperty;
        }

        public bool Contains(T1 value)
        {
            return _listInternal.Contains(value);
        }

        public int IndexOf(T1 value)
        {
            return _listInternal.IndexOf(value);
        }

        public T1 this[int index]
        {
            get
            {
                return _listInternal[index];
            }
            set
            {
                _listInternal[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return _listInternal.Count;
            }
        }

        #region Sorting Methods

        /// <summary>
        /// Sorts the elements in the entire List using the default comparer.
        /// </summary>
        public void Sort()
        {
            _listInternal.Sort();
        }

        /// <summary>
        /// Sorts the elements in the entire List using the specified System.Comparison.
        /// </summary>
        /// <param name="comparison">The System.Comparison to use when comparing elements.</param>
        public void Sort(Comparison<T1> comparison)
        {
            _listInternal.Sort(comparison);
        }

        /// <summary>
        /// Sorts the elements in the entire List using the specified comparer.
        /// </summary>
        /// <param name="comparer">The IComparer implementation to use when comparing elements, or a null reference to use the default comparer Comparer.Default.</param>
        public void Sort(IComparer<T1> comparer)
        {
            _listInternal.Sort(comparer);
        }

        /// <summary>
        /// Sorts the elements in a range of elements in List using the specified comparer.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">The IComparer implementation to use when comparing elements, or a null reference to use the default comparer Comparer.Default.</param>
        public void Sort(int index, int count, IComparer<T1> comparer)
        {
            _listInternal.Sort(index, count, comparer);
        }

        #endregion Sorting Methods

        #region IEnumerable<T1> Members

        public IEnumerator<T1> GetEnumerator()
        {
            return _listInternal.GetEnumerator();
        }

        #endregion IEnumerable<T1> Members

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _listInternal.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region Protected members

        protected bool PopulateHybrids
        {
            get
            {
                return _populateHybrids;
            }
        }

        protected string HybridKeyProperty
        {
            get
            {
                return _hybridKeyProperty;
            }
        }

        protected string HybridDataProperty
        {
            get
            {
                return _hybridDataProperty;
            }
        }

        protected IDictionary<string, T1> ObjectDictionary
        {
            get
            {
                return _objectDictionary;
            }
        }

        protected IDictionary<string, string> BindableDictionary
        {
            get
            {
                return _bindableDictionary;
            }
        }

        protected string GetHybridKey(T1 Object)
        {
            PropertyInfo PI = Object.GetType().GetProperty(_hybridKeyProperty);
            return PI.GetValue(Object, null) as string;
        }

        protected string GetHybridData(T1 Object)
        {
            PropertyInfo PI = Object.GetType().GetProperty(_hybridDataProperty);
            return PI.GetValue(Object, null) as string;
        }

        #endregion Protected members
    }
}