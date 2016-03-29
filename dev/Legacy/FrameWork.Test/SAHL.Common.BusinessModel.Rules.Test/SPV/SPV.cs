using NUnit.Framework;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.Rules.Test.SPV
{
    [TestFixture]
    public class SPV : RuleBase
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

        
        [Test, TestCaseSource("GetSPVAttributes")]
        public void TestGetValidSPV_LTV50(spvTest spv)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            IRow spvDetermineParameters = new Row();

            spvDetermineParameters.LTV = 0.50M;
            spvDetermineParameters.FLAllowed = 0;
            spvDetermineParameters.TermChangeAllowed = 0;
            spvDetermineParameters.HasBeenInCompany2 = (spv.SPVCompanyKey == 2 ? 1 : 0);
            using (new SessionScope())
            {
                ISPV spvNew = spvService.GetSPVByParameters(spvDetermineParameters, -1, Globals.SPVDetermineSources.Params, spv.SPVKey);

                Assert.That(spv.SPVCompanyKey == spvNew.SPVCompany.Key, String.Format(@"LTV = {0}, FLAllowed = {1}, TermChangeAllowed = {2}, ExistingSPV = {3}, NewSPV = {4}"
                    , spvDetermineParameters.LTV.ToString()
                    , spv.AllowFurtherLending.ToString()
                    , spv.AllowTermChange.ToString()
                    , spv.SPVKey.ToString()
                    , spvNew.Key.ToString()));
            }
        }

        
        [Test, TestCaseSource("GetSPVAttributes")]
        public void TestGetValidSPV_LTV50_Offer(spvTest spv)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            IRow spvDetermineParameters = new Row();

            spvDetermineParameters.LTV = 0.50M;
            spvDetermineParameters.FLAllowed = 0;
            spvDetermineParameters.TermChangeAllowed = 0;
            spvDetermineParameters.HasBeenInCompany2 = 0;
            using (new SessionScope())
            {
                ISPV spvNew = spvService.GetSPVByParameters(spvDetermineParameters, -1, Globals.SPVDetermineSources.Params, -1);

                Assert.That(spvNew.Key == 117, String.Format(@"LTV = {0}, FLAllowed = {1}, TermChangeAllowed = {2}, ExistingSPV = {3}, NewSPV = {4}"
                    , spvDetermineParameters.LTV.ToString()
                    , spv.AllowFurtherLending.ToString()
                    , spv.AllowTermChange.ToString()
                    , spv.SPVKey.ToString()
                    , spvNew.Key.ToString()));

            }
        }

        
        [Test, TestCaseSource("GetSPVAttributes")]
        public void TestGetValidSPV_LTV50_FurtherLending(spvTest spv)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            IRow spvDetermineParameters = new Row();

            spvDetermineParameters.LTV = 0.50M;
            spvDetermineParameters.FLAllowed = 1;
            spvDetermineParameters.TermChangeAllowed = 0;
            spvDetermineParameters.HasBeenInCompany2 = spv.SPVCompanyKey == 2 ? 1 : 0;

            using (new SessionScope())
            {
                ISPV spvNew = spvService.GetSPVByParameters(spvDetermineParameters, -1, Globals.SPVDetermineSources.Params, spv.SPVKey);

                Assert.That(spvNew.SPVCompany.Key == spv.SPVCompanyKey, String.Format(@"SPV Company should not have changed: {0} to {1}", spv.SPVKey, spvNew.Key));

                if (spv.AllowFurtherLending == 1)
                    Assert.That(spv.SPVKey == spvNew.Key, String.Format(@"SPV should not have changed: {0} to {1}", spv.SPVKey, spvNew.Key));

                if (spvDetermineParameters.HasBeenInCompany2 == 1 && spv.AllowFurtherLending != 1)
                {
                    Assert.That(spvNew.Key == 116, String.Format(@"SPV does not allow FL, been in BB: {0}", spv.SPVKey));
                }
                if (spvDetermineParameters.HasBeenInCompany2 == 0 && spv.AllowFurtherLending != 1)
                {
                    Assert.That(spvNew.Key == 117, String.Format(@"SPV does not allow FL, Not been in BB: {0}", spv.SPVKey));
                }
            }
        }

        
        [Test, TestCaseSource("GetSPVAttributes")]
        public void TestGetValidSPV_LTV81_Offer(spvTest spv)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            IRow spvDetermineParameters = new Row();

            spvDetermineParameters.LTV = 0.81M;
            spvDetermineParameters.FLAllowed = 0;
            spvDetermineParameters.TermChangeAllowed = 0;
            spvDetermineParameters.HasBeenInCompany2 = 0;
            using (new SessionScope())
            {
                ISPV spvNew = spvService.GetSPVByParameters(spvDetermineParameters, -1, Globals.SPVDetermineSources.Params, -1);

                Assert.That(spvNew.Key == 116, String.Format(@"LTV = {0}, FLAllowed = {1}, TermChangeAllowed = {2}, ExistingSPV = {3}, NewSPV = {4}"
                    , spvDetermineParameters.LTV.ToString()
                    , spv.AllowFurtherLending.ToString()
                    , spv.AllowTermChange.ToString()
                    , spv.SPVKey.ToString()
                    , spvNew.Key.ToString()));
            }
        }

        
        [Test, TestCaseSource("GetSPVAttributes")]
        public void TestGetValidSPV_LTV81_FurtherLending(spvTest spv)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            IRow spvDetermineParameters = new Row();

            spvDetermineParameters.LTV = 0.81M;
            spvDetermineParameters.FLAllowed = 1;
            spvDetermineParameters.TermChangeAllowed = 0;
            spvDetermineParameters.HasBeenInCompany2 = spv.SPVCompanyKey == 2 ? 1 : 0;

            using (new SessionScope())
            {
                ISPV spvNew = spvService.GetSPVByParameters(spvDetermineParameters, -1, Globals.SPVDetermineSources.Params, spv.SPVKey);

                Assert.That(spvNew.SPVCompany.Key == 2, String.Format(@"Everything over 81% must be BB: spvKey = {0}", spvNew.Key));

                if (spvDetermineParameters.HasBeenInCompany2 == 1 && spv.AllowFurtherLending == 1)
                    Assert.That(spv.SPVKey == spvNew.Key, String.Format(@"SPV should not have changed: {0} to {1}", spv.SPVKey, spvNew.Key));

                if (spvDetermineParameters.HasBeenInCompany2 == 0)
                    Assert.That(spvNew.Key == 116, String.Format(@"SPV can only move to BB: {0}", spv.SPVKey));
            }
        }

        
        [Test, TestCaseSource("GetSPVAttributes")]
        public void TestGetValidSPV_LTV50_TermChange(spvTest spv)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            IRow spvDetermineParameters = new Row();

            spvDetermineParameters.LTV = 0.50M;
            spvDetermineParameters.FLAllowed = 0;
            spvDetermineParameters.TermChangeAllowed = 1;
            spvDetermineParameters.HasBeenInCompany2 = spv.SPVCompanyKey == 2 ? 1 : 0;

            using (new SessionScope())
            {
                ISPV spvNew = spvService.GetSPVByParameters(spvDetermineParameters, -1, Globals.SPVDetermineSources.Params, spv.SPVKey);

                Assert.That(spvNew.SPVCompany.Key == spv.SPVCompanyKey, String.Format(@"SPV Company should not have changed: {0} to {1}", spv.SPVKey, spvNew.Key));

                if (spv.AllowFurtherLending == 1)
                    Assert.That(spv.SPVKey == spvNew.Key, String.Format(@"SPV should not have changed: {0} to {1}", spv.SPVKey, spvNew.Key));

                if (spvDetermineParameters.HasBeenInCompany2 == 1 && spv.AllowTermChange != 1)
                {
                    Assert.That(spvNew.Key == 116, String.Format(@"SPV does not allow TC, been in BB: {0}", spv.SPVKey));
                }
                if (spvDetermineParameters.HasBeenInCompany2 == 0 && spv.AllowTermChange != 1)
                {
                    Assert.That(spvNew.Key == 117, String.Format(@"SPV does not allow TC, Not been in BB: {0}", spv.SPVKey));
                }
            }
        }

        
        [Test, TestCaseSource("GetSPVAttributes")]
        public void TestGetValidSPV_LTV81_TermChange(spvTest spv)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            IRow spvDetermineParameters = new Row();

            spvDetermineParameters.LTV = 0.81M;
            spvDetermineParameters.FLAllowed = 0;
            spvDetermineParameters.TermChangeAllowed = 1;
            spvDetermineParameters.HasBeenInCompany2 = spv.SPVCompanyKey == 2 ? 1 : 0;

            using (new SessionScope())
            {
                ISPV spvNew = spvService.GetSPVByParameters(spvDetermineParameters, -1, Globals.SPVDetermineSources.Params, spv.SPVKey);

                Assert.That(spvNew.SPVCompany.Key == 2, String.Format(@"Everything over 81% must be BB: spvKey = {0}", spvNew.Key));

                if (spvDetermineParameters.HasBeenInCompany2 == 1 && spv.AllowTermChange == 1)
                    Assert.That(spv.SPVKey == spvNew.Key, String.Format(@"SPV should not have changed: {0} to {1}", spv.SPVKey, spvNew.Key));

                if (spvDetermineParameters.HasBeenInCompany2 == 0)
                    Assert.That(spvNew.Key == 116, String.Format(@"SPV can only move to BB: {0}", spv.SPVKey));
            }
        }

        #region Capitec SPV tests

        [Test, TestCaseSource("GetSPVAttributes")]
        public void TestGetValidSPV_Capitec_Alpha_Offer(spvTest spv)
        {
            ISPVService spvService = ServiceFactory.GetService<ISPVService>();
            IRow spvDetermineParameters = new Row();

            spvDetermineParameters.LTV = 0.91M;
            spvDetermineParameters.FLAllowed = 0;
            spvDetermineParameters.TermChangeAllowed = 0;
            spvDetermineParameters.HasBeenInCompany2 = 0;
            spvDetermineParameters.OfferAttributes = "26,30";
            using (new SessionScope())
            {
                ISPV spvNew = spvService.GetSPVByParameters(spvDetermineParameters, -1, Globals.SPVDetermineSources.Params, -1);

                Assert.That(spvNew.Key == 160, String.Format(@"LTV = {0}, FLAllowed = {1}, TermChangeAllowed = {2}, ExistingSPV = {3}, NewSPV = {4}"
                    , spvDetermineParameters.LTV.ToString()
                    , spv.AllowFurtherLending.ToString()
                    , spv.AllowTermChange.ToString()
                    , spv.SPVKey.ToString()
                    , spvNew.Key.ToString()));
            }
        }

        #endregion Capitec SPV tests

        public class spvTest
        {
            public spvTest(int spvKey, int spvCompanyKey, int parentSPVKey, int allowFurtherLending, int allowTermChange)
            {
                SPVKey = spvKey;
                SPVCompanyKey = spvCompanyKey;
                ParentSPVKey = parentSPVKey;
                AllowFurtherLending = allowFurtherLending;
                AllowTermChange = allowTermChange;
            }


            public int SPVKey { get; set; }
            public int SPVCompanyKey { get; set; }
            public int ParentSPVKey { get; set; }
            public int AllowFurtherLending { get; set; }
            public int AllowTermChange { get; set; }
        }

        static List<spvTest> GetSPVAttributes()
        {
            List<spvTest> lst = new List<spvTest>();

            lst.Add(new spvTest(116, 2, 16, 1, 1));
            lst.Add(new spvTest(117, 1, 17, 1, 1));
            lst.Add(new spvTest(122, 1, 24, 1, 1));
            lst.Add(new spvTest(123, 2, 25, 0, 1));
            lst.Add(new spvTest(124, 2, 26, 0, 1));
            lst.Add(new spvTest(126, 1, 28, 1, 1));
            lst.Add(new spvTest(127, 1, 29, 0, 1));
            lst.Add(new spvTest(128, 1, 30, 1, 1));
            lst.Add(new spvTest(129, 2, 32, 1, 1));
            lst.Add(new spvTest(130, 1, 33, 1, 1));
            lst.Add(new spvTest(132, 2, 35, 1, 1));
            lst.Add(new spvTest(134, 1, 31, 1, 1));
            lst.Add(new spvTest(135, 1, 37, 1, 1));
            lst.Add(new spvTest(136, 2, 32, 1, 1));
            lst.Add(new spvTest(137, 2, 32, 1, 1));
            lst.Add(new spvTest(138, 2, 32, 1, 1));
            lst.Add(new spvTest(139, 2, 32, 1, 1));
            lst.Add(new spvTest(140, 2, 32, 1, 1));
            lst.Add(new spvTest(141, 2, 32, 1, 1));
            lst.Add(new spvTest(142, 2, 32, 1, 1));
            lst.Add(new spvTest(143, 2, 32, 1, 1));
            lst.Add(new spvTest(144, 2, 32, 1, 1));
            lst.Add(new spvTest(145, 2, 32, 1, 1));
            lst.Add(new spvTest(146, 2, 32, 1, 1));
            lst.Add(new spvTest(147, 2, 32, 1, 1));
            lst.Add(new spvTest(148, 2, 32, 1, 1));
            lst.Add(new spvTest(149, 2, 32, 1, 1));
            lst.Add(new spvTest(150, 2, 32, 1, 1));
            lst.Add(new spvTest(151, 2, 32, 1, 1));
            lst.Add(new spvTest(152, 2, 32, 1, 1));
            lst.Add(new spvTest(153, 2, 32, 1, 1));
            lst.Add(new spvTest(154, 2, 32, 1, 1));
            lst.Add(new spvTest(155, 2, 32, 1, 1));

            return lst;
        }

    }
}