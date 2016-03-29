using System;

namespace SAHL.X2Engine2.Specs.Mocks
{
    public class InstantFakeTimer : ITimer
    {
        public void Start<T>(int timeout, Action<T> callback, T instance)
        {
            callback(instance);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}