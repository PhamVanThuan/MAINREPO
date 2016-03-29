using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using BaseTest;

namespace Workers.UnitTest
{
    public class LoadApplicationCaptureTest : BaseTest.BaseTest
    {

        public override void Start(int NumberToSimulate)
        {
            Workers.Clear();
            // Go get an offerkey
            StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("select top {0} o.offerkey, o.offerstatuskey ", NumberToSimulate);
            //sb.AppendFormat("from [2am]..offer o join x2data.application_Capture data on o.offerkey=data.applicationkey ");
            //sb.AppendFormat("join x2.instance i on data.instanceid=i.id ");
            //sb.AppendFormat("join x2.state s on i.stateid=s.id ");
            //sb.AppendFormat("where s.name='Application Capture' ");
            sb.AppendFormat("select top {0} * from [2am]..offer o where o.offertypekey=7", NumberToSimulate);
            DataSet ds = DBMan.ExecuteSQL(sb.ToString());
            for (int i = 0; i < NumberToSimulate; i++)
            {
                int OfferKey = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                IBaseWorker w = new ApplicationCaptureWorker();
                
                w.SetSleepTimeRange(200, 400);
                //w.SetSleepTimeRange(2, 4);
                w.SetNumberIterations(-1);

                w.Setup(-1, OfferKey, "ApplicationKey", "Application Capture", "Origination", "sahl\\bcuser1", null);
                if (w.Start())
                {
                    Workers.Add(w);
                    Thread.Sleep(new Random().Next(1500,2500));
                }
            }
        }
    }
}
