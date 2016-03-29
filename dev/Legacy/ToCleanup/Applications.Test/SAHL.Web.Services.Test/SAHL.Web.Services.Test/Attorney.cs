using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Castle.ActiveRecord;
using System.Data;
using SAHL.Test;

namespace SAHL.Web.Services.Test
{
	[TestFixture]
	public class Attorney : TestBase
	{
		
		public void ForgottenPasswordFail()
		{
			AttorneyService.AttorneyClient serviceClient = new AttorneyService.AttorneyClient();
			bool result = serviceClient.ForgottenPassword("test");
			Assert.AreEqual(result , false);			
		}

		public void ForgottenPasswordPass()
		{

			//using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
			//{
			//    string query = "select * from [2AM].[dbo].[Valuation] (nolock) where PropertyKey = 82608";
			//    DataTable DT = base.GetQueryResults(query);
			//    Assert.That(1 == DT.Rows.Count);

			//    AttorneyService.AttorneyClient serviceClient = new AttorneyService.AttorneyClient();
			//    bool result = serviceClient.ForgottenPassword("test");
			//    Assert.AreEqual(result, false);		


			//}
		}


	}
}
