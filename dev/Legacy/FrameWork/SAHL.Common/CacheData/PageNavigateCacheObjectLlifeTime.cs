//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace SAHL.Common.CacheData
//{
//    public class PageNavigateCacheObjectLifeTime : ICacheObjectLifeTime
//    {
//        IList<PageNavigateCacheItem> _items;

//        public PageNavigateCacheObjectLifeTime(IList<PageNavigateCacheItem> Items)
//        {
//            _items = Items;
//        }

//        #region ICacheObjectLifeTime Members

//        public bool ProcessRaisedLifeTimeEvent(ICacheObjectLifeTimeEventArgs EventData)
//        {
//            for (int i = 0; i < _items.Count; i++)
//            {
//                if (_items[i].ViewName == EventData.CurrentViewName)
//                {
//                    for (int j = 0; j < _items[i].NavigateValues.Length; j++)
//                    {
//                        if (_items[i].NavigateValues[j] == EventData.NavigateValue)
//                            return true;
//                    }
//                }
//            }
//                    return false;
//        }

//        #endregion
//    }

//    public class PageNavigateCacheItem
//    {
//        string _viewName;
//        string[] _navigateValues;

//        public PageNavigateCacheItem(string ViewName, string[] NavigateValues)
//        {
//            _viewName = ViewName;
//            _navigateValues = NavigateValues;
//        }

//        public string ViewName
//        {
//            get
//            {
//                return _viewName;
//            }
//        }

//        public string[] NavigateValues
//        {
//            get
//            {
//                return _navigateValues;
//            }
//        }

//    }
//}