namespace SAHL.Common.CacheData
{
    public class CacheObjectLifeTimeEventArgs : ICacheObjectLifeTimeEventArgs
    {
        string _currentViewName;
        //string _navigateValue;
        //string _savedViewName;

        public CacheObjectLifeTimeEventArgs(string CurrentViewName)//, string NavigateValue, string SavedViewName)
        {
            _currentViewName = CurrentViewName;
            //_navigateValue = NavigateValue;
            //_savedViewName = SavedViewName;
        }

        #region ICacheObjectLifeTimeEventArgs Members

        public string CurrentViewName
        {
            get { return _currentViewName; }
        }

        //public string SavedViewName
        //{
        //    get { return _savedViewName; }
        //}

        //public string NavigateValue
        //{
        //    get { return _navigateValue; }
        //}

        #endregion ICacheObjectLifeTimeEventArgs Members
    }
}