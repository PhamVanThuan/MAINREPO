using System;
using System.Collections.Generic;
using System.Text;

namespace BaseTest
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseTest : IBaseTest 
    {
        // Does nothing ATM.
        protected List<IBaseWorker> Workers = new List<IBaseWorker>();
        public abstract void Start(int NumberToSimulate);
        public void Stop()
        {
            foreach (IBaseWorker w in Workers)
            {
                w.Stop();
            }
        }
    }
}
