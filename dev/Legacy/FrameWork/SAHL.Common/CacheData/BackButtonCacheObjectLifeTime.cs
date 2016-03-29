namespace SAHL.Common.CacheData
{
    public class BackButtonCacheObjectLifeTime : ICacheObjectLifeTime
    {
        #region ICacheObjectLifeTime Members

        public bool ProcessRaisedLifeTimeEvent(ICacheObjectLifeTimeEventArgs EventData)
        {
            //if (EventData.SavedViewName != EventData.CurrentViewName)
            //    return true;
            return false;
        }

        #endregion ICacheObjectLifeTime Members
    }
}