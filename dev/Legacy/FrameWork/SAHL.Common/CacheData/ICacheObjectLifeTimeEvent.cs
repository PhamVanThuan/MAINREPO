namespace SAHL.Common.CacheData
{
    public interface ICacheObjectLifeTimeEventArgs
    {
        string CurrentViewName { get; }

        //string NavigateValue { get;}
        //string SavedViewName { get;}
    }
}