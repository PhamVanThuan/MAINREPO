using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules.Application.Credit;
using SAHL.Common.BusinessModel.Rules.ApplicationRole;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.Application
{
    [TestFixture]
    public class ApplicationRole : RuleBase
    {
        IEventList<IApplicationDeclaration> appDecs;
        IApplicationMortgageLoan appMl;

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            appMl = _mockery.StrictMock<IApplicationMortgageLoan>();
        }

        [NUnit.Framework.Test]
        public void LegalEntityApplicantNaturalPersonDeclarationsTestNoRolesTestPass()
        {
            string sql = @" Select top 1 O.OfferKey, OR1.OfferRoleKey
                            From Offer O
	                            Left Join OfferRole OR1 On O.OfferKey = OR1.OfferKey
                            Where OR1.OfferKey is null ";

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
            if (obj != null)
            {
                int iOfferKey = Convert.ToInt32(obj);
                LegalEntityApplicantNaturalPersonDeclarations leDec = new LegalEntityApplicantNaturalPersonDeclarations(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
                SetupResult.For(appML.Key).Return(iOfferKey);

                IReadOnlyEventList<IApplicationRole> appRoles = new ReadOnlyEventList<IApplicationRole>();

                SetupResult.For(appML.ApplicationRoles).IgnoreArguments().Return(appRoles);

                ExecuteRule(leDec, 0, appML);
            }
        }

        [NUnit.Framework.Test]
        public void LegalEntityApplicantNaturalPersonDeclarationsNotMainOrSuretorTestPass()
        {
            string sql = @" select top 1 ofr.OfferKey
                                from offerrole ofr (nolock)
                                join LegalEntity le (nolock) on ofr.LegalEntityKey = le.LegalEntityKey
	                                and le.LegalEntityTypeKey = 2
                                join OfferRoleType ort (nolock) on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
	                                and ort.OfferRoleTypeGroupKey = 3
	                                and ofr.OfferRoleTypeKey != 11 and ofr.OfferRoleTypeKey != 12
                                    and ofr.OfferRoleTypeKey != 8 and ofr.OfferRoleTypeKey != 10
                                left join OfferDeclaration od (nolock) on ofr.OfferRoleKey = od.OfferRoleKey ";

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
            if (obj != null)
            {
                int iOfferKey = Convert.ToInt32(obj);

                LegalEntityApplicantNaturalPersonDeclarations leDec = new LegalEntityApplicantNaturalPersonDeclarations(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
                SetupResult.For(applicationMortgageLoan.Key).Return(iOfferKey);

                ExecuteRule(leDec, 0, applicationMortgageLoan);
            }
        }

        [NUnit.Framework.Test]
        public void LegalEntityApplicantNaturalPersonDeclarationsMainOrSuretorNoDecsTestFail()
        {
            using (new SessionScope())
            {
                string sql = @" select top 1 offerkey, count(offerkey) As Errors
                                from(
                                select od.OfferRoleKey, ofr.offerkey, ofr.LegalEntityKey
                                from offerrole ofr (nolock)
                                join LegalEntity le (nolock) on ofr.LegalEntityKey = le.LegalEntityKey
	                                and le.LegalEntityTypeKey = 2 --Natural Persons
                                join OfferRoleType ort (nolock) on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
	                                and ort.OfferRoleTypeGroupKey = 3 --clients only
	                                and ofr.OfferRoleTypeKey != 13 --no life cllients
                                left join OfferDeclaration od (nolock) on ofr.OfferRoleKey = od.OfferRoleKey
                                where od.OfferRoleKey is NULL
                                Group by od.OfferRoleKey, ofr.offerkey, ofr.LegalEntityKey) src
                                group by offerkey";

                DataSet dsOffer = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                if (dsOffer != null)
                {
                    if (dsOffer.Tables.Count > 0)
                    {
                        if (dsOffer.Tables[0].Rows.Count > 0)
                        {
                            int iOfferKey = Convert.ToInt32(dsOffer.Tables[0].Rows[0]["OfferKey"]);
                            int iErrors = Convert.ToInt32(dsOffer.Tables[0].Rows[0]["Errors"]);
                            LegalEntityApplicantNaturalPersonDeclarations leDec = new LegalEntityApplicantNaturalPersonDeclarations(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
                            SetupResult.For(applicationMortgageLoan.Key).Return(iOfferKey);

                            // Setup applicationMortgageLoan.ApplicationRoles

                            ExecuteRule(leDec, iErrors, applicationMortgageLoan);
                        }
                    }
                }
            }
        }

        [NUnit.Framework.Test]
        [Ignore("Long running test creates deadlocks and random failures")]
        public void LegalEntityApplicantNaturalPersonDeclarationsMainOrSuretorInSolventTestFail()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                //this example does not exist in real data, so manufacture it
                //insolvents always have an active administration
                string sql = @"update OfferDeclaration set OfferDeclarationDate = null where OfferDeclarationQuestionKey = 2;";
                var results = CastleTransactionsServiceHelper.ExecuteNonQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                sql = @"select top 1 r.OfferKey--,  odAdministration.*, odAdministrationDate.*
	                from OfferRole r 
                    join OfferDeclaration odInsolvent on r.OfferRoleKey = odInsolvent.OfferRoleKey
		                and odInsolvent.OfferDeclarationQuestionKey = 1 And odInsolvent.OfferDeclarationAnswerKey = 1 -- Has been insolvent
	                join OfferDeclaration odInsolventDate on r.OfferRoleKey = odInsolventDate.OfferRoleKey				-- and
		                and odInsolventDate.OfferDeclarationQuestionKey = 2 And odInsolventDate.OfferDeclarationDate is null -- no rehabilitated date exists
	                join OfferDeclaration odDebtReview on r.OfferRoleKey = odDebtReview.OfferRoleKey
		                and odDebtReview.OfferDeclarationQuestionKey = 5 and odDebtReview.OfferDeclarationAnswerKey =2 -- not under debt review
					join OfferDeclaration odAdministration on r.OfferRoleKey = odAdministration.OfferRoleKey
						and odAdministration.OfferDeclarationQuestionKey = 1 and odAdministration.OfferDeclarationAnswerKey = 1 -- is under administration
					join OfferDeclaration odAdministrationDate on r.OfferRoleKey = odAdministrationDate.OfferRoleKey			-- and 
						and odAdministrationDate.OfferDeclarationQuestionKey = 2 and odAdministrationDate.OfferDeclarationDate is null -- no rescinded date";
                DataSet dsOffer = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                if (dsOffer != null && dsOffer.Tables.Count > 0 && dsOffer.Tables[0].Rows.Count > 0)
                {
                    int iOfferKey = Convert.ToInt32(dsOffer.Tables[0].Rows[0]["OfferKey"]);
                    LegalEntityApplicantNaturalPersonDeclarations leDec = new LegalEntityApplicantNaturalPersonDeclarations(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                    IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
                    SetupResult.For(applicationMortgageLoan.Key).Return(iOfferKey);

                    ExecuteRule(leDec, 2, applicationMortgageLoan);
                }
            }
        }

        [NUnit.Framework.Test]
        public void LegalEntityApplicantNaturalPersonDeclarationsMainOrSuretorInSolventTestPass()
        {
            using (new SessionScope())
            {
                string sql = @" select top 1 r.OfferKey--,  odAdministrationDate.*
	                            from OfferRole r 
                                join OfferDeclaration odInsolvent on r.OfferRoleKey = odInsolvent.OfferRoleKey
		                            and odInsolvent.OfferDeclarationQuestionKey = 1 And odInsolvent.OfferDeclarationAnswerKey = 1 -- Has been insolvent
	                            join OfferDeclaration odInsolventDate on r.OfferRoleKey = odInsolventDate.OfferRoleKey				-- and
		                            and odInsolventDate.OfferDeclarationQuestionKey = 2 And odInsolventDate.OfferDeclarationDate is not null -- has been rehabilitated
	                            join OfferDeclaration odDebtReview on r.OfferRoleKey = odDebtReview.OfferRoleKey
		                            and odDebtReview.OfferDeclarationQuestionKey = 5 and odDebtReview.OfferDeclarationAnswerKey =2 -- not under debt review
					            join OfferDeclaration odAdministrationDate on r.OfferRoleKey = odAdministrationDate.OfferRoleKey
						            and odAdministrationDate.OfferDeclarationQuestionKey = 2 and odAdministrationDate.OfferDeclarationDate is not null -- administration has been rescinded ";

                DataSet dsOffer = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                if (dsOffer != null && dsOffer.Tables.Count > 0 && dsOffer.Tables[0].Rows.Count > 0)
                {
                    int iOfferKey = Convert.ToInt32(dsOffer.Tables[0].Rows[0]["OfferKey"]);
                    LegalEntityApplicantNaturalPersonDeclarations leDec = new LegalEntityApplicantNaturalPersonDeclarations(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                    IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
                    SetupResult.For(applicationMortgageLoan.Key).Return(iOfferKey);

                    ExecuteRule(leDec, 0, applicationMortgageLoan);
                }
            }
        }

        [NUnit.Framework.Test]
        [Ignore("Long running test creates random failures")]
        public void LegalEntityApplicantNaturalPersonDeclarationsTestWithDeclarationAdministrationFail()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @" update OfferDeclaration set OfferDeclarationDate = null where OfferDeclarationQuestionKey = 4;";
                CastleTransactionsServiceHelper.ExecuteNonQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                sql = @" select top 1 r.OfferKey 
	                    from OfferRole r 
                        join OfferDeclaration odAdministration on r.OfferRoleKey = odAdministration.OfferRoleKey
		                    and odAdministration.OfferDeclarationQuestionKey = 3 And odAdministration.OfferDeclarationAnswerKey = 1 -- Has been under administration
	                    join OfferDeclaration odAdminDate on odAdministration.OfferRoleKey = odAdminDate.OfferRoleKey				-- and
		                    and odAdminDate.OfferDeclarationQuestionKey = 4 And odAdminDate.OfferDeclarationDate is null -- AdministrationDate does not exists
	                    join OfferDeclaration odDebtReview on odAdministration.OfferRoleKey = odDebtReview.OfferRoleKey
		                    and odDebtReview.OfferDeclarationQuestionKey = 5 and odDebtReview.OfferDeclarationAnswerKey =2 -- not under debt review
	                    join OfferDeclaration odInsolvent on odInsolvent.OfferRoleKey = odInsolvent.OfferRoleKey
		                    and (
			                    (odInsolvent.OfferDeclarationQuestionKey = 1 and odInsolvent.OfferDeclarationAnswerKey = 2) -- not Insolvent
			                    or																							-- or
			                    (odInsolvent.OfferDeclarationQuestionKey = 2 and odInsolvent.OfferDeclarationDate is not null) -- insolvency rehabilitated
			                    )";

                DataSet dsOffer = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                if (dsOffer != null && dsOffer.Tables.Count > 0 && dsOffer.Tables[0].Rows.Count > 0)
                {
                    int iOfferKey = Convert.ToInt32(dsOffer.Tables[0].Rows[0]["OfferKey"]);

                    LegalEntityApplicantNaturalPersonDeclarations leDec = new LegalEntityApplicantNaturalPersonDeclarations(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                    IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
                    SetupResult.For(applicationMortgageLoan.Key).Return(iOfferKey);

                    ExecuteRule(leDec, 1, applicationMortgageLoan);
                }
            }
        }

        [NUnit.Framework.Test]
        public void LegalEntityApplicantNaturalPersonDeclarationsTestWithDeclarationAdministrationTestPass()
        {
            string sql = @"select top 1 r.OfferKey 
	                from OfferRole r 
                    join OfferDeclaration odAdministration on r.OfferRoleKey = odAdministration.OfferRoleKey
		                and odAdministration.OfferDeclarationQuestionKey = 3 And odAdministration.OfferDeclarationAnswerKey = 1 --Has been under administration
	                join OfferDeclaration odAdminDate on odAdministration.OfferRoleKey = odAdminDate.OfferRoleKey
		                and odAdminDate.OfferDeclarationQuestionKey = 4 And odAdminDate.OfferDeclarationDate is not null --AdministrationDate exists
	                join OfferDeclaration odDebtReview on odAdministration.OfferRoleKey = odDebtReview.OfferRoleKey
		                and odDebtReview.OfferDeclarationQuestionKey = 5 and odDebtReview.OfferDeclarationAnswerKey =2 --not under debt review
	                join OfferDeclaration odInsolvent on odInsolvent.OfferRoleKey = odInsolvent.OfferRoleKey
		                and (
			                (odInsolvent.OfferDeclarationQuestionKey = 1 and odInsolvent.OfferDeclarationAnswerKey = 2) --not Insolvent
			                or
			                (odInsolvent.OfferDeclarationQuestionKey = 2 and odInsolvent.OfferDeclarationDate is not null) -- insolvency rehabilitated
			                )";

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
            if (obj != null)
            {
                int iOfferKey = Convert.ToInt32(obj);
                LegalEntityApplicantNaturalPersonDeclarations leDec = new LegalEntityApplicantNaturalPersonDeclarations(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
                SetupResult.For(applicationMortgageLoan.Key).Return(iOfferKey);

                ExecuteRule(leDec, 0, applicationMortgageLoan);
            }
        }

        [NUnit.Framework.Test]
        public void LegalEntityApplicantNaturalPersonDeclarationsTestWithDeclarationDebtReviewFail()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 r.OfferKey 
                    from OfferRole r 
                    join OfferDeclaration odAdministration on r.OfferRoleKey = odAdministration.OfferRoleKey
					    and (
						    (odAdministration.OfferDeclarationQuestionKey = 3 And odAdministration.OfferDeclarationAnswerKey = 1) --not under administration
						    or 
						    (odAdministration.OfferDeclarationQuestionKey = 4 And odAdministration.OfferDeclarationDate is not null) --administration rescinded
						    )
                    join OfferDeclaration odDebtReview on odAdministration.OfferRoleKey = odDebtReview.OfferRoleKey
	                    and odDebtReview.OfferDeclarationQuestionKey = 5 and odDebtReview.OfferDeclarationAnswerKey =1 --is under debt review
                    join OfferDeclaration odInsolvent on odInsolvent.OfferRoleKey = odInsolvent.OfferRoleKey
	                    and (
		                    (odInsolvent.OfferDeclarationQuestionKey = 1 and odInsolvent.OfferDeclarationAnswerKey = 2) --not Insolvent
		                    or
		                    (odInsolvent.OfferDeclarationQuestionKey = 2 and odInsolvent.OfferDeclarationDate is not null) -- insolvency rehabilitated
		                    )
                where r.OfferKey in (
							select ofr.OfferKey
							from offerrole ofr 
							join OfferRoleType ort (nolock) on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
                                    and ort.OfferRoleTypeGroupKey = 3 --clients only
                                    and ofr.OfferRoleTypeKey != 13 --no life cllients
							where ofr.GeneralStatusKey = 1 group by ofr.OfferKey
							having count(ofr.OfferKey) = 1
							)";

                DataSet dsOffer = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                if (dsOffer != null && dsOffer.Tables.Count > 0 && dsOffer.Tables[0].Rows.Count > 0)
                {
                    int iOfferKey = Convert.ToInt32(dsOffer.Tables[0].Rows[0]["OfferKey"]);
                    LegalEntityApplicantNaturalPersonDeclarations leDec = new LegalEntityApplicantNaturalPersonDeclarations(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                    IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
                    SetupResult.For(applicationMortgageLoan.Key).Return(iOfferKey);
                    // Setup applicationMortgageLoan.ApplicationRoles

                    ExecuteRule(leDec, 1, applicationMortgageLoan);
                }
            }

        }

        [NUnit.Framework.Test]
        public void LEApplicantNPDeclarationsWithDeclarationDebtReviewFail()
        {
            using (new SessionScope())
            {
                int result = 0;
                IDomainMessageCollection msgmock = _mockery.StrictMock<IDomainMessageCollection>();
                LegalEntityApplicantNaturalPersonDeclarations leDec = _mockery.StrictMock<LegalEntityApplicantNaturalPersonDeclarations>(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                //SetupResult.For(msgmock).Return(0);
                Expect.Call(leDec.ExecuteRule(msgmock, null)).Return(result);
                _mockery.ReplayAll();
                result = leDec.ExecuteRule(msgmock, null);
            }
        }

        [NUnit.Framework.Test]
        public void ApplicationDeclarations_TempTest()
        {
            using (new SessionScope())
            {
                LegalEntityApplicantNaturalPersonDeclarations leDec = new LegalEntityApplicantNaturalPersonDeclarations(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                IApplication app = appRepo.GetApplicationByKey(641646);

                ExecuteRule(leDec, 0, app);
            }
        }

        #region ValidateRemovedRoleMailingAddress

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ValidateRemovedRoleMailingAddressSuccessNoMailingAddress()
        {
            // the application has no mailing address - therefore validation will pass
            ValidateRemovedRoleMailingAddress rule = new ValidateRemovedRoleMailingAddress();

            // Setup the parameter objects to pass along
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();

            // Setup applicationroletypegroup
            IApplicationRoleTypeGroup applicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            SetupResult.For(applicationRoleTypeGroup.Key).Return((int)SAHL.Common.Globals.OfferRoleTypeGroups.Client);

            // Setup applicationroletype
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);

            // Setup application
            IApplication application = _mockery.StrictMock<IApplication>();
            SetupResult.For(applicationRole.Application).Return(application);
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

            // Setup Empty application mailing address
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            SetupResult.For(application.ApplicationMailingAddresses).Return(applicationMailingAddresses);

            //SetupResult.For(applicationMailingAddresses.Count).Return(0);

            ExecuteRule(rule, 0, applicationRole);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ValidateRemovedRoleMailingAddressSuccess()
        {
            // the application has a mailing address
            // the mailing address belongs to the legalentity that is to be removed
            // the mailing address also belongs to another legalentity on the application - therefore validation will pass
            ValidateRemovedRoleMailingAddress rule = new ValidateRemovedRoleMailingAddress();

            // Setup the parameter objects to pass along
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();

            // Setup applicationroletypegroup
            IApplicationRoleTypeGroup applicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            SetupResult.For(applicationRoleTypeGroup.Key).Return((int)SAHL.Common.Globals.OfferRoleTypeGroups.Client);

            // Setup applicationroletype
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);

            // Setup application
            IApplication application = _mockery.StrictMock<IApplication>();
            SetupResult.For(applicationRole.Application).Return(application);
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

            // Setup 2 different legalentities
            ILegalEntity le1 = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(le1.Key).Return(1);
            ILegalEntity le2 = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(le2.Key).Return(2);

            SetupResult.For(applicationRole.LegalEntity).Return(le1);

            // Setup generalstatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)SAHL.Common.Globals.GeneralStatuses.Active);

            // Setup Application Roles
            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();

            // Setup 2 different addresses to use
            IAddress mailingAddress = _mockery.StrictMock<IAddress>();
            SetupResult.For(mailingAddress.Key).Return(1);
            IAddress address2 = _mockery.StrictMock<IAddress>();
            SetupResult.For(address2.Key).Return(2);

            // Setup application role to be removed - with the  mailing address
            IApplicationRole aRole1 = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(aRole1.GeneralStatus).Return(generalStatus);
            SetupResult.For(aRole1.ApplicationRoleType).Return(applicationRoleType);
            SetupResult.For(aRole1.LegalEntity).Return(le1);
            ILegalEntityAddress leAddress1 = _mockery.StrictMock<ILegalEntityAddress>();
            IEventList<ILegalEntityAddress> legalEntityAddresses1 = new EventList<ILegalEntityAddress>();
            legalEntityAddresses1.Add(Messages, leAddress1);
            SetupResult.For(le1.LegalEntityAddresses).Return(legalEntityAddresses1);
            SetupResult.For(legalEntityAddresses1[0].Address).Return(mailingAddress);
            appRoles.Push(aRole1);

            // Setup another application role - also with the mailing address
            IApplicationRole aRole2 = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(aRole2.GeneralStatus).Return(generalStatus);
            SetupResult.For(aRole2.ApplicationRoleType).Return(applicationRoleType);
            SetupResult.For(aRole2.LegalEntity).Return(le2);
            ILegalEntityAddress leAddress2 = _mockery.StrictMock<ILegalEntityAddress>();
            IEventList<ILegalEntityAddress> legalEntityAddresses2 = new EventList<ILegalEntityAddress>();
            legalEntityAddresses2.Add(Messages, leAddress2);
            SetupResult.For(le2.LegalEntityAddresses).Return(legalEntityAddresses2);
            SetupResult.For(legalEntityAddresses2[0].Address).Return(mailingAddress);  // this will cause the validation to pass

            appRoles.Push(aRole2);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(application.ApplicationRoles).Return(applicationRoles);

            // Setup application mailing address
            IApplicationMailingAddress applicationMailingAddress = _mockery.StrictMock<IApplicationMailingAddress>();
            SetupResult.For(applicationMailingAddress.Address).Return(mailingAddress);
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            applicationMailingAddresses.Add(Messages, applicationMailingAddress);
            SetupResult.For(application.ApplicationMailingAddresses).Return(applicationMailingAddresses);

            ExecuteRule(rule, 0, applicationRole);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ValidateRemovedRoleMailingAddressFail()
        {
            // the application has a mailing address
            // the mailing address belongs to the legalentity that is to be removed
            // the mailing address does not belong to another legalentity on the application - therefore validation will fail

            ValidateRemovedRoleMailingAddress rule = new ValidateRemovedRoleMailingAddress();

            // Setup the parameter objects to pass along
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();

            // Setup applicationroletypegroup
            IApplicationRoleTypeGroup applicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            SetupResult.For(applicationRoleTypeGroup.Key).Return((int)SAHL.Common.Globals.OfferRoleTypeGroups.Client);

            // Setup applicationroletype
            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);

            // Setup application
            IApplication application = _mockery.StrictMock<IApplication>();
            SetupResult.For(applicationRole.Application).Return(application);
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

            // Setup 2 different legalentities
            ILegalEntity le1 = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(le1.Key).Return(1);
            ILegalEntity le2 = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(le2.Key).Return(2);

            SetupResult.For(applicationRole.LegalEntity).Return(le1);

            // Setup generalstatus
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)SAHL.Common.Globals.GeneralStatuses.Active);

            // Setup Application Roles
            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();

            // Setup 2 different addresses to use
            IAddress mailingAddress = _mockery.StrictMock<IAddress>();
            SetupResult.For(mailingAddress.Key).Return(1);
            IAddress address2 = _mockery.StrictMock<IAddress>();
            SetupResult.For(address2.Key).Return(2);

            // Setup application role to be removed - with the  mailing address
            IApplicationRole aRole1 = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(aRole1.GeneralStatus).Return(generalStatus);
            SetupResult.For(aRole1.ApplicationRoleType).Return(applicationRoleType);
            SetupResult.For(aRole1.LegalEntity).Return(le1);
            ILegalEntityAddress leAddress1 = _mockery.StrictMock<ILegalEntityAddress>();
            IEventList<ILegalEntityAddress> legalEntityAddresses1 = new EventList<ILegalEntityAddress>();
            legalEntityAddresses1.Add(Messages, leAddress1);
            SetupResult.For(le1.LegalEntityAddresses).Return(legalEntityAddresses1);
            SetupResult.For(legalEntityAddresses1[0].Address).Return(mailingAddress);
            appRoles.Push(aRole1);

            // Setup another application role - also with the mailing address
            IApplicationRole aRole2 = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(aRole2.GeneralStatus).Return(generalStatus);
            SetupResult.For(aRole2.ApplicationRoleType).Return(applicationRoleType);
            SetupResult.For(aRole2.LegalEntity).Return(le2);
            ILegalEntityAddress leAddress2 = _mockery.StrictMock<ILegalEntityAddress>();
            IEventList<ILegalEntityAddress> legalEntityAddresses2 = new EventList<ILegalEntityAddress>();
            legalEntityAddresses2.Add(Messages, leAddress2);
            SetupResult.For(le2.LegalEntityAddresses).Return(legalEntityAddresses2);
            SetupResult.For(legalEntityAddresses2[0].Address).Return(address2); // this will cause the validation to fail

            appRoles.Push(aRole2);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(application.ApplicationRoles).Return(applicationRoles);

            // Setup application mailing address
            IApplicationMailingAddress applicationMailingAddress = _mockery.StrictMock<IApplicationMailingAddress>();
            SetupResult.For(applicationMailingAddress.Address).Return(mailingAddress);
            IEventList<IApplicationMailingAddress> applicationMailingAddresses = new EventList<IApplicationMailingAddress>();
            applicationMailingAddresses.Add(Messages, applicationMailingAddress);
            SetupResult.For(application.ApplicationMailingAddresses).Return(applicationMailingAddresses);

            ExecuteRule(rule, 1, applicationRole);
        }

        #endregion ValidateRemovedRoleMailingAddress

        #region ApplicationHasActiveEmploymentRecord

        [NUnit.Framework.Test]
        public void ApplicationHasActiveEmploymentRecordTestPass()
        {
            ApplicationHasActiveEmploymentRecord rule = new ApplicationHasActiveEmploymentRecord();

            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            ILegalEntityNaturalPerson leNP = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            IEmploymentSalaried empSal = _mockery.StrictMock<IEmploymentSalaried>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationRoleTypeGroup appRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();

            //
            SetupResult.For(appRoleTypeGroup.Key).Return((int)OfferRoleTypeGroups.Client);

            //
            SetupResult.For(appRoleType.ApplicationRoleTypeGroup).Return(appRoleTypeGroup);

            //
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(empStatus.Key).Return((int)EmploymentStatuses.Current);

            //
            SetupResult.For(empSal.EmploymentStatus).Return(empStatus);
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            employments.Add(null, empSal);

            //
            SetupResult.For(leNP.Employment).Return(employments);

            //
            SetupResult.For(appRole.LegalEntity).Return(leNP);

            //
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);

            //
            Stack<IApplicationRole> stackAppRoles = new Stack<IApplicationRole>();
            stackAppRoles.Push(appRole);
            IReadOnlyEventList<IApplicationRole> appRoles = new ReadOnlyEventList<IApplicationRole>(stackAppRoles);

            //
            SetupResult.For(appMl.ApplicationRoles).Return(appRoles);

            //
            ExecuteRule(rule, 0, appMl);
        }

        [NUnit.Framework.Test]
        public void ApplicationHasActiveEmploymentRecordTestFail()
        {
            ApplicationHasActiveEmploymentRecord rule = new ApplicationHasActiveEmploymentRecord();

            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            ILegalEntityNaturalPerson leNP = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            IEmploymentSalaried empSal = _mockery.StrictMock<IEmploymentSalaried>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationRoleTypeGroup appRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();

            //
            SetupResult.For(appRoleTypeGroup.Key).Return((int)OfferRoleTypeGroups.Client);

            //
            SetupResult.For(appRoleType.ApplicationRoleTypeGroup).Return(appRoleTypeGroup);

            //
            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(empStatus.Key).Return((int)EmploymentStatuses.Previous);

            //
            SetupResult.For(empSal.EmploymentStatus).Return(empStatus);
            IEventList<IEmployment> employments = new EventList<IEmployment>();
            employments.Add(null, empSal);

            //
            SetupResult.For(leNP.Employment).Return(employments);

            //
            SetupResult.For(appRole.LegalEntity).Return(leNP);

            //
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);

            //
            Stack<IApplicationRole> stackAppRoles = new Stack<IApplicationRole>();
            stackAppRoles.Push(appRole);
            IReadOnlyEventList<IApplicationRole> appRoles = new ReadOnlyEventList<IApplicationRole>(stackAppRoles);

            //
            SetupResult.For(appMl.ApplicationRoles).Return(appRoles);

            //
            ExecuteRule(rule, 1, appMl);
        }

        #endregion ApplicationHasActiveEmploymentRecord

        #region ApplicationRoleRemoveLegalEntityMinimum

        [NUnit.Framework.Test]
        public void ApplicationRoleRemoveLegalEntityMinimumTestPass()
        {
            ///
            ApplicationRoleRemoveLegalEntityMinimum rule = new ApplicationRoleRemoveLegalEntityMinimum();
            IApplicationMortgageLoan _appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            //
            IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(status.Key).Return((int)GeneralStatuses.Active);

            //
            ILegalEntityNaturalPerson leMA = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(leMA.Key).Return(1);

            //
            IApplicationRoleTypeGroup applicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            SetupResult.For(applicationRoleTypeGroup.Key).Return((int)SAHL.Common.Globals.OfferRoleTypeGroups.Client);

            //
            IApplicationRoleType appRoleTypeLMA = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleTypeLMA.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRoleTypeLMA.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);

            //
            IApplicationRole appRoleMA = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(appRoleMA.ApplicationRoleType).Return(appRoleTypeLMA);
            SetupResult.For(appRoleMA.LegalEntity).Return(leMA);
            SetupResult.For(appRoleMA.GeneralStatus).Return(status);

            ///
            ILegalEntityNaturalPerson leLMA = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(leLMA.Key).Return(2);

            //
            IApplicationRoleType appRoleTypeMA = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleTypeMA.Key).Return((int)OfferRoleTypes.LeadMainApplicant);
            SetupResult.For(appRoleTypeMA.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);

            //
            IApplicationRole appRoleLMA = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(appRoleLMA.ApplicationRoleType).Return(appRoleTypeMA);
            SetupResult.For(appRoleLMA.LegalEntity).Return(leLMA);
            SetupResult.For(appRoleLMA.GeneralStatus).Return(status);

            ///
            Stack<IApplicationRole> stkAppRole = new Stack<IApplicationRole>();
            stkAppRole.Push(appRoleMA);
            stkAppRole.Push(appRoleLMA);

            //
            IReadOnlyEventList<IApplicationRole> appRoleList = new ReadOnlyEventList<IApplicationRole>(stkAppRole);

            //
            SetupResult.For(_appML.ApplicationRoles).Return(appRoleList);

            //
            SetupResult.For(appRoleMA.Application).Return(_appML);

            //
            ExecuteRule(rule, 0, appRoleMA);
        }

        [NUnit.Framework.Test]
        public void ApplicationRoleRemoveLegalEntityMinimumTestFail()
        {
            ///
            ApplicationRoleRemoveLegalEntityMinimum rule = new ApplicationRoleRemoveLegalEntityMinimum();
            IApplicationMortgageLoan _appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            //
            IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(status.Key).Return((int)GeneralStatuses.Active);

            //
            ILegalEntityNaturalPerson leMA = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(leMA.Key).Return(1);

            //
            IApplicationRoleTypeGroup applicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            SetupResult.For(applicationRoleTypeGroup.Key).Return((int)SAHL.Common.Globals.OfferRoleTypeGroups.Client);
            IApplicationRoleType appRoleTypeLMA = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleTypeLMA.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRoleTypeLMA.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);

            //
            IApplicationRole appRoleMA = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(appRoleMA.ApplicationRoleType).Return(appRoleTypeLMA);
            SetupResult.For(appRoleMA.LegalEntity).Return(leMA);
            SetupResult.For(appRoleMA.GeneralStatus).Return(status);

            ///
            ILegalEntityNaturalPerson leLMA = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(leLMA.Key).Return(1);

            //
            IApplicationRoleType appRoleTypeMA = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleTypeMA.Key).Return((int)OfferRoleTypes.LeadMainApplicant);

            //
            IApplicationRole appRoleLMA = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(appRoleLMA.ApplicationRoleType).Return(appRoleTypeMA);
            SetupResult.For(appRoleLMA.LegalEntity).Return(leLMA);
            SetupResult.For(appRoleLMA.GeneralStatus).Return(status);

            ///
            Stack<IApplicationRole> stkAppRole = new Stack<IApplicationRole>();
            stkAppRole.Push(appRoleMA);
            stkAppRole.Push(appRoleLMA);

            //
            IReadOnlyEventList<IApplicationRole> appRoleList = new ReadOnlyEventList<IApplicationRole>(stkAppRole);

            //
            SetupResult.For(_appML.ApplicationRoles).Return(appRoleList);

            //
            SetupResult.For(appRoleMA.Application).Return(_appML);

            //
            ExecuteRule(rule, 1, appRoleMA);
        }

        #endregion ApplicationRoleRemoveLegalEntityMinimum

        #region ApplicationRoleUpdateLegalEntityMinimum

        [NUnit.Framework.Test]
        public void ApplicationRoleUpdateLegalEntityMinimumTestPass()
        {
            ///
            ApplicationRoleUpdateLegalEntityMinimum rule = new ApplicationRoleUpdateLegalEntityMinimum();
            IApplicationMortgageLoan _appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            //
            IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(status.Key).Return((int)GeneralStatuses.Active);

            //
            ILegalEntityNaturalPerson leMA = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(leMA.Key).Return(1);

            //
            IApplicationRoleTypeGroup applicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            SetupResult.For(applicationRoleTypeGroup.Key).Return((int)SAHL.Common.Globals.OfferRoleTypeGroups.Client);

            IApplicationRoleType appRoleTypeLMA = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleTypeLMA.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRoleTypeLMA.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);

            //
            IApplicationRole appRoleMA = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(appRoleMA.ApplicationRoleType).Return(appRoleTypeLMA);
            SetupResult.For(appRoleMA.LegalEntity).Return(leMA);
            SetupResult.For(appRoleMA.GeneralStatus).Return(status);

            ///
            ILegalEntityNaturalPerson leLMA = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(leLMA.Key).Return(2);

            //
            IApplicationRoleType appRoleTypeMA = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleTypeMA.Key).Return((int)OfferRoleTypes.LeadMainApplicant);

            //
            IApplicationRole appRoleLMA = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(appRoleLMA.ApplicationRoleType).Return(appRoleTypeMA);
            SetupResult.For(appRoleLMA.LegalEntity).Return(leLMA);
            SetupResult.For(appRoleLMA.GeneralStatus).Return(status);

            ///
            Stack<IApplicationRole> stkAppRole = new Stack<IApplicationRole>();
            stkAppRole.Push(appRoleMA);
            stkAppRole.Push(appRoleLMA);

            //
            IReadOnlyEventList<IApplicationRole> appRoleList = new ReadOnlyEventList<IApplicationRole>(stkAppRole);

            //
            SetupResult.For(_appML.ApplicationRoles).Return(appRoleList);

            //
            SetupResult.For(appRoleMA.Application).Return(_appML);

            //
            ExecuteRule(rule, 0, appRoleMA);
        }

        [NUnit.Framework.Test]
        public void ApplicationRoleUpdateLegalEntityMinimumTestFail()
        {
            ///
            ApplicationRoleUpdateLegalEntityMinimum rule = new ApplicationRoleUpdateLegalEntityMinimum();
            IApplicationMortgageLoan _appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            //
            IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(status.Key).Return((int)GeneralStatuses.Inactive);

            //
            ILegalEntityNaturalPerson leMA = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(leMA.Key).Return(1);

            //
            IApplicationRoleTypeGroup applicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            SetupResult.For(applicationRoleTypeGroup.Key).Return((int)SAHL.Common.Globals.OfferRoleTypeGroups.Client);

            IApplicationRoleType appRoleTypeLMA = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleTypeLMA.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRoleTypeLMA.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);

            //
            IApplicationRole appRoleMA = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(appRoleMA.ApplicationRoleType).Return(appRoleTypeLMA);
            SetupResult.For(appRoleMA.LegalEntity).Return(leMA);
            SetupResult.For(appRoleMA.GeneralStatus).Return(status);

            ///
            ILegalEntityNaturalPerson leLMA = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(leLMA.Key).Return(2);

            //
            IApplicationRoleType appRoleTypeMA = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleTypeMA.Key).Return((int)OfferRoleTypes.LeadMainApplicant);

            //
            IApplicationRole appRoleLMA = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(appRoleLMA.ApplicationRoleType).Return(appRoleTypeMA);
            SetupResult.For(appRoleLMA.LegalEntity).Return(leLMA);
            SetupResult.For(appRoleLMA.GeneralStatus).Return(status);

            ///
            Stack<IApplicationRole> stkAppRole = new Stack<IApplicationRole>();
            stkAppRole.Push(appRoleMA);
            stkAppRole.Push(appRoleLMA);

            //
            IReadOnlyEventList<IApplicationRole> appRoleList = new ReadOnlyEventList<IApplicationRole>(stkAppRole);

            //
            SetupResult.For(_appML.ApplicationRoles).Return(appRoleList);

            //
            SetupResult.For(appRoleMA.Application).Return(_appML);

            //
            ExecuteRule(rule, 1, appRoleMA);
        }

        #endregion ApplicationRoleUpdateLegalEntityMinimum

        #region OfferRoleFLAddMainApplicant

        private void OfferRoleFLAddMainApplicantHelper(int RoleTypeKey, int appTypeKey, int msgCount)
        {
            //adding a main applicant to a FL offer should add one domain Error message
            OfferRoleFLAddMainApplicant rule = new OfferRoleFLAddMainApplicant();
            IApplication app = _mockery.StrictMock<IApplication>();

            SetupResult.For(app.Key).Return(1);

            IApplicationType at = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(at.Key).Return(appTypeKey);
            SetupResult.For(app.ApplicationType).Return(at);

            IApplicationRoleType art = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(art.Key).Return(RoleTypeKey);

            IApplicationRole ar = _mockery.StrictMock<IApplicationRole>();
            SetupResult.For(ar.Key).Return(1);
            SetupResult.For(ar.ApplicationRoleType).Return(art);

            SetupResult.For(ar.Application).Return(app);

            ExecuteRule(rule, msgCount, ar);
        }

        [NUnit.Framework.Test]
        public void OfferRoleFLAddMainApplicantTestFail()
        {
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.MainApplicant, (int)OfferTypes.ReAdvance, 1);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.MainApplicant, (int)OfferTypes.FurtherAdvance, 1);

            //OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.MainApplicant, (int)OfferTypes.FurtherLoan, 1);
        }

        [NUnit.Framework.Test]
        public void OfferRoleFLAddMainApplicantTestPass()
        {
            //adding a suretor to a FL offer should result in no domain messages
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.Suretor, (int)OfferTypes.ReAdvance, 0);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.Suretor, (int)OfferTypes.FurtherAdvance, 0);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.Suretor, (int)OfferTypes.FurtherLoan, 0);

            //all other application types should be unaffected
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.MainApplicant, (int)OfferTypes.Life, 0);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.MainApplicant, (int)OfferTypes.NewPurchaseLoan, 0);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.MainApplicant, (int)OfferTypes.RefinanceLoan, 0);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.MainApplicant, (int)OfferTypes.SwitchLoan, 0);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.MainApplicant, (int)OfferTypes.Unknown, 0);

            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.Suretor, (int)OfferTypes.Life, 0);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.Suretor, (int)OfferTypes.NewPurchaseLoan, 0);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.Suretor, (int)OfferTypes.RefinanceLoan, 0);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.Suretor, (int)OfferTypes.SwitchLoan, 0);
            OfferRoleFLAddMainApplicantHelper((int)OfferRoleTypes.Suretor, (int)OfferTypes.Unknown, 0);
        }

        #endregion OfferRoleFLAddMainApplicant
    }
}