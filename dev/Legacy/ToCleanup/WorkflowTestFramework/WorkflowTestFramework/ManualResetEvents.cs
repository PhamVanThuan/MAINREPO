using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WorkflowTestFramework
{
    public class ManualResetEvents
    {
        //Constructor
        public ManualResetEvents(int InstanceCount)
        {
            //number of arrays to create in the list, 64 items per array
            double d = InstanceCount / arrSize;
            mreLcount = Convert.ToInt32(d);

            //Get the size of the last array
            lastmrecount = Convert.ToInt32(InstanceCount - (d * arrSize));
            
            //0 bound array, need to minus one
            if (lastmrecount == 0)
                mreLcount -= 1;

            //create the list of required arrays
            for (int i = 0; i <= mreLcount; i++)
            {
                if (i == mreLcount && lastmrecount != 0)
                    arrSize = lastmrecount;

                MREventList.Add(i, new ManualResetEvent[arrSize]);
            }

            //reset this as the start array
            mreLcount = 0;
        }
        
        public ManualResetEvent GetNextResetEvent()
        {
            try
            {
                lock (MREventList)
                {
                    ManualResetEvent mre = new ManualResetEvent(false);
                    ManualResetEvent[] mreArr;

                    if (MREventList.ContainsKey(mreLcount))
                    {
                        MREventList.TryGetValue(mreLcount, out mreArr);

                        if (mreArr != null)
                        {
                            mreArr[mrecount] = mre;

                            mrecount += 1;
                            if (mrecount >= 64)//if the array is full, set the list to use a new array next time
                            {
                                mrecount = 0;
                                mreLcount += 1;
                            }
                        }
                    }
                    return mre;
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private Dictionary<int, ManualResetEvent[]> _mrEventList;

        public Dictionary<int, ManualResetEvent[]> MREventList
        {
            get 
            {
                if (_mrEventList == null)
                    _mrEventList = new Dictionary<int, ManualResetEvent[]>();

                return _mrEventList; 
            }
        }

        private int mrecount = 0;
        private int lastmrecount = 0;
        private int mreLcount = 0;
        private int arrSize = 64;
    }
}
