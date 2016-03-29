using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SAHL.Common.X2.BusinessModel.Interfaces
{
  public interface IDAOObject<T, T1>
  {
    T GetDAOObject();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T">DAO Object Type.</typeparam>
  /// <typeparam name="T1">Interfaced DAOObject Type.</typeparam>
  /// <typeparam name="T2">Concrete class implementing the interface.</typeparam>
  public class DAOIList<T, T1, T2> : IList<T1>, IEnumerable<T1>, IEnumerable
  {
    private class InternalDAOList<T, T1, T2> : IEnumerator<T1>, IEnumerator
    {
      IList<T> _List;
      T1 _current;
      int Pos = 0;
      public InternalDAOList(IList<T> lst)
      {
        this._List = lst;
      }

      #region IEnumerator<T1> Members

      public T1 Current
      {
        get
        {
          return _current;
        }
      }

      #endregion

      #region IDisposable Members

      public void Dispose()
      {
        _List = null;
      }

      #endregion

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
          T1 newobj = (T1)Activator.CreateInstance(typeof(T2), _List[Pos]);
          if (newobj == null)
            throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
          else
            _current = newobj;

          Pos++;
          return true;
        }
        Pos = _List.Count + 1;
        _current = default(T1);
        return false;
      }

      public void Reset()
      {
        Pos = -1;
      }

      #endregion
    }

    IList<T> _DAOObjectList;
    int _enumeratorIndex = -1;

    public DAOIList(IList<T> DAOList)
    {
      this._DAOObjectList = (IList<T>)DAOList;
    }

    private T GetDAOFromItem(T1 Item)
    {
      IDAOObject<T, T1> iobj = Item as IDAOObject<T, T1>;
      if (iobj != null)
      {
        T obj = iobj.GetDAOObject();
        return obj;
      }
      return default(T);
    }

    public IList<T> GetDAOList()
    {
      return _DAOObjectList;
    }


    #region IList<T1> Members

    public int IndexOf(T1 item)
    {
      T obj = GetDAOFromItem(item);
      if (obj != null)
        return _DAOObjectList.IndexOf(obj);
      return -1;
    }

    public void Insert(int index, T1 item)
    {
      T obj = GetDAOFromItem(item);
      if (obj != null)
        _DAOObjectList.Insert(index, obj);
      else
        throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
    }

    public void RemoveAt(int index)
    {
      _DAOObjectList.RemoveAt(index);
    }

    public T1 this[int index]
    {
      get
      {
        T1 newobj = (T1)Activator.CreateInstance(typeof(T2), _DAOObjectList[index]);
        if (newobj != null)
          return newobj;
        else
          throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
      }
      set
      {
        T obj = GetDAOFromItem(value);
        _DAOObjectList[index] = obj;
      }
    }

    #endregion

    #region ICollection<T1> Members

    public void Add(T1 item)
    {
      T obj = GetDAOFromItem(item);
      _DAOObjectList.Add(obj);
    }

    public void Clear()
    {
      _DAOObjectList.Clear();
    }

    public bool Contains(T1 item)
    {
      T obj = GetDAOFromItem(item);
      return _DAOObjectList.Contains(obj);
    }

    public void CopyTo(T1[] array, int arrayIndex)
    {
      for (int i = 0; i < _DAOObjectList.Count; i++)
      {
        T1 newobj = (T1)Activator.CreateInstance(typeof(T2), _DAOObjectList[i]);
        if (newobj == null)
          throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");

        array[arrayIndex + 1] = newobj;
      }
    }

    public int Count
    {
      get { return _DAOObjectList.Count; }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    public bool Remove(T1 item)
    {
      T obj = GetDAOFromItem(item);
      return _DAOObjectList.Remove(obj);
    }

    #endregion

    #region IEnumerable<T1> Members

    public IEnumerator<T1> GetEnumerator()
    {
      return new InternalDAOList<T, T1, T2>(_DAOObjectList);
    }

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator()
    {
      return new InternalDAOList<T, T1, T2>(_DAOObjectList);
    }

    #endregion
  }
}
