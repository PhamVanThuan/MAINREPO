using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Test;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class AdCheckTest : TestBase
    {
        [Test]
        public void TestCreateXmlString()
        {
            using (new SessionScope())
            {
                DataSet request = new DataSet("request");
                DataSet response = new DataSet("response");
                DataTable dt1 = new DataTable("one");
                DataTable dt2 = new DataTable("two");
                DataColumn col1 = new DataColumn("col1", typeof(int));
                DataColumn col2 = new DataColumn("col2", typeof(string));
                dt1.Columns.Add(col1);
                dt1.Columns.Add(col2);
                DataRow row = dt1.NewRow();
                row.ItemArray = new object[] { 23, null };
                dt1.Rows.Add(row);
                request.Tables.Add(dt1);
                response.Tables.Add(dt2);
                string xml = SAHL.Common.Service.XMLHistory.CreateXmlString(request, response, "providerX", "methodX");
                Assert.That(string.IsNullOrEmpty(xml) != true);
            }
        }
    }
}