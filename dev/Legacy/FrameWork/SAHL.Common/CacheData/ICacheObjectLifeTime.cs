namespace SAHL.Common.CacheData
{
    public interface ICacheObjectLifeTime
    {
        /// <summary>
        /// Called by the framework on each page unload.
        /// </summary>
        /// <param name="EventData"></param>
        /// <returns>True if the cache should be cleared, otherwise false.</returns>
        bool ProcessRaisedLifeTimeEvent(ICacheObjectLifeTimeEventArgs EventData);
    }
}