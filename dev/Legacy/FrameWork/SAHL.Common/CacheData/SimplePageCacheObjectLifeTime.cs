using System.Collections.Generic;

namespace SAHL.Common.CacheData
{
    public class SimplePageCacheObjectLifeTime : ICacheObjectLifeTime
    {
        List<string> _validPages;

        public SimplePageCacheObjectLifeTime(List<string> ValidPages)
        {
            _validPages = new List<string>(ValidPages);
        }

        #region ICacheObjectLifeTime Members

        public bool ProcessRaisedLifeTimeEvent(ICacheObjectLifeTimeEventArgs EventData)
        {
            foreach (string ValidPage in _validPages)
            {
                if (ValidPage == EventData.CurrentViewName)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion ICacheObjectLifeTime Members
    }
}