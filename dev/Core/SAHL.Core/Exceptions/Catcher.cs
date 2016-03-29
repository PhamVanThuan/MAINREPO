using System;

namespace SAHL.Core.Exceptions
{
    public class Catcher : ICatcher
    {
        private static ICatcher instance;
        private static readonly object lockObject = new object();

        public static ICatcher Catch
        {
            get
            {
                lock (lockObject)
                {
                    if(instance == null)
                    {
                        instance = new Catcher();
                    }
                    return instance;
                }
            }
            set { instance = value; }
        }

        public void Silently(Action actionToSilentlyCatch)
        {
            try
            {
                actionToSilentlyCatch();
            }
            catch (Exception) 
            { 
                // Do nothing 
            }
        }
    }
}
