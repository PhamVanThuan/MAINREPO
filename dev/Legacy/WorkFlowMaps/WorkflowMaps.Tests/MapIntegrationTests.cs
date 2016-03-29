using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Config;
using SAHL.Common.BusinessModel.Configuration;
using SAHL.Common.Collections;
using SAHL.X2.Framework.Common;
using SAHL.Common.Collections.Interfaces;
using System.Diagnostics;
using Castle.ActiveRecord;
using Life;

namespace WorkflowMaps.Tests
{
	[TestFixture]
	public class MapIntegrationTests
	{
        [Test]
        public void Test()
        {
            ////Sample Code
            //var map = new X2Debt_Counselling();
            //var data = new X2Debt_Counselling_Data();

            //SAHL.X2.Framework.DataSets.dsX2InstanceData ds = new SAHL.X2.Framework.DataSets.dsX2InstanceData();
            //var row = ds.X2InstanceData.NewX2InstanceDataRow();
            //row.CreationDate = DateTime.Now;
            //row.CreatorADUserName = "sahl\\bob";
            //row.Name = "";
            //row.WorkFlowName = "Debt Counselling";
            //row.WorkFlowProvider = "";
            //row.ID = 5631923;
            //ds.X2InstanceData.AddX2InstanceDataRow(row);

            //X2InstanceData instanceData = new X2InstanceData(ds.X2InstanceData);

            //data.DebtCounsellingKey = 7467;
            //data.AccountKey = 2632603;
            //data.AssignADUserName = "sahl\\dcccuser1";
            //data.AssignWorkflowRoleTypeKey = 5;
            //string message = String.Empty;
            //IDomainMessageCollection messages = new DomainMessageCollection();
            //try
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        long endticks;
            //        long startticks = DateTime.Now.Ticks;
            //        map.OnStartActivity_Send_Proposal_for_Approval(messages, data, instanceData, new SAHL.X2.Framework.Common.X2Params(String.Empty, String.Empty, String.Empty, false, new object()));
            //        endticks = DateTime.Now.Ticks;
            //        Debug.WriteLine(string.Format("Onstart took:{0}ms", new TimeSpan(endticks - startticks).Milliseconds));

            //        startticks = DateTime.Now.Ticks;
            //        map.OnCompleteActivity_Send_Proposal_for_Approval(messages, data, instanceData, new SAHL.X2.Framework.Common.X2Params(String.Empty, String.Empty, String.Empty, false, new object()), ref message);
            //        endticks = DateTime.Now.Ticks;
            //        Debug.WriteLine(string.Format("OnComplete took:{0}ms", new TimeSpan(endticks - startticks).Milliseconds));
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }

        [Test]
        public void LifeOriginationTest()
        {
            var map = new X2LifeOrigination();
            var data = new X2LifeOrigination_Data();

            SAHL.X2.Framework.DataSets.dsX2InstanceData ds = new SAHL.X2.Framework.DataSets.dsX2InstanceData();
            var row = ds.X2InstanceData.NewX2InstanceDataRow();
            row.CreationDate = DateTime.Now;
            row.CreatorADUserName = "sahl\\lcuser";
            row.Name = "";
            row.WorkFlowName = "LifeOrigination";
            row.WorkFlowProvider = "";
            row.ID = 5698203;
            ds.X2InstanceData.AddX2InstanceDataRow(row);


            X2InstanceData instanceData = new X2InstanceData(ds.X2InstanceData);
            data.OfferKey = 1311443;

            string message = String.Empty;
            IDomainMessageCollection messages = new DomainMessageCollection();
            try
            {
                //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                //{

                    map.OnCompleteActivity_Accept_Benefits(messages, data, instanceData, new SAHL.X2.Framework.Common.X2Params(String.Empty, String.Empty, String.Empty, false, new object()), ref message);
                //}
            }
            catch (Exception ex)
            {
            }
        }
	}
}
