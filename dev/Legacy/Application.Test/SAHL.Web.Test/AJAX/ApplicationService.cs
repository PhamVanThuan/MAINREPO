using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Web.AJAX;
using System.Data;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Test.AJAX
{
    [TestFixture]
    public class ApplicationService : TestViewBase
    {

        private Application _appService;


        [SetUp]
        public void Setup()
        {
            _appService = new Application();
        }

        [TearDown]
        public void TearDown()
        {
            _appService = null;
        }


        [Test]
        public void GetApplicationKeys()
        {
            string sql = "select top 1 OfferKey from [2am]..Offer o where LEN(o.OfferKey) > 4";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data to test");
            string prefix = dt.Rows[0]["OfferKey"].ToString().Substring(0, 4);
            dt.Dispose();

            SAHLAutoCompleteItem[] keys = _appService.GetApplicationKeys(prefix);
            Assert.LessOrEqual(keys.Length, 10);
            foreach (SAHLAutoCompleteItem item in keys)
            {
                if (!item.Value.ToString().StartsWith(prefix))
                    Assert.Fail("{0} does not start with expected prefix {1}", item.Value, prefix);
            }


        }
    }
}
