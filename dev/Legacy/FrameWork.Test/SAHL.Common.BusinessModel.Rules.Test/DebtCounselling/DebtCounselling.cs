using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.DebtCounselling;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel.Rules.Test.DebtCounselling
{
    [TestFixture]
    public class DebtCounselling : RuleBase
    {
        IRuleService _ruleService;
        DomainMessageCollection _messages;

        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void DebtCounsellingActiveCounterProposalExistsPassTest()
        {
            DebtCounsellingActiveCounterProposalExists rule = new DebtCounsellingActiveCounterProposalExists();

            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtcounselling.Key).Return(1);

            //Mock the Debt Counselling Repository
            IDebtCounsellingRepository debtRepoMock = _mockery.StrictMock<IDebtCounsellingRepository>();
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), debtRepoMock);

            List<IProposal> proposalList = _mockery.StrictMock<List<IProposal>>();
            IProposal proposal = _mockery.StrictMock<IProposal>();
            IProposalStatus ps = _mockery.StrictMock<IProposalStatus>();

            SetupResult.For(proposal.Key).Return(1);
            SetupResult.For(ps.Key).Return((int)ProposalStatuses.Active);
            SetupResult.For(proposal.ProposalStatus).Return(ps);

            proposalList.Add(proposal);

            SetupResult.For(debtRepoMock.GetProposalsByType(1, ProposalTypes.CounterProposal)).IgnoreArguments().Return(proposalList);

            int expectedMessageCount = 0;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        [Test]
        public void DebtCounsellingActiveCounterProposalExistsFailTest()
        {
            DebtCounsellingActiveCounterProposalExists rule = new DebtCounsellingActiveCounterProposalExists();

            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtcounselling.Key).Return(1);

            //Mock the Debt Counselling Repository
            IDebtCounsellingRepository debtRepoMock = _mockery.StrictMock<IDebtCounsellingRepository>();
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), debtRepoMock);

            List<IProposal> proposalList = _mockery.StrictMock<List<IProposal>>();

            SetupResult.For(debtRepoMock.GetProposalsByType(1, ProposalTypes.CounterProposal)).IgnoreArguments().Return(proposalList);

            int expectedMessageCount = 1;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        [Test]
        public void DebtCounsellingRelatedActiveProposalExistsFailTest()
        {
            DebtCounsellingRelatedActiveProposalExists rule = new DebtCounsellingRelatedActiveProposalExists();

            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtcounselling.Key).Return(1);

            //Mock a debtCounselling Account
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.Key).Return(1);
            SetupResult.For(debtcounselling.Account).Return(account);

            //Mock the debtCounselling Case list
            List<IDebtCounselling> debtCounsellingList = _mockery.StrictMock<List<IDebtCounselling>>();
            IDebtCounselling debtCounsellingRelatedCase = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtCounsellingRelatedCase.Key).Return(2);
            debtCounsellingList.Add(debtCounsellingRelatedCase);

            //Mock the Debt Counselling Repository
            IDebtCounsellingRepository debtRepoMock = _mockery.StrictMock<IDebtCounsellingRepository>();
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), debtRepoMock);

            List<IProposal> proposalList = _mockery.StrictMock<List<IProposal>>();
            IProposal proposal = _mockery.StrictMock<IProposal>();
            SetupResult.For(proposal.Accepted).Return(true);
            proposalList.Add(proposal);

            SetupResult.For(debtRepoMock.GetDebtCounsellingByAccountKey(1, DebtCounsellingStatuses.Open)).IgnoreArguments().Return(debtCounsellingList);
            SetupResult.For(debtRepoMock.GetProposalsByTypeAndStatus(1, ProposalTypes.Proposal, ProposalStatuses.Active)).IgnoreArguments().Return(proposalList);

            int expectedMessageCount = 1;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        [Test]
        public void DebtCounsellingRelatedActiveProposalExistsPassTest1()
        {
            DebtCounsellingRelatedActiveProposalExists rule = new DebtCounsellingRelatedActiveProposalExists();

            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtcounselling.Key).Return(1);

            //Mock a debtCounselling Account
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.Key).Return(1);
            SetupResult.For(debtcounselling.Account).Return(account);

            //Mock the debtCounselling Case list
            List<IDebtCounselling> debtCounsellingList = _mockery.StrictMock<List<IDebtCounselling>>();
            debtCounsellingList.Add(debtcounselling);

            //Mock the Debt Counselling Repository
            IDebtCounsellingRepository debtRepoMock = _mockery.StrictMock<IDebtCounsellingRepository>();
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), debtRepoMock);

            SetupResult.For(debtRepoMock.GetDebtCounsellingByAccountKey(1, DebtCounsellingStatuses.Open)).IgnoreArguments().Return(debtCounsellingList);
            SetupResult.For(debtRepoMock.GetProposalsByTypeAndStatus(1, ProposalTypes.Proposal, ProposalStatuses.Active)).IgnoreArguments().Return(null);

            int expectedMessageCount = 0;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        [Test]
        public void DebtCounsellingRelatedActiveProposalExistsPassTest2()
        {
            DebtCounsellingRelatedActiveProposalExists rule = new DebtCounsellingRelatedActiveProposalExists();

            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtcounselling.Key).Return(1);

            //Mock a debtCounselling Account
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.Key).Return(1);
            SetupResult.For(debtcounselling.Account).Return(account);

            //Mock the debtCounselling Case list
            List<IDebtCounselling> debtCounsellingList = _mockery.StrictMock<List<IDebtCounselling>>();
            IDebtCounselling debtCounsellingRelatedCase = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtCounsellingRelatedCase.Key).Return(2);
            debtCounsellingList.Add(debtCounsellingRelatedCase);

            //Mock the Debt Counselling Repository
            IDebtCounsellingRepository debtRepoMock = _mockery.StrictMock<IDebtCounsellingRepository>();
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), debtRepoMock);

            //1 Proposal that is not accepted
            List<IProposal> proposalList = _mockery.StrictMock<List<IProposal>>();
            IProposal proposal = _mockery.StrictMock<IProposal>();
            SetupResult.For(proposal.Accepted).Return(false);
            proposalList.Add(proposal);

            SetupResult.For(debtRepoMock.GetDebtCounsellingByAccountKey(1, DebtCounsellingStatuses.Open)).IgnoreArguments().Return(debtCounsellingList);
            SetupResult.For(debtRepoMock.GetProposalsByTypeAndStatus(1, ProposalTypes.Proposal, ProposalStatuses.Active)).IgnoreArguments().Return(proposalList);

            int expectedMessageCount = 0;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        [Test]
        public void DebtCounsellingRelatedActiveProposalExistsPassTest3()
        {
            DebtCounsellingRelatedActiveProposalExists rule = new DebtCounsellingRelatedActiveProposalExists();

            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtcounselling.Key).Return(1);

            //Mock a debtCounselling Account
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.Key).Return(1);
            SetupResult.For(debtcounselling.Account).Return(account);

            //Mock the debtCounselling Case list
            List<IDebtCounselling> debtCounsellingList = _mockery.StrictMock<List<IDebtCounselling>>();
            IDebtCounselling debtCounsellingRelatedCase = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtCounsellingRelatedCase.Key).Return(2);
            debtCounsellingList.Add(debtCounsellingRelatedCase);

            //Mock the Debt Counselling Repository
            IDebtCounsellingRepository debtRepoMock = _mockery.StrictMock<IDebtCounsellingRepository>();
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), debtRepoMock);

            //1 Proposal that is not accepted and accepted bit is null
            List<IProposal> proposalList = _mockery.StrictMock<List<IProposal>>();
            IProposal proposal = _mockery.StrictMock<IProposal>();
            SetupResult.For(proposal.Accepted).Return(null);
            proposalList.Add(proposal);

            SetupResult.For(debtRepoMock.GetDebtCounsellingByAccountKey(1, DebtCounsellingStatuses.Open)).IgnoreArguments().Return(debtCounsellingList);
            SetupResult.For(debtRepoMock.GetProposalsByTypeAndStatus(1, ProposalTypes.Proposal, ProposalStatuses.Active)).IgnoreArguments().Return(proposalList);

            int expectedMessageCount = 0;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        [Test]
        public void IsDebtCounsellingUserTestPass()
        {
            //Can't create a proper test without setting up Impersonation
            SAHLPrincipal principal = base.TestPrincipal;

            SetupPrincipalCache(principal, true);
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            int expectedMessageCount = 0;
            IsDebtCounsellingUser rule = new IsDebtCounsellingUser();
            ExecuteRule(rule, expectedMessageCount, principal);
        }

        [Test]
        public void DebtCounsellingDeleteDebitOrderTestPass()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
            SAHLPrincipal principal = base.TestPrincipal;
            SetupPrincipalCache(principal, true);
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

            IManualDebitOrder manualDebitOrder = _mockery.StrictMock<IManualDebitOrder>();
            IFinancialService fs = _mockery.StrictMock<IFinancialService>();
            IAccount account = _mockery.StrictMock<IAccount>();

            SetupResult.For(account.Key).Return(1);
            SetupResult.For(account.UnderDebtCounselling).Return(true);
            SetupResult.For(fs.Account).Return(account);

            SetupResult.For(manualDebitOrder.UserID).Return("TestUser");
            SetupResult.For(manualDebitOrder.FinancialService).Return(fs);

            int expected = 1;
            DebtCounsellingDeleteDebitOrder rule = new DebtCounsellingDeleteDebitOrder();

            ExecuteRule(rule, expected, manualDebitOrder, principal);
        }

        [Test]
        public void DebtCounsellingDeleteDebitOrderTestFail()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
            SAHLPrincipal principal = base.TestPrincipal;
            SetupPrincipalCache(principal, true);
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

            IManualDebitOrder manualDebitOrder = _mockery.StrictMock<IManualDebitOrder>();
            IFinancialService fs = _mockery.StrictMock<IFinancialService>();
            IAccount account = _mockery.StrictMock<IAccount>();

            SetupResult.For(account.Key).Return(1);
            SetupResult.For(account.UnderDebtCounselling).Return(false);
            SetupResult.For(fs.Account).Return(account);

            SetupResult.For(manualDebitOrder.UserID).Return("TestUser");
            SetupResult.For(manualDebitOrder.FinancialService).Return(fs);

            int expected = 0;
            DebtCounsellingDeleteDebitOrder rule = new DebtCounsellingDeleteDebitOrder();

            ExecuteRule(rule, expected, manualDebitOrder, principal);
        }

        [Test]
        public void DebtCounsellingOpenCasesExistTestPass()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);

            DebtCounsellingOpenCasesExist rule = new DebtCounsellingOpenCasesExist();
            int expectedMessageCount = 0;
            ILegalEntityRepository leRepo;
            ILegalEntity le;

            using (new SessionScope())
            {
                string sql = String.Format(@"select top 1 legalentitykey from ExternalRole er
                    where er.ExternalRoleTypekey = 2 and GeneralStatusKey = 2
                    and legalentitykey not in (select legalentitykey from ExternalRole where ExternalRoleTypekey = 2 and GeneralStatusKey = 1)
                    order by ExternalRolekey desc");

                DataTable DT = base.GetQueryResults(sql);

                if (DT.Rows.Count == 1)
                {
                    int leKey = Convert.ToInt32(DT.Rows[0][0]);

                    leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                    le = (ILegalEntity)leRepo.GetLegalEntityByKey(leKey);

                    ExecuteRule(rule, expectedMessageCount, le);
                }
                else
                {
                    SetRepositoryStrategy(TypeFactoryStrategy.Mock);

                    leRepo = _mockery.StrictMock<ILegalEntityRepository>();
                    MockCache.Add(typeof(ILegalEntityRepository).ToString(), leRepo);

                    le = _mockery.StrictMock<ILegalEntity>();
                    SetupResult.For(le.Key).Return(1);

                    IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
                    SetupResult.For(gs.Key).Return((int)GeneralStatuses.Inactive);

                    IExternalRole er = _mockery.StrictMock<IExternalRole>();
                    SetupResult.For(er.GeneralStatus).Return(gs);

                    IReadOnlyEventList<IExternalRole> erList = new ReadOnlyEventList<IExternalRole>(
                        new[]
                        {
                            er
                        });

                    SetupResult.For(leRepo.GetExternalRoles(GenericKeyTypes.DebtCounselling2AM, ExternalRoleTypes.DebtCounsellor, 1)).IgnoreArguments().Return((IReadOnlyEventList<IExternalRole>)erList);

                    ExecuteRule(rule, expectedMessageCount, le);
                }
            }
        }

        [Test]
        public void DebtCounsellingOpenCasesExistTestFail()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);

            DebtCounsellingOpenCasesExist rule = new DebtCounsellingOpenCasesExist();
            int expectedMessageCount = 1;
            ILegalEntityRepository leRepo;
            ILegalEntity le;

            using (new SessionScope())
            {
                string sql = String.Format(@"select top 1 legalentitykey from ExternalRole er where er.ExternalRoleTypekey = 2 and GeneralStatusKey = 1 order by ExternalRolekey desc");

                DataTable DT = base.GetQueryResults(sql);

                if (DT.Rows.Count == 1)
                {
                    int leKey = Convert.ToInt32(DT.Rows[0][0]);

                    leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                    le = (ILegalEntity)leRepo.GetLegalEntityByKey(leKey);

                    ExecuteRule(rule, expectedMessageCount, le);
                }
                else
                {
                    SetRepositoryStrategy(TypeFactoryStrategy.Mock);

                    leRepo = _mockery.StrictMock<ILegalEntityRepository>();
                    MockCache.Add(typeof(ILegalEntityRepository).ToString(), leRepo);

                    le = _mockery.StrictMock<ILegalEntity>();
                    SetupResult.For(le.Key).Return(1);

                    IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
                    SetupResult.For(gs.Key).Return((int)GeneralStatuses.Active);

                    IExternalRole er = _mockery.StrictMock<IExternalRole>();
                    SetupResult.For(er.GeneralStatus).Return(gs);

                    IReadOnlyEventList<IExternalRole> erList = new ReadOnlyEventList<IExternalRole>(
                        new[]
                        {
                            er
                        });

                    SetupResult.For(leRepo.GetExternalRoles(GenericKeyTypes.DebtCounselling2AM, ExternalRoleTypes.DebtCounsellor, 1)).IgnoreArguments().Return((IReadOnlyEventList<IExternalRole>)erList);

                    ExecuteRule(rule, expectedMessageCount, le);
                }
            }
        }

        [Test]
        public void DebtCounsellingProposalTermLimit()
        {
            //Fail Test - term exceeds 276 months so display error
            DebtCounsellingProposalTermLimitFailTest();

            //Pass Test - term does not exceed 276 months so dont display anything
            DebtCounsellingProposalTermLimitPassTest();
        }

        private void DebtCounsellingProposalTermLimitFailTest()
        {
            DebtCounsellingProposalTermLimit rule = new DebtCounsellingProposalTermLimit();
            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtcounselling.Key).Return(1);

            //Setup the reason definition
            IReasonDefinition reasonDefinition = _mockery.StrictMock<IReasonDefinition>();
            IReasonDescription reasonDescription = _mockery.StrictMock<IReasonDescription>();
            SetupResult.For(reasonDescription.Key).Return((int)ReasonDescriptions.ProposalAcceptance);
            SetupResult.For(reasonDefinition.ReasonDescription).Return(reasonDescription);

            //Setup the account for the Debt Counselling object
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.OpenDate).Return(DateTime.Now.AddMonths(-278));
            SetupResult.For(debtcounselling.Account).Return(account);

            //Mock the Control Repository
            IControlRepository controlRepoMock = _mockery.StrictMock<IControlRepository>();
            MockCache.Add(typeof(IControlRepository).ToString(), controlRepoMock);

            //Mock a Control
            IControl ctrl = _mockery.StrictMock<IControl>();
            SetupResult.For(ctrl.ControlNumeric).Return(276);

            IProposal proposal = _mockery.StrictMock<IProposal>();
            SetupResult.For(proposal.Key).Return(1);
            SetupResult.For(proposal.TotalTerm).Return(278);

            SetupResult.For(debtcounselling.GetActiveProposal(ProposalTypes.Proposal)).Return(proposal);
            SetupResult.For(controlRepoMock.GetControlByDescription("1")).IgnoreArguments().Return(ctrl);

            int expectedMessageCount = 1;
            ExecuteRule(rule, expectedMessageCount, debtcounselling, reasonDefinition);
        }

        private void DebtCounsellingProposalTermLimitPassTest()
        {
            DebtCounsellingProposalTermLimit rule = new DebtCounsellingProposalTermLimit();
            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtcounselling.Key).Return(1);

            //Setup the account for the Debt Counselling object
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.OpenDate).Return(DateTime.Now.AddMonths(-27));
            SetupResult.For(debtcounselling.Account).Return(account);

            //Setup the reason definition
            IReasonDefinition reasonDefinition = _mockery.StrictMock<IReasonDefinition>();
            IReasonDescription reasonDescription = _mockery.StrictMock<IReasonDescription>();
            SetupResult.For(reasonDescription.Key).Return((int)ReasonDescriptions.ProposalAcceptance);
            SetupResult.For(reasonDefinition.ReasonDescription).Return(reasonDescription);

            //Mock the Control Repository
            IControlRepository controlRepoMock = _mockery.StrictMock<IControlRepository>();
            MockCache.Add(typeof(IControlRepository).ToString(), controlRepoMock);

            //Mock the Reasons Repository
            IReasonRepository reasonRepoMock = _mockery.StrictMock<IReasonRepository>();
            MockCache.Add(typeof(IReasonRepository).ToString(), reasonRepoMock);

            List<IProposal> proposalList = _mockery.StrictMock<List<IProposal>>();
            IProposal proposal = _mockery.StrictMock<IProposal>();
            SetupResult.For(proposal.Key).Return(1);
            SetupResult.For(proposal.TotalTerm).Return(275);

            //Mock a Control
            IControl ctrl = _mockery.StrictMock<IControl>();
            SetupResult.For(ctrl.ControlNumeric).Return(276);

            //Setup the reasons

            SetupResult.For(debtcounselling.GetActiveProposal(ProposalTypes.Proposal)).Return(proposal);
            SetupResult.For(controlRepoMock.GetControlByDescription("1")).IgnoreArguments().Return(ctrl);

            int expectedMessageCount = 0;
            ExecuteRule(rule, expectedMessageCount, debtcounselling, reasonDefinition);
        }

        [Test]
        public void DebtCounsellingProposalRemainingTerm()
        {
            //Fail Test
            DebtCounsellingProposalRemainingTermFailTest();

            //Pass Test
            DebtCounsellingProposalRemainingTermPassTest();
        }

        private void DebtCounsellingProposalRemainingTermFailTest()
        {
            DebtCounsellingProposalRemainingTerm rule = new DebtCounsellingProposalRemainingTerm();
            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtcounselling.Key).Return(1);

            //Setup the account for the Debt Counselling object
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.OpenDate).Return(DateTime.Now.AddMonths(-278));
            SetupResult.For(debtcounselling.Account).Return(account);

            //Mock the Control Repository
            IControlRepository controlRepoMock = _mockery.StrictMock<IControlRepository>();
            MockCache.Add(typeof(IControlRepository).ToString(), controlRepoMock);

            //Mock a Control
            IControl ctrl = _mockery.StrictMock<IControl>();
            SetupResult.For(ctrl.ControlNumeric).Return(276);

            IProposal proposal = _mockery.StrictMock<IProposal>();
            SetupResult.For(proposal.Key).Return(1);
            SetupResult.For(proposal.TotalTerm).Return(277);

            SetupResult.For(debtcounselling.GetActiveProposal(ProposalTypes.Proposal)).Return(proposal);
            SetupResult.For(controlRepoMock.GetControlByDescription("1")).IgnoreArguments().Return(ctrl);

            int expectedMessageCount = 1;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        private void DebtCounsellingProposalRemainingTermPassTest()
        {
            DebtCounsellingProposalRemainingTerm rule = new DebtCounsellingProposalRemainingTerm();
            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(debtcounselling.Key).Return(1);

            //Setup the account for the Debt Counselling object
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.OpenDate).Return(DateTime.Now.AddMonths(-27));
            SetupResult.For(debtcounselling.Account).Return(account);

            //Mock the Control Repository
            IControlRepository controlRepoMock = _mockery.StrictMock<IControlRepository>();
            MockCache.Add(typeof(IControlRepository).ToString(), controlRepoMock);

            //Mock a Control
            IControl ctrl = _mockery.StrictMock<IControl>();
            SetupResult.For(ctrl.ControlNumeric).Return(276);

            List<IProposal> proposalList = _mockery.StrictMock<List<IProposal>>();
            IProposal proposal = _mockery.StrictMock<IProposal>();
            SetupResult.For(proposal.Key).Return(1);
            SetupResult.For(proposal.TotalTerm).Return(275);

            SetupResult.For(debtcounselling.GetActiveProposal(ProposalTypes.Proposal)).Return(proposal);
            SetupResult.For(controlRepoMock.GetControlByDescription("1")).IgnoreArguments().Return(ctrl);

            int expectedMessageCount = 0;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        [Test]
        public void ProposalItemsDatesOverlap()
        {
            //Pass Test
            ProposalItemsDatesOverlapPassTest();

            //One date overlaps
            ProposalItemsDatesOverlapFailTest();

            //More than one date overlaps
            ProposalItemsMultipleDatesOverlapFailTest();
        }

        #region Hearing Detail Tests

        [Test]
        public void DebtCounsellingHearingDetailCommentTests()
        {
            DebtCounsellingHearingDetailCommentPassTest1();
            DebtCounsellingHearingDetailCommentPassTest2();
            DebtCounsellingHearingDetailCommentFailTest1();
            DebtCounsellingHearingDetailCommentFailTest2();
        }

        private void DebtCounsellingHearingDetailCommentPassTest1()
        {
            HearingDetailCommentMandatory rule = new HearingDetailCommentMandatory();
            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();

            //Add 2 active hearing details
            IList<IHearingDetail> activeHearingDetails = new List<IHearingDetail>();
            IHearingDetail hearingDetailItem1 = _mockery.StrictMock<IHearingDetail>();
            SetupResult.For(hearingDetailItem1.Key).Return(1);
            SetupResult.For(hearingDetailItem1.Comment).Return(null);
            activeHearingDetails.Add(hearingDetailItem1);

            IHearingDetail hearingDetailItem2 = _mockery.StrictMock<IHearingDetail>();
            SetupResult.For(hearingDetailItem2.Key).Return(2);
            SetupResult.For(hearingDetailItem2.Comment).Return("test");
            activeHearingDetails.Add(hearingDetailItem2);

            SetupResult.For(debtcounselling.GetActiveHearingDetails).Return(activeHearingDetails);

            int expectedMessageCount = 0;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        private void DebtCounsellingHearingDetailCommentPassTest2()
        {
            HearingDetailCommentMandatory rule = new HearingDetailCommentMandatory();
            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();

            //Add 1 active hearing detail
            IList<IHearingDetail> activeHearingDetails = new List<IHearingDetail>();
            IHearingDetail hearingDetailItem1 = _mockery.StrictMock<IHearingDetail>();
            SetupResult.For(hearingDetailItem1.Key).Return(1);
            SetupResult.For(hearingDetailItem1.Comment).Return("test");
            activeHearingDetails.Add(hearingDetailItem1);

            SetupResult.For(debtcounselling.GetActiveHearingDetails).Return(activeHearingDetails);

            int expectedMessageCount = 0;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        private void DebtCounsellingHearingDetailCommentFailTest1()
        {
            HearingDetailCommentMandatory rule = new HearingDetailCommentMandatory();
            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();

            //Add 2 active hearing details
            IList<IHearingDetail> activeHearingDetails = new List<IHearingDetail>();
            IHearingDetail hearingDetailItem1 = _mockery.StrictMock<IHearingDetail>();
            SetupResult.For(hearingDetailItem1.Key).Return(1);
            SetupResult.For(hearingDetailItem1.Comment).Return(null);
            activeHearingDetails.Add(hearingDetailItem1);

            IHearingDetail hearingDetailItem2 = _mockery.StrictMock<IHearingDetail>();
            SetupResult.For(hearingDetailItem2.Key).Return(2);
            SetupResult.For(hearingDetailItem2.Comment).Return(null);
            activeHearingDetails.Add(hearingDetailItem2);

            SetupResult.For(debtcounselling.GetActiveHearingDetails).Return(activeHearingDetails);

            int expectedMessageCount = 1;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        private void DebtCounsellingHearingDetailCommentFailTest2()
        {
            HearingDetailCommentMandatory rule = new HearingDetailCommentMandatory();
            IDebtCounselling debtcounselling = _mockery.StrictMock<IDebtCounselling>();

            //Add 1 active hearing details to the list
            IList<IHearingDetail> activeHearingDetails = new List<IHearingDetail>();
            IHearingDetail hearingDetailItem1 = _mockery.StrictMock<IHearingDetail>();
            SetupResult.For(hearingDetailItem1.Key).Return(1);
            SetupResult.For(hearingDetailItem1.Comment).Return(null);
            activeHearingDetails.Add(hearingDetailItem1);

            SetupResult.For(debtcounselling.GetActiveHearingDetails).Return(activeHearingDetails);

            int expectedMessageCount = 1;
            ExecuteRule(rule, expectedMessageCount, debtcounselling);
        }

        #endregion Hearing Detail Tests

        private void ProposalItemsDatesOverlapPassTest()
        {
            ProposalItemsDatesOverlap rule = new ProposalItemsDatesOverlap();

            IProposal proposal = _mockery.StrictMock<IProposal>();

            IEventList<IProposalItem> propItems = new EventList<IProposalItem>();
            IProposalItem proposalItem1 = _mockery.StrictMock<IProposalItem>();
            DateTime CurrentDateTime = DateTime.Now;
            SetupResult.For(proposalItem1.StartDate).Return(CurrentDateTime.AddDays(-30));
            SetupResult.For(proposalItem1.EndDate).Return(CurrentDateTime.AddDays(-28));
            propItems.Add(new DomainMessageCollection(), proposalItem1);

            IProposalItem proposalItem2 = _mockery.StrictMock<IProposalItem>();
            SetupResult.For(proposalItem2.StartDate).Return(CurrentDateTime.AddDays(-27));
            SetupResult.For(proposalItem2.EndDate).Return(CurrentDateTime.AddDays(-25));
            propItems.Add(new DomainMessageCollection(), proposalItem2);

            IProposalItem proposalItem3 = _mockery.StrictMock<IProposalItem>();
            SetupResult.For(proposalItem3.StartDate).Return(CurrentDateTime.AddDays(-24));
            SetupResult.For(proposalItem3.EndDate).Return(CurrentDateTime.AddDays(-22));
            propItems.Add(new DomainMessageCollection(), proposalItem3);

            SetupResult.For(proposal.ProposalItems).Return(propItems);

            int expectedMessageCount = 0;
            ExecuteRule(rule, expectedMessageCount, proposal);
        }

        private void ProposalItemsDatesOverlapFailTest()
        {
            ProposalItemsDatesOverlap rule = new ProposalItemsDatesOverlap();

            IProposal proposal = _mockery.StrictMock<IProposal>();

            IEventList<IProposalItem> propItems = new EventList<IProposalItem>();
            IProposalItem proposalItem1 = _mockery.StrictMock<IProposalItem>();
            DateTime CurrentDateTime = DateTime.Now;
            SetupResult.For(proposalItem1.StartDate).Return(CurrentDateTime.AddDays(-30));
            SetupResult.For(proposalItem1.EndDate).Return(CurrentDateTime.AddDays(-28));
            propItems.Add(new DomainMessageCollection(), proposalItem1);

            IProposalItem proposalItem2 = _mockery.StrictMock<IProposalItem>();
            SetupResult.For(proposalItem2.StartDate).Return(CurrentDateTime.AddDays(-28));
            SetupResult.For(proposalItem2.EndDate).Return(CurrentDateTime.AddDays(-25));
            propItems.Add(new DomainMessageCollection(), proposalItem2);

            IProposalItem proposalItem3 = _mockery.StrictMock<IProposalItem>();
            SetupResult.For(proposalItem3.StartDate).Return(CurrentDateTime.AddDays(-24));
            SetupResult.For(proposalItem3.EndDate).Return(CurrentDateTime.AddDays(-22));
            propItems.Add(new DomainMessageCollection(), proposalItem3);

            SetupResult.For(proposal.ProposalItems).Return(propItems);

            int expectedMessageCount = 1;
            ExecuteRule(rule, expectedMessageCount, proposal);
        }

        private void ProposalItemsMultipleDatesOverlapFailTest()
        {
            ProposalItemsDatesOverlap rule = new ProposalItemsDatesOverlap();

            IProposal proposal = _mockery.StrictMock<IProposal>();

            IEventList<IProposalItem> propItems = new EventList<IProposalItem>();
            IProposalItem proposalItem1 = _mockery.StrictMock<IProposalItem>();
            DateTime CurrentDateTime = DateTime.Now;
            SetupResult.For(proposalItem1.StartDate).Return(CurrentDateTime.AddDays(-30));
            SetupResult.For(proposalItem1.EndDate).Return(CurrentDateTime.AddDays(-28));
            propItems.Add(new DomainMessageCollection(), proposalItem1);

            IProposalItem proposalItem2 = _mockery.StrictMock<IProposalItem>();
            SetupResult.For(proposalItem2.StartDate).Return(CurrentDateTime.AddDays(-28));
            SetupResult.For(proposalItem2.EndDate).Return(CurrentDateTime.AddDays(-25));
            propItems.Add(new DomainMessageCollection(), proposalItem2);

            IProposalItem proposalItem3 = _mockery.StrictMock<IProposalItem>();
            SetupResult.For(proposalItem3.StartDate).Return(CurrentDateTime.AddDays(-23));
            SetupResult.For(proposalItem3.EndDate).Return(CurrentDateTime.AddDays(-22));
            propItems.Add(new DomainMessageCollection(), proposalItem3);

            SetupResult.For(proposal.ProposalItems).Return(propItems);

            int expectedMessageCount = 1;
            ExecuteRule(rule, expectedMessageCount, proposal);
        }

        /// <summary>
        /// Debt Counselling Active Counter Proposal Requires Reason Pass with Reasons
        /// </summary>
        [Test]
        public void DebtCounsellingActiveCounterProposalRequiresReasonPassWithReasons()
        {
            DebtCounsellingActiveCounterProposalRequiresReasonHelper(true, ProposalTypes.CounterProposal, 0);
        }

        /// <summary>
        /// Debt Counselling Active Counter Proposal Requires Reason Fail without Reasons
        /// </summary>
        [Test]
        public void DebtCounsellingActiveCounterProposalRequiresReasonFailWithoutReasons()
        {
            DebtCounsellingActiveCounterProposalRequiresReasonHelper(false, ProposalTypes.CounterProposal, 1);
        }

        /// <summary>
        /// Debt Counselling Active Counter Proposal Requires Reason Helper
        /// </summary>
        /// <param name="hasReasons"></param>
        /// <param name="expectedMessageCount"></param>
        private void DebtCounsellingActiveCounterProposalRequiresReasonHelper(bool hasReasons, ProposalTypes proposalType, int expectedMessageCount)
        {
            DebtCounsellingActiveCounterProposalRequiresReason rule = new DebtCounsellingActiveCounterProposalRequiresReason();
            IProposal proposal = _mockery.StrictMock<IProposal>();
            IReason reason = _mockery.StrictMock<IReason>();

            //I cannot believe that I had to use propType (abbreviation fail)
            IProposalType propType = _mockery.StrictMock<IProposalType>();

            SetupResult.For(propType.Key).Return((int)proposalType);
            SetupResult.For(proposal.ProposalType).Return(propType);
            SetupResult.For(proposal.ActiveReason).Return(hasReasons ? reason : null);

            ExecuteRule(rule, expectedMessageCount, proposal);
        }

        [Test]
        public void CounterProposalReasonMemo()
        {
            //Active CP with memo and description
            CounterProposalReasonMemoHelper(ProposalStatuses.Active, ProposalTypes.CounterProposal, true, true, 0);

            //Active CP with memo no description
            CounterProposalReasonMemoHelper(ProposalStatuses.Active, ProposalTypes.CounterProposal, true, false, 1);

            //Active CP no memo
            CounterProposalReasonMemoHelper(ProposalStatuses.Active, ProposalTypes.CounterProposal, false, false, 1);

            //Draft CP no memo
            CounterProposalReasonMemoHelper(ProposalStatuses.Draft, ProposalTypes.CounterProposal, false, false, 0);

            //Draft CP no memo
            CounterProposalReasonMemoHelper(ProposalStatuses.Inactive, ProposalTypes.CounterProposal, false, false, 0);

            //Inactive Proposal no memo
            CounterProposalReasonMemoHelper(ProposalStatuses.Active, ProposalTypes.Proposal, false, false, 0);
        }

        private void CounterProposalReasonMemoHelper(ProposalStatuses propStatus, ProposalTypes propType, bool hasMemo, bool hasMemoDescription, int msgCount)
        {
            CounterProposalReasonMemo rule = new CounterProposalReasonMemo();

            IProposal proposal = _mockery.StrictMock<IProposal>();
            IProposalStatus ps = _mockery.StrictMock<IProposalStatus>();
            IProposalType pt = _mockery.StrictMock<IProposalType>();

            SetupResult.For(ps.Key).Return((int)propStatus);
            SetupResult.For(pt.Key).Return((int)propType);

            SetupResult.For(proposal.Key).Return(1);
            SetupResult.For(proposal.ProposalType).Return(pt);
            SetupResult.For(proposal.ProposalStatus).Return(ps);

            if (hasMemo)
            {
                IMemo memo = _mockery.StrictMock<IMemo>();

                if (hasMemoDescription)
                    SetupResult.For(memo.Description).Return("some text here....");
                else
                    SetupResult.For(memo.Description).Return("");

                SetupResult.For(proposal.Memo).Return(memo);
            }
            else
                SetupResult.For(proposal.Memo).Return(null);

            ExecuteRule(rule, msgCount, proposal);
        }

        [Test]
        public void ManyDCCasesForAccount()
        {
            using (new SessionScope(FlushAction.Never))
            {
                MultipleDebtCounsellingCasesForAccount rule = new MultipleDebtCounsellingCasesForAccount(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string query = String.Format(@"select dc.AccountKey, Count(dc.AccountKey) as Occurs
                    from debtcounselling.DebtCounselling dc (nolock)
                    where
	                    dc.DebtCounsellingStatusKey = 1
	                    --and dc.AccountKey = @AccountKey
                    Group by AccountKey
                    having Count(dc.AccountKey) > 1");

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                int accKey = Convert.ToInt32(obj);
                if (accKey > 0)
                    ExecuteRule(rule, 1, accKey);

                query = String.Format(@"select dc.AccountKey, Count(dc.AccountKey) as Occurs
                    from debtcounselling.DebtCounselling dc (nolock)
                    where
	                    dc.DebtCounsellingStatusKey = 1
	                    --and dc.AccountKey = @AccountKey
                    Group by AccountKey
                    having Count(dc.AccountKey) = 1");

                obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                accKey = Convert.ToInt32(obj);
                if (accKey > 0)
                    ExecuteRule(rule, 0, accKey);
            }
        }

        [Test]
        public void ManyDCGroupsForLE()
        {
            using (new SessionScope(FlushAction.Never))
            {
                MultipleDebtCounsellingGroupsForLegalEntity rule = new MultipleDebtCounsellingGroupsForLegalEntity(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string query = String.Format(@"select min(dc.AccountKey) as AccountKey, er.LegalEntityKey, min(dcg.DebtCounsellingGroupKey) as minDCG, max(dcg.DebtCounsellingGroupKey) as maxDCG, Count(er.LegalEntityKey)
                        from debtcounselling.DebtCounsellingGroup dcg (nolock)
                        join debtcounselling.DebtCounselling dc (nolock) on dcg.DebtCounsellingGroupKey = dc.DebtCounsellingGroupKey
		                        and dc.DebtCounsellingStatusKey = 1 -- open only
                        join ExternalRole er (nolock) on dc.DebtCounsellingKey = er.GenericKey
		                        and er.GenericKeyTypeKey = 27 --DebtCounselling
		                        and er.ExternalRoleTypeKey = 1
		                        and er.GeneralStatusKey = 1
                        Group By er.LegalEntityKey
                        Having min(dcg.DebtCounsellingGroupKey) != max(dcg.DebtCounsellingGroupKey)");

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                int accKey = Convert.ToInt32(obj);
                if (accKey > 0)
                    ExecuteRule(rule, 1, accKey);

                query = String.Format(@"select min(dc.AccountKey) as AccountKey, er.LegalEntityKey, min(dcg.DebtCounsellingGroupKey) as minDCG, max(dcg.DebtCounsellingGroupKey) as maxDCG, Count(er.LegalEntityKey)
                        from debtcounselling.DebtCounsellingGroup dcg (nolock)
                        join debtcounselling.DebtCounselling dc (nolock) on dcg.DebtCounsellingGroupKey = dc.DebtCounsellingGroupKey
		                        and dc.DebtCounsellingStatusKey = 1 -- open only
                        join ExternalRole er (nolock) on dc.DebtCounsellingKey = er.GenericKey
		                        and er.GenericKeyTypeKey = 27 --DebtCounselling
		                        and er.ExternalRoleTypeKey = 1
		                        and er.GeneralStatusKey = 1
                        Group By er.LegalEntityKey
                        Having min(dcg.DebtCounsellingGroupKey) = max(dcg.DebtCounsellingGroupKey)");

                obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                accKey = Convert.ToInt32(obj);
                if (accKey > 0)
                    ExecuteRule(rule, 0, accKey);
            }
        }

        #region DebtCounsellingHasLitigationAttorney

        [Test]
        public void DebtCounsellingHasLitigationAttorneyTestPass()
        {
            //            string sql = @"select top 1 er.GenericKey DebtCounsellingKey
            //                            from dbo.ExternalRole er (nolock)
            //                            join dbo.LegalEntity le (nolock) on le.LegalEntityKey = er.LegalEntityKey
            //                            join dbo.Attorney a (nolock) on a.LegalEntityKey = er.LegalEntityKey
            //                            join dbo.ExternalRole er2 (nolock) on er2.GenericKey = a.AttorneyKey
            //		                            and er2.GenericKeyTypeKey = 35 and er2.ExternalRoleTypeKey = 6 and er2.GeneralStatusKey = 1
            //                            where er.GenericKeyTypeKey = 27
            //                            and er.GeneralStatusKey = 1
            //                            and er.ExternalRoleTypeKey = 5";

            //            DataTable DT = base.GetQueryResults(sql);
            //            int DebtCounsellingKey = Convert.ToInt32(DT.Rows[0][0]);
            //            IDebtCounsellingRepository repo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            //            IDebtCounselling dc = repo.GetDebtCounsellingByKey(DebtCounsellingKey);

            //            _ruleService = ServiceFactory.GetService<IRuleService>();
            //            _messages = new DomainMessageCollection();

            //            _ruleService.Enabled = true;
            //            int result = _ruleService.ExecuteRule(_messages, "DebtCounsellingHasLitigationAttorney", dc);
            //            _ruleService.Enabled = false;

            //            Assert.That(result == 1);

            DebtCounsellingHasLitigationAttorney rule = new DebtCounsellingHasLitigationAttorney();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            IAttorney litigationAttorney = _mockery.StrictMock<IAttorney>();
            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();

            //IList<ILegalEntity> le = attorney.GetContacts(ExternalRoleTypes.DebtCounselling, GeneralStatuses.Active);
            //SetupResult.For(dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(debtCounsellingKey, (int)HearingTypes.Court, hearingAppearanceTypeKeys)).IgnoreArguments().Return(false);
            IDebtCounsellingRepository dcRepo = _mockery.StrictMock<IDebtCounsellingRepository>();
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), dcRepo);
            SetupResult.For(debtCounselling.LitigationAttorney).Return(litigationAttorney);
            SetupResult.For(litigationAttorney.GetContacts(ExternalRoleTypes.DebtCounselling, GeneralStatuses.Active)).IgnoreArguments().Return(new List<ILegalEntity>() { le });
            ExecuteRule(rule, 0, debtCounselling);
        }

        [Test]
        public void DebtCounsellingHasLitigationAttorneyTestFail()
        {
            DebtCounsellingHasLitigationAttorney rule = new DebtCounsellingHasLitigationAttorney();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            ILegalEntity litigationAttorney = _mockery.StrictMock<ILegalEntity>();
            IDebtCounsellingRepository dcRepo = _mockery.StrictMock<IDebtCounsellingRepository>();
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), dcRepo);
            SetupResult.For(debtCounselling.LitigationAttorney).Return(null);
            ExecuteRule(rule, 1, debtCounselling);
        }

        #endregion DebtCounsellingHasLitigationAttorney

        [Test]
        public void DebtCounsellingDepositCheck()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);

            using (new SessionScope(FlushAction.Never))
            {
                DebtCounsellingDepositCheck rule = new DebtCounsellingDepositCheck();

                string query = String.Format(@"SELECT top 1 dc.DebtCounsellingKey--, max(lt.loantransactioneffectivedate), max(st.transitiondate)
                    FROM debtcounselling.debtcounselling AS dc (nolock)
                    left JOIN FinancialService fs (nolock) on dc.AccountKey = fs.AccountKey
                    INNER JOIN stagetransition AS st (nolock)
                             ON st.generickey = dc.debtcounsellingkey
	                     AND st.stagedefinitionstagedefinitiongroupkey = 4445
                           INNER JOIN fin.FinancialTransaction AS ft (nolock)
                             ON fs.FinancialServicekey = ft.FinancialServicekey
                    WHERE
                           ft.TransactionTypeKey = 312
                           group by dc.DebtCounsellingKey
                           having max(ft.EffectiveDate) <  max(st.endtransitiondate)");

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                int dcKey = Convert.ToInt32(obj);
                if (dcKey > 0)
                    ExecuteRule(rule, 0, dcKey);

                query = String.Format(@"SELECT top 1 dc.DebtCounsellingKey--, max(lt.loantransactioneffectivedate), max(st.transitiondate)
                    FROM debtcounselling.debtcounselling AS dc (nolock)
                    left JOIN FinancialService fs (nolock) on dc.AccountKey = fs.AccountKey
                    INNER JOIN stagetransition AS st (nolock)
                             ON st.generickey = dc.debtcounsellingkey
	                     AND st.stagedefinitionstagedefinitiongroupkey = 4445
                           INNER JOIN fin.FinancialTransaction AS ft (nolock)
                             ON fs.FinancialServicekey = ft.FinancialServicekey
                    WHERE
                           ft.TransactionTypeKey = 312
                           group by dc.DebtCounsellingKey
                           having max(ft.EffectiveDate) >  max(st.endtransitiondate)");

                obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                dcKey = Convert.ToInt32(obj);
                if (dcKey > 0)
                    ExecuteRule(rule, 1, dcKey);
            }
        }

        #region DebtCounsellingHasPaymentDistributionAgent

        [Test]
        public void DebtCounsellingHasPaymentDistributionAgentTestPass()
        {
            DebtCounsellingHasPaymentDistributionAgent rule = new DebtCounsellingHasPaymentDistributionAgent();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            ILegalEntity paymentDistributionAgent = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(debtCounselling.PaymentDistributionAgent).Return(paymentDistributionAgent);
            ExecuteRule(rule, 0, debtCounselling);
        }

        [Test]
        public void DebtCounsellingHasPaymentDistributionAgentTestFail()
        {
            DebtCounsellingHasPaymentDistributionAgent rule = new DebtCounsellingHasPaymentDistributionAgent();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            ILegalEntity paymentDistributionAgent = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(debtCounselling.PaymentDistributionAgent).Return(null);
            ExecuteRule(rule, 1, debtCounselling);
        }

        //has PDA
        [Test]
        public void DebtCounsellingHasPaymentDistributionAgentOrDebitOrderTestPass1()
        {
            DebtCounsellingHasPaymentDistributionAgentOrDebitOrder rule = new DebtCounsellingHasPaymentDistributionAgentOrDebitOrder();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            ILegalEntity paymentDistributionAgent = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(debtCounselling.PaymentDistributionAgent).Return(paymentDistributionAgent);
            ExecuteRule(rule, 0, debtCounselling);
        }

        //does not have PDA but has d/o
        [Test]
        public void DebtCounsellingHasPaymentDistributionAgentOrDebitOrderTestPass2()
        {
            DebtCounsellingHasPaymentDistributionAgentOrDebitOrder rule = new DebtCounsellingHasPaymentDistributionAgentOrDebitOrder();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(debtCounselling.PaymentDistributionAgent).Return(null);
            SetupResult.For(debtCounselling.Account).Return(account);
            SetupResult.For(account.Details).Return(null);
            SetupResult.For(account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan)).Return(financialService);
            SetupResult.For(financialService.CurrentBankAccount).Return(financialServiceBankAccount);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);

            ExecuteRule(rule, 0, debtCounselling);
        }

        //does not have PDA but has d/o with no d/o suspended detail types
        [Test]
        public void DebtCounsellingHasPaymentDistributionAgentOrDebitOrderTestPass3()
        {
            DebtCounsellingHasPaymentDistributionAgentOrDebitOrder rule = new DebtCounsellingHasPaymentDistributionAgentOrDebitOrder();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IEventList<IDetail> details = _mockery.StrictMock<IEventList<IDetail>>();
            IDetail detail = _mockery.StrictMock<IDetail>();
            IDetailType detailType = _mockery.StrictMock<IDetailType>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(debtCounselling.PaymentDistributionAgent).Return(null);
            SetupResult.For(debtCounselling.Account).Return(account);

            // any dt other than DebitOrderSuspended or BankDetailsIncorrect
            SetupResult.For(detailType.Key).Return((int)DetailTypes.InstructionSent);
            SetupResult.For(detail.DetailType).Return(detailType);
            details = new EventList<IDetail>(new IDetail[] { detail });
            SetupResult.For(account.Details).Return(details);
            SetupResult.For(account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan)).Return(financialService);
            SetupResult.For(financialService.CurrentBankAccount).Return(financialServiceBankAccount);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);

            ExecuteRule(rule, 0, debtCounselling);
        }

        //does not have PDA or d/o but has Direct Payment
        [Test]
        public void DebtCounsellingHasPaymentDistributionAgentOrDebitOrderTestPass4()
        {
            DebtCounsellingHasPaymentDistributionAgentOrDebitOrder rule = new DebtCounsellingHasPaymentDistributionAgentOrDebitOrder();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(debtCounselling.PaymentDistributionAgent).Return(null);
            SetupResult.For(debtCounselling.Account).Return(account);
            SetupResult.For(account.Details).Return(null);
            SetupResult.For(account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan)).Return(financialService);
            SetupResult.For(financialService.CurrentBankAccount).Return(financialServiceBankAccount);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.DirectPayment);

            ExecuteRule(rule, 0, debtCounselling);
        }

        //does not have PDA or d/o but has Subsidy Account
        [Test]
        public void DebtCounsellingHasPaymentDistributionAgentOrDebitOrderTestPass5()
        {
            DebtCounsellingHasPaymentDistributionAgentOrDebitOrder rule = new DebtCounsellingHasPaymentDistributionAgentOrDebitOrder();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(debtCounselling.PaymentDistributionAgent).Return(null);
            SetupResult.For(debtCounselling.Account).Return(account);
            SetupResult.For(account.Details).Return(null);
            SetupResult.For(account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan)).Return(financialService);
            SetupResult.For(financialService.CurrentBankAccount).Return(financialServiceBankAccount);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.SubsidyPayment);

            ExecuteRule(rule, 0, debtCounselling);
        }

        //does not have PDA but has d/o and d/o suspended detail types
        [Test]
        public void DebtCounsellingHasPaymentDistributionAgentOrDebitOrderTestFail1()
        {
            DebtCounsellingHasPaymentDistributionAgentOrDebitOrder rule = new DebtCounsellingHasPaymentDistributionAgentOrDebitOrder();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IEventList<IDetail> details = _mockery.StrictMock<IEventList<IDetail>>();
            IDetail detail = _mockery.StrictMock<IDetail>();
            IDetailType detailType = _mockery.StrictMock<IDetailType>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(debtCounselling.PaymentDistributionAgent).Return(null);
            SetupResult.For(debtCounselling.Account).Return(account);
            SetupResult.For(detailType.Key).Return((int)DetailTypes.DebitOrderSuspended);
            SetupResult.For(detail.DetailType).Return(detailType);
            details = new EventList<IDetail>(new IDetail[] { detail });
            SetupResult.For(account.Details).Return(details);
            SetupResult.For(account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan)).Return(financialService);
            SetupResult.For(financialService.CurrentBankAccount).Return(financialServiceBankAccount);
            SetupResult.For(financialServiceBankAccount.FinancialServicePaymentType).Return(financialServicePaymentType);
            SetupResult.For(financialServicePaymentType.Key).Return((int)FinancialServicePaymentTypes.DirectPayment);

            ExecuteRule(rule, 1, debtCounselling);
        }

        //does not have PDA and no detailtypes with financialservice but no financialservicebankaccount
        [Test]
        public void DebtCounsellingHasPaymentDistributionAgentOrDebitOrderTestFail2()
        {
            DebtCounsellingHasPaymentDistributionAgentOrDebitOrder rule = new DebtCounsellingHasPaymentDistributionAgentOrDebitOrder();
            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IEventList<IDetail> details = _mockery.StrictMock<IEventList<IDetail>>();
            IDetail detail = _mockery.StrictMock<IDetail>();
            IDetailType detailType = _mockery.StrictMock<IDetailType>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            IFinancialServiceBankAccount financialServiceBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType financialServicePaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(debtCounselling.PaymentDistributionAgent).Return(null);
            SetupResult.For(debtCounselling.Account).Return(account);
            SetupResult.For(detail.DetailType).Return(null);
            SetupResult.For(account.Details).Return(null);
            SetupResult.For(account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan)).Return(financialService);
            SetupResult.For(financialService.CurrentBankAccount).Return(null);

            ExecuteRule(rule, 1, debtCounselling);
        }

        #endregion DebtCounsellingHasPaymentDistributionAgent

        /// <summary>
        /// Debt Counselling Term Review Date Tests
        /// </summary>
        [Test]
        public void DebtCounsellingTermReviewDateTests()
        {
            //Null - Error Count 1
            DebtCounsellingTermReviewDateMandatoryHelper(null, 1);
        }

        /// <summary>
        /// Debt Counselling Term Review Date Mandatory Helper
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <param name="expectedErrorCount"></param>
        public void DebtCounsellingTermReviewDateMandatoryHelper(DateTime? dateToCheck, int expectedErrorCount)
        {
            DebtCounsellingTermReviewDateMandatory rule = new DebtCounsellingTermReviewDateMandatory();

            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            IProposal proposal = _mockery.StrictMock<IProposal>();

            SetupResult.For(debtCounselling.AcceptedActiveProposal).Return(proposal);
            SetupResult.For(proposal.ReviewDate).Return(dateToCheck);

            ExecuteRule(rule, expectedErrorCount, debtCounselling);
        }

        /// <summary>
        /// Debt Counselling Payment Received Date Tests
        /// </summary>
        [Test]
        public void DebtCounsellingReceivedDateMandatoryTests()
        {
            //Null - Error Count 1
            DebtCounsellingReceivedDateMandatoryHelper(null, 1);

            //Future Dated - Error Count 1
            DebtCounsellingReceivedDateMandatoryHelper(DateTime.Now.AddDays(1), 1);
        }

        /// <summary>
        /// DebtCounsellingReceivedDateMandatory
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <param name="expectedErrorCount"></param>
        public void DebtCounsellingReceivedDateMandatoryHelper(DateTime? dateToCheck, int expectedErrorCount)
        {
            DebtCounsellingPaymentReceivedDateMandatory rule = new DebtCounsellingPaymentReceivedDateMandatory();

            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();

            SetupResult.For(debtCounselling.PaymentReceivedDate).Return(dateToCheck);

            ExecuteRule(rule, expectedErrorCount, debtCounselling);
        }

        /// <summary>
        /// DebtCounsellingPaymentReceivedAmountMandatory
        /// </summary>
        [Test]
        public void DebtCounsellingPaymentReceivedAmountMandatoryTests()
        {
            //0 - Error Count 1
            DebtCounsellingPaymentReceivedAmountMandatoryHelper(0, 1);

            //-1 - Error Count 1
            DebtCounsellingPaymentReceivedAmountMandatoryHelper(-1, 1);

            //1 - Error Count 0
            DebtCounsellingPaymentReceivedAmountMandatoryHelper(1, 0);
        }

        /// <summary>
        /// DebtCounsellingPaymentReceivedAmountMandatory
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <param name="expectedErrorCount"></param>
        public void DebtCounsellingPaymentReceivedAmountMandatoryHelper(double? value, int expectedErrorCount)
        {
            DebtCounsellingPaymentReceivedAmountMandatory rule = new DebtCounsellingPaymentReceivedAmountMandatory();

            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();

            SetupResult.For(debtCounselling.PaymentReceivedAmount).Return(value);

            ExecuteRule(rule, expectedErrorCount, debtCounselling);
        }

        /// <summary>
        /// DebtCounsellingMaximumReviewDateInMonths
        /// </summary>
        [Test]
        public void DebtCounsellingMaximumReviewDateInMonthsTests()
        {
            //18 Months in the future - Error Count 1
            DebtCounsellingMaximumReviewDateInMonthsHelper(DateTime.Now.AddMonths(18).AddDays(1), 1);

            //1 Day in the Future - Error Count 0
            DebtCounsellingMaximumReviewDateInMonthsHelper(DateTime.Now.AddDays(1), 0);
        }

        /// <summary>
        /// DebtCounsellingMaximumReviewDateInMonths
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <param name="expectedErrorCount"></param>
        public void DebtCounsellingMaximumReviewDateInMonthsHelper(DateTime? dateToCheck, int expectedErrorCount)
        {
            DebtCounsellingMaximumReviewDateInMonths rule = new DebtCounsellingMaximumReviewDateInMonths();

            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            IProposal proposal = _mockery.StrictMock<IProposal>();

            SetupResult.For(debtCounselling.AcceptedActiveProposal).Return(proposal);
            SetupResult.For(proposal.ReviewDate).Return(dateToCheck);

            ExecuteRule(rule, expectedErrorCount, debtCounselling);
        }

        /// <summary>
        /// DebtCounsellingMaximumReviewDateInMonths
        /// </summary>
        [Test]
        public void DebtCounsellingMinimumReviewDateTests()
        {
            //Past Dated - Error Count 1
            DebtCounsellingMinimumReviewDateHelper(DateTime.Now.AddDays(-1), 1);

            //1 Day in the Future - Error Count 0
            DebtCounsellingMinimumReviewDateHelper(DateTime.Now.AddDays(1), 0);
        }

        /// <summary>
        /// DebtCounsellingMaximumReviewDateInMonths
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <param name="expectedErrorCount"></param>
        public void DebtCounsellingMinimumReviewDateHelper(DateTime? dateToCheck, int expectedErrorCount)
        {
            DebtCounsellingMinimumReviewDate rule = new DebtCounsellingMinimumReviewDate();

            IDebtCounselling debtCounselling = _mockery.StrictMock<IDebtCounselling>();
            IProposal proposal = _mockery.StrictMock<IProposal>();

            SetupResult.For(debtCounselling.AcceptedActiveProposal).Return(proposal);
            SetupResult.For(proposal.ReviewDate).Return(dateToCheck);

            ExecuteRule(rule, expectedErrorCount, debtCounselling);
        }

        /// <summary>
        /// DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay
        /// </summary>
        [Test]
        public void DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDayTests()
        {
            DateTime firstDate = DateTime.Now;
            DateTime secondDate = DateTime.Now;

            //same Day - Error Count 0
            DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDayHelper(firstDate, secondDate, 0);

            //Different Days - Error Count 1
            DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDayHelper(firstDate, secondDate.AddDays(1), 1);
        }

        /// <summary>
        /// DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <param name="expectedErrorCount"></param>
        public void DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDayHelper(DateTime? firstDate, DateTime? secondDate, int expectedErrorCount)
        {
            DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay rule = new DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay();

            ExecuteRule(rule, expectedErrorCount, firstDate, secondDate);
        }

        #region DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheck

        [Test]
        public void DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheckTestPass()
        {
            DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheck rule = new DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheck();
            IDebtCounsellingRepository dcRepo = _mockery.StrictMock<IDebtCounsellingRepository>();
            int debtCounsellingKey = 1;
            List<int> hearingAppearanceTypeKeys = new List<int>();
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.CourtApplication);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalCourtApplication);
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), dcRepo);
            SetupResult.For(dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(debtCounsellingKey, (int)HearingTypes.Court, hearingAppearanceTypeKeys)).IgnoreArguments().Return(false);
            ExecuteRule(rule, 0, debtCounsellingKey);
        }

        [Test]
        public void DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheckTestFail()
        {
            DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheck rule = new DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheck();
            IDebtCounsellingRepository dcRepo = _mockery.StrictMock<IDebtCounsellingRepository>();
            int debtCounsellingKey = 1;
            List<int> hearingAppearanceTypeKeys = new List<int>();
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.CourtApplication);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalCourtApplication);
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), dcRepo);
            SetupResult.For(dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(debtCounsellingKey, (int)HearingTypes.Court, hearingAppearanceTypeKeys)).IgnoreArguments().Return(true);
            ExecuteRule(rule, 1, debtCounsellingKey);
        }

        #endregion DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheck

        #region DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheck

        [Test]
        public void DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheckTestPass()
        {
            DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheck rule = new DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheck();
            IDebtCounsellingRepository dcRepo = _mockery.StrictMock<IDebtCounsellingRepository>();
            int debtCounsellingKey = 1;

            List<int> hearingAppearanceTypeKeys = new List<int>();
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.CourtApplication);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalCourtApplication);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.OrderGranted);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalOrderGranted);

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), dcRepo);
            SetupResult.For(dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(debtCounsellingKey, (int)HearingTypes.Court, hearingAppearanceTypeKeys)).IgnoreArguments().Return(false);
            ExecuteRule(rule, 0, debtCounsellingKey);
        }

        [Test]
        public void DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheckTestFail()
        {
            DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheck rule = new DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheck();
            IDebtCounsellingRepository dcRepo = _mockery.StrictMock<IDebtCounsellingRepository>();
            int debtCounsellingKey = 1;

            List<int> hearingAppearanceTypeKeys = new List<int>();
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.CourtApplication);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalCourtApplication);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.OrderGranted);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalOrderGranted);

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), dcRepo);
            SetupResult.For(dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(debtCounsellingKey, (int)HearingTypes.Court, hearingAppearanceTypeKeys)).IgnoreArguments().Return(true);
            ExecuteRule(rule, 1, debtCounsellingKey);
        }

        #endregion DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheck

        #region 10 Day Termination Reminder

        /// <summary>
        /// We Really need to look at these rules
        /// All these termination/reminder timer rules should not be done like this
        /// Suggestion : Pass in the types you want to check and the debt counselling key. Hard coding the types you want to test in the rules are killing these rules' reuse
        /// </summary>
        /// TODO : Fix these rules as well as these tests
        private void DebtCounsellingTerminationReminderHelper(HearingTypes hearingType, HearingAppearanceTypes hearingAppearanceType, GeneralStatuses generalStatus, int expectedErrorCount)
        {
            DebtCounselling10DayTerminationReminderHearingAppearanceTypesCheck rule = new DebtCounselling10DayTerminationReminderHearingAppearanceTypesCheck();

            IDebtCounselling debtCounsellingCase = _mockery.StrictMock<IDebtCounselling>();
            IGeneralStatus hearingDetailGeneralStatus = _mockery.StrictMock<IGeneralStatus>();
            IEventList<IHearingDetail> hearingDetails = null;

            IHearingDetail debtCounsellingHearingDetail = _mockery.StrictMock<IHearingDetail>();
            IHearingType debtCounsellingHearingType = _mockery.StrictMock<IHearingType>();
            IHearingAppearanceType debtCounsellingHearingAppearanceType = _mockery.StrictMock<IHearingAppearanceType>();

            hearingDetails = new EventList<IHearingDetail>(new List<IHearingDetail> { debtCounsellingHearingDetail });

            SetupResult.For(hearingDetailGeneralStatus.Key).Return((int)generalStatus);
            SetupResult.For(debtCounsellingHearingType.Key).Return((int)hearingType);
            SetupResult.For(debtCounsellingHearingDetail.GeneralStatus).Return(hearingDetailGeneralStatus);
            SetupResult.For(debtCounsellingHearingDetail.HearingType).Return(debtCounsellingHearingType);
            SetupResult.For(debtCounsellingCase.HearingDetails).Return(hearingDetails);
            SetupResult.For(debtCounsellingHearingDetail.HearingAppearanceType).Return(debtCounsellingHearingAppearanceType);
            SetupResult.For(debtCounsellingHearingAppearanceType.Key).Return((int)hearingAppearanceType);

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ExecuteRule(rule, expectedErrorCount, debtCounsellingCase);
        }

        /// <summary>
        /// Pass
        /// </summary>
        //[Ignore("Move to Framework Spec Test")]
        [Test]
        public void DebtCounselling10TerminationReminderHearingPass()
        {
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.Appeal, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.AppealDeclined, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.AppealGranted, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.AppealPostponed, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.CourtApplicationPostponed, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.TribunalAppeal, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.TribunalAppealDeclined, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.TribunalAppealGranted, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.TribunalAppealPostponed, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.TribunalCourtApplicationPostponed, GeneralStatuses.Active, 0);

            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.Appeal, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.AppealDeclined, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.AppealGranted, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.AppealPostponed, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.CourtApplicationPostponed, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.TribunalAppeal, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.TribunalAppealDeclined, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.TribunalAppealGranted, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.TribunalAppealPostponed, GeneralStatuses.Active, 0);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.TribunalCourtApplicationPostponed, GeneralStatuses.Active, 0);
        }

        /// <summary>
        /// Fail
        /// </summary>
        //[Ignore("Move to Framework Spec Test")]
        [Test]
        public void DebtCounselling10TerminationReminderHearingFail()
        {
            //Court
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.CourtApplication, GeneralStatuses.Active, 1);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.TribunalCourtApplication, GeneralStatuses.Active, 1);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.OrderGranted, GeneralStatuses.Active, 1);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Court, HearingAppearanceTypes.TribunalOrderGranted, GeneralStatuses.Active, 1);

            //Tribunal
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.CourtApplication, GeneralStatuses.Active, 1);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.TribunalCourtApplication, GeneralStatuses.Active, 1);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.OrderGranted, GeneralStatuses.Active, 1);
            DebtCounsellingTerminationReminderHelper(HearingTypes.Tribunal, HearingAppearanceTypes.TribunalOrderGranted, GeneralStatuses.Active, 1);
        }

        #endregion 10 Day Termination Reminder

        #region Maintain Debt Counselling Legal Entities

        [Test]
        public void RemoveLegalEntityFromDebtCounsellingCheckTestFail()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);

            using (new SessionScope(FlushAction.Never))
            {
                RemoveLegalEntityFromDebtCounsellingCheck rule = new RemoveLegalEntityFromDebtCounsellingCheck();
                string query = String.Format(@"SELECT TOP 1 dc.DebtCounsellingKey
                        FROM debtcounselling.debtcounselling AS dc (NOLOCK)
                        JOIN dbo.ExternalRole er (NOLOCK) ON dc.DebtCounsellingKey = er.GenericKey
                            AND er.GenericKeyTypeKey = 27
                            AND er.ExternalRoleTypeKey = 1
                            AND er.GeneralStatusKey = 1
                        JOIN [Role] r (NOLOCK) ON er.LegalEntityKey = r.LegalEntityKey
	                        AND dc.AccountKey = r.AccountKey
	                        AND r.GeneralStatusKey = 1
	                        AND r.RoleTypeKey IN (2, 3)
                        GROUP BY dc.DebtCounsellingKey
                        HAVING COUNT(er.GenericKey) = 1
                        ORDER BY dc.DebtCounsellingKey DESC");

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                int dcKey = Convert.ToInt32(obj);
                if (dcKey > 0)
                {
                    IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                    IDebtCounselling dc = dcRepo.GetDebtCounsellingByKey(dcKey);
                    IList<ILegalEntity> clients = dc.Clients;
                    if (clients != null && clients.Count == 1)
                        ExecuteRule(rule, 1, dcKey, clients[0].Key);
                }
            }
        }

        [Test]
        public void RemoveLegalEntityFromDebtCounsellingCheckTestPass()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);

            using (new SessionScope(FlushAction.Never))
            {
                RemoveLegalEntityFromDebtCounsellingCheck rule = new RemoveLegalEntityFromDebtCounsellingCheck();
                string query = String.Format(@"SELECT TOP 1 dc.DebtCounsellingKey
                        FROM debtcounselling.debtcounselling AS dc (NOLOCK)
                        JOIN dbo.ExternalRole er (NOLOCK) ON dc.DebtCounsellingKey = er.GenericKey
                            AND er.GenericKeyTypeKey = 27
                            AND er.ExternalRoleTypeKey = 1
                            AND er.GeneralStatusKey = 1
                        JOIN [Role] r (NOLOCK) ON er.LegalEntityKey = r.LegalEntityKey
	                        AND dc.AccountKey = r.AccountKey
	                        AND r.GeneralStatusKey = 1
	                        AND r.RoleTypeKey IN (2, 3)
                        GROUP BY dc.DebtCounsellingKey
                        HAVING COUNT(er.GenericKey) > 1
                        ORDER BY dc.DebtCounsellingKey DESC");

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                int dcKey = Convert.ToInt32(obj);
                if (dcKey > 0)
                {
                    IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                    IDebtCounselling dc = dcRepo.GetDebtCounsellingByKey(dcKey);
                    IList<ILegalEntity> clients = dc.Clients;
                    if (clients != null && clients.Count > 1)
                        ExecuteRule(rule, 0, dcKey, clients[0].Key);
                }
            }
        }

        #endregion Maintain Debt Counselling Legal Entities

        /// <summary>
        /// Debt Counselling Proposal Decline Reasons Pass
        /// </summary>
        [Test]
        public void DebtCounsellingProposalDeclineReasons()
        {
            //If there are decline reasons, there are no error messages
            DebtCounsellingProposalDeclineWithReasonsHelper(true, 0);

            //If there are no decline reasons, there are error message
            DebtCounsellingProposalDeclineWithReasonsHelper(false, 1);
        }

        /// <summary>
        /// Debt Counsealling Proposal
        /// </summary>
        /// <param name="hasCount"></param>
        /// <param name="expectedMessageCount"></param>
        private void DebtCounsellingProposalDeclineWithReasonsHelper(bool hasCount, int expectedMessageCount)
        {
            var rule = new DebtCounsellingProposalDeclineWithReasons();

            //Setup the repository
            IStageDefinitionRepository stageDefinitionRepository = _mockery.StrictMock<IStageDefinitionRepository>();
            MockCache.Add(typeof(IStageDefinitionRepository).ToString(), stageDefinitionRepository);

            IDebtCounselling debtCounsellingCase = _mockery.StrictMock<IDebtCounselling>();
            IStageTransition stageTransition = _mockery.StrictMock<IStageTransition>();
            IList<IStageTransition> stageTransitions = new List<IStageTransition>();
            if (hasCount)
            {
                stageTransitions.Add(stageTransition);
            }

            SetupResult.For(debtCounsellingCase.Key).Return(0);
            SetupResult.For(stageDefinitionRepository.GetStageTransitionList(0, 0, null)).IgnoreArguments().Return(stageTransitions);

            ExecuteRule(rule, expectedMessageCount, debtCounsellingCase);
        }

        /// <summary>
        /// Debt Counselling Court Order With Appeal Exist
        /// </summary>
        [Test]
        public void DebtCounsellingCourtOrderWithAppealExist()
        {
            //Has Transitions with Court Order With Appeal
            DebtCounsellingCourtOrderWithAppealExistHelper(1, true, ReasonDescriptions.CourtOrderWithAppeal);

            //Has Transitions with a Decline Reason
            DebtCounsellingCourtOrderWithAppealExistHelper(0, true, ReasonDescriptions.CourtOrderWithAcceptance);

            //Doesn't have Transitions and it doesn't matter what decline reason it has
            DebtCounsellingCourtOrderWithAppealExistHelper(0, false, ReasonDescriptions.CourtOrderWithAppeal);
        }

        /// <summary>
        /// Debt Counselling Court Order With Appeal Exist Helper
        /// </summary>
        private void DebtCounsellingCourtOrderWithAppealExistHelper(int expectedMessageCount, bool hasCount, ReasonDescriptions reasonDescriptions)
        {
            var rule = new DebtCounsellingCourtOrderWithAppealExist();

            //Setup the repository
            var stageDefinitionRepository = _mockery.StrictMock<IStageDefinitionRepository>();
            var reasonRepository = _mockery.StrictMock<IReasonRepository>();

            MockCache.Add(typeof(IStageDefinitionRepository).ToString(), stageDefinitionRepository);
            MockCache.Add(typeof(IReasonRepository).ToString(), reasonRepository);

            var debtCounsellingCase = _mockery.StrictMock<IDebtCounselling>();
            var stageTransition = _mockery.StrictMock<IStageTransition>();
            var reason = _mockery.StrictMock<IReason>();
            var reasonDefinition = _mockery.StrictMock<IReasonDefinition>();
            var reasonDescription = _mockery.StrictMock<IReasonDescription>();

            var stageTransitions = new List<IStageTransition>();
            if (hasCount)
            {
                stageTransitions.Add(stageTransition);
            }

            var reasonsList = new ReadOnlyEventList<IReason>(new IReason[] { reason });

            SetupResult.For(reason.ReasonDefinition).Return(reasonDefinition);
            SetupResult.For(reasonDefinition.ReasonDescription).Return(reasonDescription);
            SetupResult.For(reasonDescription.Key).Return((int)reasonDescriptions);
            SetupResult.For(debtCounsellingCase.Key).Return(0);
            SetupResult.For(reasonRepository.GetReasonsByStageTransitionKeys(new int[] { 0 })).Return(reasonsList);
            SetupResult.For(stageTransition.TransitionDate).Return(DateTime.Now);
            SetupResult.For(stageTransition.Key).Return(0);
            SetupResult.For(reason.StageTransition).Return(stageTransition);
            SetupResult.For(stageDefinitionRepository.GetStageTransitionList(0, 0, null)).IgnoreArguments().Return(stageTransitions);

            ExecuteRule(rule, expectedMessageCount, debtCounsellingCase);
        }

        /// <summary>
        /// Debt Counselling Court Order With Appeal Exist
        /// </summary>
        [Test]
        public void DebtCounsellingLatestTransitionIsCourtOrderWithAppeal()
        {
            //Latest Transition is a Court Order With Appeal
            DebtCounsellingLatestTransitionIsCourtOrderWithAppealHelper(1, true, ReasonDescriptions.CourtOrderWithAppeal);

            //Latest Transition is not a Court Order With Appeal
            DebtCounsellingLatestTransitionIsCourtOrderWithAppealHelper(0, true, ReasonDescriptions.CourtOrderWithAcceptance);

            //No Transitions
            DebtCounsellingLatestTransitionIsCourtOrderWithAppealHelper(0, false, ReasonDescriptions.CourtOrderWithAppeal);
        }

        /// <summary>
        /// Debt Counselling Court Order With Appeal Exist Helper
        /// </summary>
        private void DebtCounsellingLatestTransitionIsCourtOrderWithAppealHelper(int expectedMessageCount, bool hasCount, ReasonDescriptions reasonDescriptions)
        {
            var rule = new DebtCounsellingLatestTransitionIsCourtOrderWithAppeal();

            //Setup the repository
            var stageDefinitionRepository = _mockery.StrictMock<IStageDefinitionRepository>();
            var reasonRepository = _mockery.StrictMock<IReasonRepository>();

            MockCache.Add(typeof(IStageDefinitionRepository).ToString(), stageDefinitionRepository);
            MockCache.Add(typeof(IReasonRepository).ToString(), reasonRepository);

            var debtCounsellingCase = _mockery.StrictMock<IDebtCounselling>();
            var stageTransition = _mockery.StrictMock<IStageTransition>();
            var reason = _mockery.StrictMock<IReason>();
            var reasonDefinition = _mockery.StrictMock<IReasonDefinition>();
            var reasonDescription = _mockery.StrictMock<IReasonDescription>();

            var stageTransitions = new List<IStageTransition>();
            if (hasCount)
            {
                stageTransitions.Add(stageTransition);
            }

            var reasonsList = new ReadOnlyEventList<IReason>(new IReason[] { reason });

            SetupResult.For(reason.ReasonDefinition).Return(reasonDefinition);
            SetupResult.For(reasonDefinition.ReasonDescription).Return(reasonDescription);
            SetupResult.For(reasonDescription.Key).Return((int)reasonDescriptions);
            SetupResult.For(debtCounsellingCase.Key).Return(0);
            SetupResult.For(reasonRepository.GetReasonsByStageTransitionKeys(new int[] { 0 })).Return(reasonsList);
            SetupResult.For(stageTransition.TransitionDate).Return(DateTime.Now);
            SetupResult.For(stageTransition.Key).Return(0);
            SetupResult.For(reason.StageTransition).Return(stageTransition);
            SetupResult.For(stageDefinitionRepository.GetStageTransitionsByGenericKey(0)).IgnoreArguments().Return(stageTransitions);

            ExecuteRule(rule, expectedMessageCount, debtCounsellingCase);
        }

        [Test]
        public void DuplicateDraftSavePass()
        {
            //inputs
            DebtCounsellingDuplicateDraftProposalHelper(ProposalStatuses.Active, ProposalTypes.Proposal, 0);
            DebtCounsellingDuplicateDraftProposalHelper(ProposalStatuses.Inactive, ProposalTypes.Proposal, 0);
            DebtCounsellingDuplicateDraftProposalHelper(ProposalStatuses.Active, ProposalTypes.CounterProposal, 0);
            DebtCounsellingDuplicateDraftProposalHelper(ProposalStatuses.Inactive, ProposalTypes.CounterProposal, 0);
        }

        [Test]
        public void DuplicateDraftSaveFail()
        {
            //inputs
            DebtCounsellingDuplicateDraftProposalHelper(ProposalStatuses.Draft, ProposalTypes.CounterProposal, 1);
            DebtCounsellingDuplicateDraftProposalHelper(ProposalStatuses.Draft, ProposalTypes.Proposal, 1);
        }

        private void DebtCounsellingDuplicateDraftProposalHelper(ProposalStatuses psKey, ProposalTypes ptKey, int msgCount)
        {
            DebtCounsellingDuplicateDraftProposal rule = new DebtCounsellingDuplicateDraftProposal();

            //Setup the repository
            IDebtCounsellingRepository dcRepo = _mockery.StrictMock<IDebtCounsellingRepository>();

            MockCache.Add(typeof(IDebtCounsellingRepository).ToString(), dcRepo);

            IProposalStatus ps = _mockery.StrictMock<IProposalStatus>();
            SetupResult.For(ps.Key).Return((int)psKey);

            IProposalType pt = _mockery.StrictMock<IProposalType>();
            SetupResult.For(pt.Key).Return((int)ptKey);
            SetupResult.For(pt.Description).Return("Proposal Type");

            IDebtCounselling dc = _mockery.StrictMock<IDebtCounselling>();
            SetupResult.For(dc.Key).Return(1);

            IProposal p = _mockery.StrictMock<IProposal>();
            SetupResult.For(p.Key).Return(1);
            SetupResult.For(p.DebtCounselling).Return(dc);
            SetupResult.For(p.ProposalStatus).Return(ps);
            SetupResult.For(p.ProposalType).Return(pt);

            IProposal p1 = _mockery.StrictMock<IProposal>();
            SetupResult.For(p1.Key).Return(2);

            List<IProposal> listP = new List<IProposal>();
            listP.Add(p);
            listP.Add(p1);

            SetupResult.For(dcRepo.GetProposalsByTypeAndStatus(1, ProposalTypes.CounterProposal, ProposalStatuses.Draft)).IgnoreArguments().Return(listP);

            ExecuteRule(rule, msgCount, p);
        }

        /// <summary>
        /// Legal Entities Under Debt Counselling For Account Test
        /// </summary>LegalEntitiesUnderDebtCounsellingForAccount
        [Test]
        public void LegalEntitiesUnderDebtcounsellingForAccountTest()
        {
            LegalEntitiesUnderDebtCounsellingForAccountHelper(true, true, 1);
            LegalEntitiesUnderDebtCounsellingForAccountHelper(true, false, 0);
            LegalEntitiesUnderDebtCounsellingForAccountHelper(false, false, 0);
            LegalEntitiesUnderDebtCounsellingForAccountHelper(false, true, 0);
        }

        /// <summary>
        /// Legal Entities Under Debt Counselling For Account
        /// </summary>
        private void LegalEntitiesUnderDebtCounsellingForAccountHelper(bool hasDebtCounsellingCases, bool roleGeneralStatus, int expectedErrorCount)
        {
            var rule = new LegalEntitiesUnderDebtCounsellingForAccount();
            IAccount account = _mockery.StrictMock<IAccount>();
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            IRole role = _mockery.StrictMock<IRole>();
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IDebtCounselling debtCounsellingCase = _mockery.StrictMock<IDebtCounselling>();

            IEventList<IRole> roles = new EventList<IRole>(new[]{
				role
			});
            IEventList<IDebtCounselling> debtCounsellingCases = new EventList<IDebtCounselling>(new[]{
				debtCounsellingCase
			});

            SetupResult.For(account.Key).Return(1);
            SetupResult.For(debtCounsellingCase.Account).Return(account);
            SetupResult.For(account.Roles).Return(roles);
            SetupResult.For(generalStatus.Key).Return((int)(roleGeneralStatus ? GeneralStatuses.Active : GeneralStatuses.Inactive));
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(roleType.Description).Return("RoleTypeDescription");
            SetupResult.For(legalEntity.DisplayName).Return("DisplayName");
            SetupResult.For(legalEntity.DebtCounsellingCases).Return(hasDebtCounsellingCases ? debtCounsellingCases : null);

            ExecuteRule(rule, expectedErrorCount, account);
        }

        /// <summary>
        /// Legal Entities Under Debt Counselling For Account Test
        /// </summary>
        [Test]
        public void LegalEntitiesUnderDebtcounsellingForApplicationTest()
        {
            LegalEntitiesUnderDebtCounsellingForApplicationHelper(true, true, 1);
            LegalEntitiesUnderDebtCounsellingForApplicationHelper(true, false, 0);
            LegalEntitiesUnderDebtCounsellingForApplicationHelper(false, false, 0);
            LegalEntitiesUnderDebtCounsellingForApplicationHelper(false, true, 0);
        }

        /// <summary>
        /// Legal Entities Under Debt Counselling For Account
        /// </summary>
        private void LegalEntitiesUnderDebtCounsellingForApplicationHelper(bool hasDebtCounsellingCases, bool roleGeneralStatus, int expectedErrorCount)
        {
            var rule = new LegalEntitiesUnderDebtCounsellingForAccount();
            IApplication application = _mockery.StrictMock<IApplication>();
            IApplicationRole role = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType roleType = _mockery.StrictMock<IApplicationRoleType>();
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IDebtCounselling debtCounsellingCase = _mockery.StrictMock<IDebtCounselling>();
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();

            IReadOnlyEventList<IApplicationRole> roles = new ReadOnlyEventList<IApplicationRole>(new[]{
				role
			});
            IEventList<IDebtCounselling> debtCounsellingCases = new EventList<IDebtCounselling>(new[]{
				debtCounsellingCase
			});

            SetupResult.For(application.Key).Return(1);
            SetupResult.For(application.ApplicationRoles).Return(roles);
            SetupResult.For(generalStatus.Key).Return((int)(roleGeneralStatus ? GeneralStatuses.Active : GeneralStatuses.Inactive));
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            SetupResult.For(role.ApplicationRoleType).Return(roleType);
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(roleType.Description).Return("RoleTypeDescription");
            SetupResult.For(legalEntity.DisplayName).Return("DisplayName");
            SetupResult.For(legalEntity.DebtCounsellingCases).Return(hasDebtCounsellingCases ? debtCounsellingCases : null);

            ExecuteRule(rule, expectedErrorCount, application);
        }

        [Test]
        public void LegalEntityUnderDebtCounsellingTest()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);

            var query = @"select top 1 le.LegalEntityKey
                        from [2am]..LegalEntity le (nolock)
                        join [2am]..ExternalRole er (nolock) on le.LegalEntityKey = er.LegalEntityKey and er.GenericKeyTypeKey = 27 --DebtCounselling
                        join [2am].debtcounselling.DebtCounselling dc (nolock) on er.GenericKey = dc.DebtCounsellingKey
                        where er.GeneralStatusKey = 1 and dc.DebtCounsellingStatusKey = 1 and er.ExternalRoleTypeKey = 1
                        order by le.LegalEntityKey desc";

            using (new SessionScope())
            {
                var obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                var legalEntityKeyUnderDebtCounselling = Convert.ToInt32(obj);

                var legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                var legalEntity = legalEntityRepo.GetLegalEntityByKey(legalEntityKeyUnderDebtCounselling);

                var rule = new LegalEntityUnderDebtCounselling();

                int expectedMessageCount = 1;

                ExecuteRule(rule, expectedMessageCount, legalEntity);
            }
        }

        // Find a DebtCounsellor that doesnt have an active address linked to them but their parent does have an active address.
        const string DebtCounsellorActiveAddressRequiredQUERY = @"

                            declare @temp table (DCLEKey int, DCKey int, DCParentLEKey int)
                            insert into @temp
                            select distinct er.LegalEntityKey, er.GenericKey, plea.LegalEntityKey
                            from [2AM].dbo.ExternalRole (nolock) er
                            left join [2AM].dbo.LegalEntityAddress (nolock) lea on er.LegalEntityKey = lea.LegalEntityKey 
                            join [2AM].dbo.LegalEntityOrganisationStructure (nolock) leos on leos.LegalEntityKey = er.LegalEntityKey
                            join [2AM].dbo.OrganisationStructure (nolock) os on leos.OrganisationStructureKey = os.OrganisationStructureKey
                            join [2AM].dbo.OrganisationStructure pos (nolock) on pos.OrganisationStructurekey = os.parentkey
                            join [2AM].dbo.LegalEntityOrganisationStructure pleos (NOLOCK) on pleos.OrganisationStructurekey = pos.OrganisationStructurekey 
                            join [2AM].dbo.LegalEntity ple (nolock) ON ple.LegalEntityKey = pleos.LegalEntityKey
                            join [2am].[debtcounselling].[DebtCounsellorDetail] dcd (nolock) on dcd.LegalEntityKey = ple.LegalEntityKey
                            join [2AM].dbo.LegalEntityAddress (nolock) plea on ple.LegalEntityKey = plea.LegalEntityKey 
                            where lea.LegalEntityAddressKey is null
                              and er.GeneralStatusKey = 1 and er.ExternalRoleTypeKey = 2 and er.GenericKeyTypeKey = 27
                              and plea.GeneralStatusKey = 1
                            order by er.LegalEntityKey

                            declare @DCLEKey int,
		                            @DCKey int,
		                            @DCParentLEKey int
		
                            select top 1 DCLEKey, DCKey, DCParentLEKey from @temp";

        // Find a DebtCounsellor that doesnt have an active address linked to them and their parent does not have an active address.
        const string DebtCounsellorNoActiveAddressQUERY = @"

                            declare @temp table (DCLEKey int, DCKey int, DCParentLEKey int)
                            insert into @temp
                            select distinct er.LegalEntityKey, er.GenericKey, plea.LegalEntityKey
                            from [2AM].dbo.ExternalRole (nolock) er
                            left join [2AM].dbo.LegalEntityAddress (nolock) lea on er.LegalEntityKey = lea.LegalEntityKey 
                            join [2AM].dbo.LegalEntityOrganisationStructure (nolock) leos on leos.LegalEntityKey = er.LegalEntityKey
                            join [2AM].dbo.OrganisationStructure (nolock) os on leos.OrganisationStructureKey = os.OrganisationStructureKey
                            join [2AM].dbo.OrganisationStructure pos (nolock) on pos.OrganisationStructurekey = os.parentkey
                            join [2AM].dbo.LegalEntityOrganisationStructure pleos (NOLOCK) on pleos.OrganisationStructurekey = pos.OrganisationStructurekey 
                            join [2AM].dbo.LegalEntity ple (nolock) ON ple.LegalEntityKey = pleos.LegalEntityKey
                            join [2am].[debtcounselling].[DebtCounsellorDetail] dcd (nolock) on dcd.LegalEntityKey = ple.LegalEntityKey
                            left join [2AM].dbo.LegalEntityAddress (nolock) plea on ple.LegalEntityKey = plea.LegalEntityKey 
                            where lea.LegalEntityAddressKey is null
                              and er.GeneralStatusKey = 1 and er.ExternalRoleTypeKey = 2 and er.GenericKeyTypeKey = 27
                            order by er.LegalEntityKey

                            declare @DCLEKey int,
		                            @DCKey int,
		                            @DCParentLEKey int
		
                            select top 1 DCLEKey, DCKey, DCParentLEKey from @temp";

        [Test]
        public void DebtCounsellorActiveAddressRequired_DebtCounsellorHasNoActiveAddressButParentDoes()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);

            using (new SessionScope())
            {
                var dt = base.GetQueryResults(DebtCounsellorActiveAddressRequiredQUERY);

                if (dt.Rows.Count < 1) Assert.Ignore("No Data Found for test.");

                //int dcLEKey = Convert.ToInt32(dt.Rows[0][0]);
                int dcKey = Convert.ToInt32(dt.Rows[0][1]);
                //int dcParentLEKey = Convert.ToInt32(dt.Rows[0][2]);

                var dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                var dc = dcRepo.GetDebtCounsellingByKey(dcKey);

                var rule = new DebtCounsellorActiveAddressRequired(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                int expectedMessageCount = 0;

                ExecuteRule(rule, expectedMessageCount, dc);
            }
        }


        [Test]
        public void DebtCounsellorActiveAddressRequired_DebtCounsellorHasNoActiveAddressANDParentHasNoActiveAddress()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Default);

            using (var session = new TransactionScope(OnDispose.Rollback))
            {
                var dt = base.GetQueryResults(DebtCounsellorNoActiveAddressQUERY);

                if (dt.Rows.Count < 1) Assert.Ignore("No Data Found for test.");

                int dcLEKey = Convert.ToInt32(dt.Rows[0][0]);
                int dcKey = Convert.ToInt32(dt.Rows[0][1]);
                int dcParentLEKey = Convert.ToInt32(dt.Rows[0][2]);

                var dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                var dc = dcRepo.GetDebtCounsellingByKey(dcKey);

                var updateQuery = string.Format(@"  update [2AM].dbo.LegalEntityAddress
                                                    set GeneralStatusKey = 2
                                                    where LegalEntityKey = {0}", dcParentLEKey);

                CastleTransactionsServiceHelper.ExecuteNonQueryOnCastleTran(updateQuery, typeof(GeneralStatus_DAO), null);

                var rule = new DebtCounsellorActiveAddressRequired(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                int expectedMessageCount = 1;

                ExecuteRule(rule, expectedMessageCount, dc);
            }
        }
    }
}