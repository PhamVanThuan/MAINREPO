using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.Web.UI.Events
{
  /// <summary>
  /// Method delegate that will be fired when a row is clicked on a datagrid. Passes the index of the object selected
  /// </summary>
  /// <param name="sender">a datagrid</param>
  /// <param name="index">index of item that was selected (clicked)</param>
  public delegate void GridClickdEventHandler(Object sender, object index);

  /// <summary>
  /// Event argument class for the <see cref="GridClickdEventHandler"/> delegate.
  /// </summary>
  public class GridRowSelectEventArgs : EventArgs 
  {
    private object _obj;
    public GridRowSelectEventArgs(object Index)
    {
      _obj = Index;
    }

    public object Index
    {
      get { return _obj; }
    }

  }
}
