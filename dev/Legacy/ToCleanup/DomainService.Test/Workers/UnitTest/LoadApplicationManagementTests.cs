using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BaseTest;
using System.Threading;

namespace Workers.UnitTest
{
    public class LoadApplicationManagementTests: BaseTest.BaseTest
    {
        public override void Start(int NumberToSimulate)
        {
            Workers.Clear();
            // Go get an offerkey
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select top {0} ", NumberToSimulate);
            sb.AppendFormat("d.applicationkey, d.InstanceID ");
            sb.AppendFormat("from x2data.application_management d ");
            sb.AppendFormat("join x2.instance i on d.instanceid=i.id ");
            sb.AppendFormat("join x2.state s on i.stateid=s.id ");
            sb.AppendFormat("where s.name='Awaiting Application' order by i.id desc");
            DataSet ds = DBMan.ExecuteSQL(sb.ToString());
            for (int i = 0; i < NumberToSimulate; i++)
            {
                int OfferKey = Convert.ToInt32(ds.Tables[0].Rows[i][0]);
                Int64 IID = Convert.ToInt64(ds.Tables[0].Rows[i][1]);
                AppMan w = new AppMan();
                
                w.SetSleepTimeRange(20, 40);
                //w.SetSleepTimeRange(2, 4);
                w.SetNumberIterations(-1);

                w.Setup(IID, OfferKey, "ApplicationKey", "Application Management", "Origination", "sahl\\NBPUser1", null);
                if (w.Start())
                {
                    Workers.Add(w);
                    Thread.Sleep(new Random().Next(1500,2500));
                }
            }
        }
    }
}
