using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Rules.Correspondence;

namespace SAHL.Common.BusinessModel.Rules.Test.Correspondence
{
    [TestFixture]
    public class Correspondence : RuleBase
    {

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Correspondence.CorrespondenceAlreadySent"/> rule.
        /// </summary>
        [Test]
        public void CorrespondenceAlreadySent_Failure()
        {
            CorrespondenceAlreadySent rule = new CorrespondenceAlreadySent();

            // get the first correspondence record
            Correspondence_DAO dao = Correspondence_DAO.FindFirst();
            int reportStatementKey = dao.ReportStatement.Key;
            int genericKey = dao.GenericKey;
            int genericKeyTypeKey = dao.GenericKeyType.Key;

            // Setup the parameter objects to pass along
            List<int> reportStatementKeys = new List<int>();
            reportStatementKeys.Add(reportStatementKey);

            // expect 1 messages to be returned
            ExecuteRule(rule, 1, reportStatementKeys, genericKey, genericKeyTypeKey);
        }

        [Test]
        public void CorrespondenceAlreadySent_Success()
        {
            CorrespondenceAlreadySent rule = new CorrespondenceAlreadySent();

            // get the first correspondence record
            Correspondence_DAO dao = Correspondence_DAO.FindFirst();
            int reportStatementKey = dao.ReportStatement.Key;
            int genericKey = 999999999;
            int genericKeyTypeKey = dao.GenericKeyType.Key;

            // Setup the parameter objects to pass along
            List<int> reportStatementKeys = new List<int>();

            reportStatementKeys.Add(reportStatementKey);

            // expect 0 messages to be returned
            ExecuteRule(rule, 0, reportStatementKeys, genericKey, genericKeyTypeKey);
        }
    }
}
