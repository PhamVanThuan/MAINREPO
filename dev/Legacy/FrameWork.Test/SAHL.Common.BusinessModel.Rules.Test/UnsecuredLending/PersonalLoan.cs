using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.CacheData;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Rules.Test.UnsecuredLending
{
    [TestFixture]
    public class PersonalLoan : RuleBase
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

        [Test]
        public void UnsecuredLendingConfirmIncomeTestFail()
        {
            IApplicationUnsecuredLending applicationUnsecuredLending = _mockery.StrictMock<IApplicationUnsecuredLending>();
            IReadOnlyEventList<IExternalRole> externalRoles = new ReadOnlyEventList<IExternalRole>();
            SetupResult.For(applicationUnsecuredLending.ActiveClientRoles).Return(externalRoles);
            UnsecuredLendingConfirmIncome rule = new UnsecuredLendingConfirmIncome();
            ExecuteRule(rule, 1, applicationUnsecuredLending);
        }

        [Test]
        public void UnsecuredLendingConfirmIncomeTestPass()
        {
            // Mock a LegalEntity and Employments
            IEmploymentStatus employmentStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(employmentStatus.Key).Return((int)EmploymentStatuses.Current);

            IEmployment employment = _mockery.StrictMock<IEmployment>();
            SetupResult.For(employment.EmploymentStatus).Return(employmentStatus);
            SetupResult.For(employment.IsConfirmed).Return(true);
            SetupResult.For(employment.ConfirmedIncomeFlag).Return(true);

            EventList<IEmployment> employments = new EventList<IEmployment>();
            employments.Add(null, employment);

            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(legalEntity.Employment).Return(employments);

            // Setup the ExternalRole

            IExternalRoleType externalRoleType = _mockery.StrictMock<IExternalRoleType>();
            SetupResult.For(externalRoleType.Key).Return((int)ExternalRoleTypes.Client);

            IExternalRole externalRole = _mockery.StrictMock<IExternalRole>();
            SetupResult.For(externalRole.ExternalRoleType).Return(externalRoleType);
            SetupResult.For(externalRole.LegalEntity).Return(legalEntity);

            // Setup ExternalRoles Collection
            var list = new List<IExternalRole>() { externalRole };
            ReadOnlyEventList<IExternalRole> externalRoles = new ReadOnlyEventList<IExternalRole>(list);

            // Setup the IApplicationUnsecuredLending
            IApplicationUnsecuredLending applicationUnsecuredLending = _mockery.StrictMock<IApplicationUnsecuredLending>();
            SetupResult.For(applicationUnsecuredLending.ActiveClientRoles).Return(externalRoles);

            UnsecuredLendingConfirmIncome rule = new UnsecuredLendingConfirmIncome();
            ExecuteRule(rule, 0, applicationUnsecuredLending);
        }

        [Test]
        public void CheckCanEmailPersonalLoanApplicationWhenCorrespondenceTypeEmail()
        {
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Default);

            // Get a personal loans offer that has a correspondence type of post
            var query = @"select top 1 oma.OfferKey
                          from OfferMailingAddress oma
                          join Offer o on oma.OfferKey = o.OfferKey
                          join LegalEntity le on le.LegalEntityKey = oma.LegalEntityKey
                          where oma.CorrespondenceMediumKey = 2 and o.OfferTypeKey = 11";

            using (var session = new TransactionScope())
            {
                var datatable = base.GetQueryResults(query);

                if (datatable.Rows.Count != 1)
                    Assert.Ignore("No Data Found for test.");

                var applicationKey = Convert.ToInt32(datatable.Rows[0][0]);

                var applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                var rule = new CheckCanEmailPersonalLoanApplication(applicationRepo);

                ExecuteRule(rule, 0, new object[] { applicationKey });
            }
        }

        [Test]
        public void CheckCanEmailPersonalLoanApplicationWhenCorrespondenceTypePost()
        {
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Default);

            // Get a personal loans offer that has a correspondence type of post
            var query = @"select oma.OfferKey
                          from OfferMailingAddress oma
                          join Offer o on oma.OfferKey = o.OfferKey
                          where oma.CorrespondenceMediumKey = 1 and o.OfferTypeKey = 11";

            using (var session = new TransactionScope())
            {
                var datatable = base.GetQueryResults(query);

                if (datatable.Rows.Count < 1)
                    Assert.Ignore("No Data Found for test.");

                var applicationKey = Convert.ToInt32(datatable.Rows[0][0]);

                var applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                var rule = new CheckCanEmailPersonalLoanApplication(applicationRepo);

                // We expect the rule to fail
                ExecuteRule(rule, 1, new object[] { applicationKey });
            }
        }

        [Test]
        public void CheckUnderDebtReviewDeclarationRule_Pass()
        {
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Default);

            // Active Client, Unsecured Lending, Open, DebtReviewDeclarationAnswer = No 
            string query = @"select top 1 o.OfferKey
                            from dbo.Offer o (nolock)
                            join dbo.ExternalRole erl (nolock) on erl.GenericKey = o.OfferKey
	                            and erl.GenericKeyTypeKey = 2
	                            and erl.GeneralStatusKey = 1
	                            and erl.ExternalRoleTypeKey = 1
                            join dbo.ExternalRoleDeclaration erd1 (nolock) on erd1.ExternalRoleKey = erl.ExternalRoleKey
                                and erd1.OfferDeclarationQuestionKey = 5
                                and erd1.OfferDeclarationAnswerKey = 2
                            join dbo.ExternalRoleDeclaration erd2 (nolock) on erd2.ExternalRoleKey = erl.ExternalRoleKey
                                and erd2.OfferDeclarationQuestionKey = 6
                                and erd2.OfferDeclarationAnswerKey = 2 
                            where o.OfferTypeKey = 11 ";

            using (var session = new TransactionScope())
            {
                var datatable = base.GetQueryResults(query);

                if (datatable.Rows.Count < 1)
                    Assert.Ignore("No Data Found for test.");

                int applicationKey = Convert.ToInt32(datatable.Rows[0][0]);

                IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplicationUnsecuredLending app = (IApplicationUnsecuredLending)applicationRepo.GetApplicationByKey(applicationKey);

                UnderDebtReviewDeclaration rule = new UnderDebtReviewDeclaration(applicationRepo);

                // We expect the rule to pass
                ExecuteRule(rule, 0, new object[] { app });
            }
        }

        [Test]
        public void CheckUnderDebtReviewDeclarationRule_Fail1()
        {
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Default);
            // Active Client, Unsecured Lending, Open, DebtReviewDeclarationAnswer = Yes 
            string query = @"select top 1 o.OfferKey
                            from dbo.Offer o (nolock)
                            join dbo.ExternalRole erl (nolock) on erl.GenericKey = o.OfferKey
                                and erl.GenericKeyTypeKey = 2
                                and erl.GeneralStatusKey = 1
                                and erl.ExternalRoleTypeKey = 1
                            join dbo.ExternalRoleDeclaration erd1 (nolock) on erd1.ExternalRoleKey = erl.ExternalRoleKey
                                and erd1.OfferDeclarationQuestionKey in (5, 6)
                                and erd1.OfferDeclarationAnswerKey = 1 
                            where o.OfferTypeKey = 11 ";

            using (var session = new TransactionScope())
            {
                var datatable = base.GetQueryResults(query);

                if (datatable.Rows.Count < 1)
                    Assert.Ignore("No Data Found for test.");

                int applicationKey = Convert.ToInt32(datatable.Rows[0][0]);

                IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplicationUnsecuredLending app = (IApplicationUnsecuredLending)applicationRepo.GetApplicationByKey(applicationKey);

                UnderDebtReviewDeclaration rule = new UnderDebtReviewDeclaration(applicationRepo);

                // We expect the rule to fail
                ExecuteRule(rule, 1, new object[] { app });
            }
        }

        [Test]
        public void CheckUnderDebtReviewDeclarationRule_Fail2()
        {
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Default);
            // Active Client, Unsecured Lending, Open, No ExternalRoleDeclaration Answers
            string query = @"select top 1 o.OfferKey
                            from dbo.Offer o (nolock)
                            join dbo.ExternalRole erl (nolock) on erl.GenericKey = o.OfferKey
	                            and erl.GenericKeyTypeKey = 2
	                            and erl.GeneralStatusKey = 1
	                            and erl.ExternalRoleTypeKey = 1
                            left join dbo.ExternalRoleDeclaration erd (nolock) on erd.ExternalRoleKey = erl.ExternalRoleKey
                            where o.OfferTypeKey = 11 
                            and erd.ExternalRoleDeclarationKey is null ";

            using (var session = new TransactionScope())
            {
                var datatable = base.GetQueryResults(query);

                if (datatable.Rows.Count < 1)
                    Assert.Ignore("No Data Found for test.");

                int applicationKey = Convert.ToInt32(datatable.Rows[0][0]);

                IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplicationUnsecuredLending app = (IApplicationUnsecuredLending)applicationRepo.GetApplicationByKey(applicationKey);

                UnderDebtReviewDeclaration rule = new UnderDebtReviewDeclaration(applicationRepo);

                // We expect the rule to fail
                ExecuteRule(rule, 1, new object[] { app });
            }
        }

        [Test]
        public void CheckIfCapitecClient_NoAttribute_Pass()
        {

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            ICastleTransactionsService castleTransactionsService = RepositoryFactory.GetRepository<ICastleTransactionsService>();

            using (var session = new TransactionScope(OnDispose.Rollback))
            {
                // get an open/accepted offer with a main applicant
                var query = @"select top 1 ofr.LegalEntityKey
                                from [2am]..Offer o (nolock)
                                join [2am]..OfferRole ofr (nolock) on ofr.OfferKey = o.OfferKey
								left join [2am]..OfferAttribute oa (nolock) on oa.OfferKey = o.OfferKey and oa.OfferAttributeTypeKey = 30
                                where o.OfferStatusKey in (1,3) 
                                and o.OfferTypeKey = 6
                                and ofr.OfferRoleTypeKey = 8
								and oa.OfferAttributeKey is null";

                var datatable = base.GetQueryResults(query);

                if (datatable.Rows.Count != 1)
                    Assert.Ignore("No Data Found for test.");

                var legalEntityKey = Convert.ToInt32(datatable.Rows[0][0]);

                if (legalEntityKey > 0)
                {
                    ILegalEntity legalEntity = leRepo.GetLegalEntityByKey(legalEntityKey);
                    CheckIfCapitecClient rule = new CheckIfCapitecClient(castleTransactionsService);
                    ExecuteRule(rule, 0, new object[] { legalEntity });
                }
            }
        }

        [Test]
        public void CheckIfCapitecClient_WithAttribute_Fail()
        {
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Default);

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            ICastleTransactionsService castleTransactionsService = RepositoryFactory.GetRepository<ICastleTransactionsService>();

            // get an ntu finalized offer with a main applicant
            var query = @"select top 1 o.OfferKey
                                from [2am]..Offer o (nolock)
                                join [2am]..OfferRole ofr (nolock) on ofr.OfferKey = o.OfferKey
                                where o.OfferStatusKey in (1,3) 
                                and ofr.OfferRoleTypeKey = 8";

            bool capitecApplicationAttributeInserted = false;
            int capitecApplicationKey = CreateCapitecOfferToTest(spc, query, out capitecApplicationAttributeInserted);

            using (var session = new TransactionScope(OnDispose.Commit))
            {
                // get the app again
                IApplication application = appRepo.GetApplicationByKey(capitecApplicationKey);

                ILegalEntity legalEntity = application.GetFirstApplicationRoleByType(OfferRoleTypes.LeadMainApplicant).LegalEntity;

                CheckIfCapitecClient rule = new CheckIfCapitecClient(castleTransactionsService);

                // We expect the rule to fail
                ExecuteRule(rule, 1, new object[] { legalEntity });

                // remove the capitec attribute
                if (capitecApplicationAttributeInserted)
                    RemoveCapitecApplicationAttribute(spc, application);
            }
        }

        [Test]
        public void CheckIfCapitecClient_WithAttribute_NTUFinalized_Pass()
        {
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Default);

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            ICastleTransactionsService castleTransactionsService = RepositoryFactory.GetRepository<ICastleTransactionsService>();

            // get an ntu finalized offer with a main applicant
            var query = @"select top 1 o.OfferKey, ofr.LegalEntityKey, stc.GenericKey, stc.TransitionDate, stc.StageDefinitionStageDefinitionGroupKey 
                            from [2am]..StageDefinitionStageDefinitionGroup sdsdg (nolock)
                            join[2am]..StageDefinitionStageDefinitionGroup friends (nolock) on friends.StageDefinitionGroupKey = sdsdg.StageDefinitionGroupKey
                            join [2am]..StageTransitionComposite stc (nolock) on stc.StageDefinitionStageDefinitionGroupKey = friends.StageDefinitionStageDefinitionGroupKey
                            join [2am]..Offer o (nolock) on o.OfferKey = stc.GenericKey
                            join [2am]..OfferRole ofr (nolock) on ofr.OfferKey = o.OfferKey
                            where o.OfferStatusKey in (4,5) 
                            and ofr.OfferRoleTypeKey = 8
                            and sdsdg.StageDefinitionStageDefinitionGroupKey in (110,111,1344)
                            and stc.StageDefinitionStageDefinitionGroupKey in (110,111,1344)";

            bool capitecApplicationAttributeInserted = false;
            int capitecApplicationKey = CreateCapitecOfferToTest(spc, query, out capitecApplicationAttributeInserted);

            using (var session = new TransactionScope(OnDispose.Commit))
            {
                // get the app again
                IApplication application = appRepo.GetApplicationByKey(capitecApplicationKey);

                ILegalEntity legalEntity = application.GetFirstApplicationRoleByType(OfferRoleTypes.LeadMainApplicant).LegalEntity;

                CheckIfCapitecClient rule = new CheckIfCapitecClient(castleTransactionsService);

                // We expect the rule to pass
                ExecuteRule(rule, 0, new object[] { legalEntity });

                // remove the capitec attribute
                if (capitecApplicationAttributeInserted)
                    RemoveCapitecApplicationAttribute(spc, application);
            }
        }

        [Test]
        public void CheckIfCapitecClient_WithAttribute_NTUNotFinalized_Fail()
        {
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Default);

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            ICastleTransactionsService castleTransactionsService = RepositoryFactory.GetRepository<ICastleTransactionsService>();

            // get an ntu finalized offer with a main applicant
            var query = @"select top 1 o.OfferKey, ofr.LegalEntityKey, stc.GenericKey, stc.TransitionDate, stc.StageDefinitionStageDefinitionGroupKey 
                                        from [2am]..Offer o (nolock)
                                        join [2am]..OfferRole ofr (nolock) on ofr.OfferKey = o.OfferKey
                                        left join [2am]..StageTransitionComposite stc (nolock) on stc.GenericKey = o.OfferKey
                                        where o.OfferStatusKey in (4,5) 
                                        and o.OfferTypeKey = 6
                                        and ofr.OfferRoleTypeKey = 8
                                        and stc.GenericKey is null";

            bool capitecApplicationAttributeInserted = false;
            int capitecApplicationKey = CreateCapitecOfferToTest(spc, query, out capitecApplicationAttributeInserted);

            using (var session = new TransactionScope(OnDispose.Commit))
            {
                // get the app again
                IApplication application = appRepo.GetApplicationByKey(capitecApplicationKey);

                ILegalEntity legalEntity = application.GetFirstApplicationRoleByType(OfferRoleTypes.LeadMainApplicant).LegalEntity;

                CheckIfCapitecClient rule = new CheckIfCapitecClient(castleTransactionsService);

                // We expect the rule to fail
                ExecuteRule(rule, 1, new object[] { legalEntity });

                // remove the capitec attribute
                if (capitecApplicationAttributeInserted)
                    RemoveCapitecApplicationAttribute(spc, application);
            }
        }

        private int CreateCapitecOfferToTest(SAHLPrincipalCache spc, string query, out bool capitecApplicationAttributeInserted)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication application = null;
            capitecApplicationAttributeInserted = false;

            using (var session = new TransactionScope(OnDispose.Commit))
            {
                var datatable = base.GetQueryResults(query);

                if (datatable.Rows.Count != 1)
                    Assert.Ignore("No Data Found for test.");

                var applicationKey = Convert.ToInt32(datatable.Rows[0][0]);

                application = appRepo.GetApplicationByKey(applicationKey);

                // add a capitec offer attribute to the offer
                if (!application.IsCapitec())
                {
                    IApplicationAttribute capitecApplicationAttribute = appRepo.GetEmptyApplicationAttribute();
                    capitecApplicationAttribute.ApplicationAttributeType = base.LookupRepository.ApplicationAttributesTypes.ObjectDictionary[Convert.ToString((int)OfferAttributeTypes.CapitecLoan)];
                    capitecApplicationAttribute.Application = application;
                    application.ApplicationAttributes.Add(spc.DomainMessages, capitecApplicationAttribute);

                    appRepo.SaveApplication(application);

                    capitecApplicationAttributeInserted = true;
                }
            }

            return application.Key;
        }

        private void RemoveCapitecApplicationAttribute(SAHLPrincipalCache spc, IApplication application)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationAttribute capitecApplicationAttribute = application.ApplicationAttributes.FirstOrDefault(x => x.ApplicationAttributeType.Key == (int)SAHL.Common.Globals.OfferAttributeTypes.CapitecLoan);

            application.ApplicationAttributes.Remove(spc.DomainMessages, capitecApplicationAttribute);
            spc.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);
            appRepo.SaveApplication(application);
        }
    }
}