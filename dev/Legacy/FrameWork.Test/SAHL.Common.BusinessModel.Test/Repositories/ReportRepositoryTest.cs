using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Test;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class ReportRepositoryTest : TestBase
    {
        private static IReportRepository _rRepo = RepositoryFactory.GetRepository<IReportRepository>();

        [SetUp]
        public void Setup()
        {
            // set the strategy to default so we actually go to the database
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            MockCache.Flush();
        }

        [Test]
        public void GetReportStatementByKey()
        {
            int iReportStatementKey = 86;

            IReportStatement _reportStatement = _rRepo.GetReportStatementByKey(iReportStatementKey);

            Assert.IsTrue(_reportStatement.Key == 86);
            Assert.IsTrue(_reportStatement.ReportName == "Letter Of Acceptance");
        }

        [Test]
        public void GetReportStatementByName()
        {
            string sReportName = "Adhoc - Email";

            IEventList<IReportStatement> _reportStatements = _rRepo.GetReportStatementByName(sReportName);

            Assert.IsTrue(_reportStatements.Count > 0);
            Assert.IsTrue(_reportStatements[0].ReportName == sReportName);
        }

        [Test]
        public void GetReportStatementByNameAndOSP()
        {
            string sReportName = "Letter Of Acceptance";
            int iOSPKey = 4;

            IReportStatement _reportStatement = _rRepo.GetReportStatementByNameAndOSP(sReportName, iOSPKey);

            Assert.IsTrue(_reportStatement.Key == 86);
            Assert.IsTrue(_reportStatement.ReportName == "Letter Of Acceptance");
        }

        [Test]
        public void CreateReportParameterMock()
        {
            IReportRepository repo = _mockery.StrictMock<IReportRepository>();
            IReportParameter irp = _mockery.StrictMock<IReportParameter>();
            Expect.Call(repo.CreateReportParameter()).Return(irp).IgnoreArguments();
            _mockery.ReplayAll();
            irp = repo.CreateReportParameter();
            Assert.IsNotNull(irp);
        }

        [Test]
        public void GetReportParameterByKeyMock()
        {
            IReportRepository repo = _mockery.StrictMock<IReportRepository>();
            IReportParameter irp = _mockery.StrictMock<IReportParameter>();
            Expect.Call(repo.GetReportParameterByKey(1)).Return(irp).IgnoreArguments();
            _mockery.ReplayAll();
            irp = repo.GetReportParameterByKey(1);
            Assert.IsNotNull(irp);
        }

        [Test]
        public void GetUIStatementTextMock()
        {
            IReportRepository repo = _mockery.StrictMock<IReportRepository>();
            IReportStatement irs = _mockery.StrictMock<IReportStatement>();
            string statement = "GetLoanAdjustmentWorkFlow";
            Expect.Call(repo.GetUIStatementText(irs)).Return(statement).IgnoreArguments();
            _mockery.ReplayAll();
            statement = repo.GetUIStatementText(irs);
            Assert.IsNotNull(statement);
        }

        [Test]
        public void GetReportParametersByReportStatementKeyMock()
        {
            IReadOnlyEventList<IReportParameter> reportlist = _rRepo.GetReportParametersByReportStatementKey(1);

            Assert.IsNotNull(reportlist);
        }

        [Test]
        public void ExportDataReportToExcelMock()
        {
            IReportRepository repo = _mockery.StrictMock<IReportRepository>();
            DataTable irp = _mockery.StrictMock<DataTable>();
            IReportStatement irstatement = _mockery.StrictMock<IReportStatement>();
            string datareport = "";
            Expect.Call(repo.ExportDataReportToExcel(irp, irstatement)).Return(datareport).IgnoreArguments();
            datareport = repo.ExportDataReportToExcel(irp, irstatement);
        }

        [Test]
        public void RenderSQLReportTest()
        {
            IDictionary<string, string> sqlReportParameters = new Dictionary<string, string>();
            string renderedMessage;
            sqlReportParameters.Add("DateFrom", "01/Jan/2009");
            sqlReportParameters.Add("DateTo", "01/Jan/2009");
            byte[] irp = _rRepo.RenderSQLReport("/Origination/Applications/Broker Performance", sqlReportParameters, out renderedMessage);
            Assert.IsTrue(renderedMessage.Length == 0, renderedMessage);
        }

        [Test]
        public void GetUIStatementText()
        {
            using (new SessionScope())
            {
                //hardcoded keys are generally frowned up, but these are managed
                IReportStatement rs = _rRepo.GetReportStatementByKey(9);
                string str = _rRepo.GetUIStatementText(rs);

                Assert.IsNotNull(str);

                foreach (IReportParameter rp in rs.ReportParameters)
                {
                    IReportParameter nrp = _rRepo.GetReportParameterByKey(rp.Key);
                    Assert.AreEqual(rp, nrp);
                }
            }
        }

        [Test]
        public void GetReportStatementByReportGroupKey()
        {
            using (new SessionScope())
            {
                IReadOnlyEventList<IReportStatement> rsList = _rRepo.GetReportStatementByReportGroupKey(5);
                Assert.Greater(rsList.Count, 0);
            }
        }

        [Test]
        public void ExecuteSQLReport()
        {
            //hardcoded values to simplify the test. The hardcoded keys are managed for reports
            using (new SessionScope())
            {
                IReadOnlyEventList<IReportParameter> paramList = _rRepo.GetReportParametersByReportStatementKey(162);

                Dictionary<SAHL.Common.BusinessModel.Interfaces.IReportParameter, object> _params = new Dictionary<IReportParameter, object>();
                foreach (IReportParameter param in paramList)
                {
                    _params.Add(param, "01/01/2009");
                }

                IReportStatement _reportStatement = _rRepo.GetReportStatementByKey(162);

                _rRepo.ExecuteSqlReport(_params, _reportStatement);
            }
        }
    }
}