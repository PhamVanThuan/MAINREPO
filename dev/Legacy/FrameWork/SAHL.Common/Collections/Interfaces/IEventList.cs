using System;
using System.Collections;
using System.Collections.Generic;

namespace SAHL.Common.Collections.Interfaces
{
    public delegate void EventListHandler(ICancelDomainArgs args, object Item);

    public enum EventListAction : int
    {
        Add = 0,
        Remove = 1,
        Insert
    }

    public interface IEventList<T1> : IEnumerable<T1>, IEnumerable
    {
        event EventListHandler BeforeAdd;
        event EventListHandler BeforeRemove;
        event EventListHandler AfterAdd;
        event EventListHandler AfterRemove;

        bool Contains(T1 value);

        int IndexOf(T1 value);

        bool Add(IDomainMessageCollection Messages, T1 value);

        bool Insert(IDomainMessageCollection Messages, int index, T1 value);

        bool Remove(IDomainMessageCollection Messages, T1 value);

        bool RemoveAt(IDomainMessageCollection Messages, int index);

        T1 this[int index] { get; set; }

        int Count { get; }

        IDictionary<string, string> BindableDictionary { get; }

        IDictionary<string, T1> ObjectDictionary { get; }

        #region Sorting Methods

        /// <summary>
        /// Sorts the elements in the entire List using the default comparer.
        /// </summary>
        void Sort();

        /// <summary>
        /// Sorts the elements in the entire List using the specified System.Comparison.
        /// </summary>
        /// <param name="comparison">The System.Comparison to use when comparing elements.</param>
        void Sort(Comparison<T1> comparison);

        /// <summary>
        /// Sorts the elements in the entire List using the specified comparer.
        /// </summary>
        /// <param name="comparer">The IComparer implementation to use when comparing elements, or a null reference to use the default comparer Comparer.Default.</param>
        void Sort(IComparer<T1> comparer);

        /// <summary>
        /// Sorts the elements in a range of elements in List using the specified comparer.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">The IComparer implementation to use when comparing elements, or a null reference to use the default comparer Comparer.Default.</param>
        void Sort(int index, int count, IComparer<T1> comparer);

        #endregion Sorting Methods
    }

    public interface ICancelDomainArgs
    {
        EventListAction Action { get; }

        bool Cancel { get; set; }

        IDomainMessageCollection Messages { get; }
    }
}