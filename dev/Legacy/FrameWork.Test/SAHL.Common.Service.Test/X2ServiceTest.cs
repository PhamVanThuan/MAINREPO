using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Test;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.TypeMapper;
using Rhino.Mocks;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Security;

namespace SAHL.Common.Service.Test
{
    [TestFixture]
    public class X2ServiceTest : TestBase
    {
       
        [Test]
        public void GetUserActivitiesForInstance()
        {
            string query = @"select top 1 I.ID, IAS.ADUserName, count(IAS.ID) 
                from [X2].[X2].Instance I (nolock)
                JOIN [X2].[X2].InstanceActivitySecurity IAS (nolock) on IAS.InstanceID = I.ID 
                JOIN [X2].[X2].Activity A (nolock) on A.ID = IAS.ActivityID 
                where A.[Type] = 1 
                group by I.ID, IAS.ADUserName 
                order by count(IAS.ID) desc";
            DataTable DT = base.GetQueryResults(query);

            Assert.That(DT.Rows.Count == 1);
            long instanceID = Convert.ToInt32(DT.Rows[0][0]);
            string role = Convert.ToString(DT.Rows[0][1]);

            List<string> roles = new List<string>();
            roles.Add(role);
            SAHLPrincipal principal = base.TestPrincipal;
            // principal.CachedRoles = roles;

            X2Service X2 = new X2Service();
            X2.GetUserActivitiesForInstance(principal, instanceID);
        }
    }
    
}
