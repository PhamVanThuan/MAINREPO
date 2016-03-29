using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.Attorney.Attorney;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.Attorney
{
    [TestFixture]
    public class Attorney : RuleBase
    {
        IApplication _application;

        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();
            _application = _mockery.StrictMock<IApplication>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        //ApplicationReasonTypeAdd TESTS

        [NUnit.Framework.Test]
        public void AttorneyValidateTransferAttorneyNullFail()
        {
            AttorneyValidateTransferAttorney art = new AttorneyValidateTransferAttorney();
            IApplicationMortgageLoan application = _mockery.StrictMock<IApplicationMortgageLoan>();
            Expect.Call(application.TransferringAttorney).Return(null);

            ExecuteRule(art, 1, application);
        }

        #region AttorneyMandatoryFields

        [NUnit.Framework.Test]
        public void AttorneyMandatoryFieldsTestPass()
        {
            AttorneyMandatoryFields rule = new AttorneyMandatoryFields();

            //Attorney mock setup
            IAttorney attorney_your_mother_is_err_hamster = _mockery.StrictMock<IAttorney>();
            ILegalEntityCompany attorney_LE_Company_your_mother_is_err_hamster = _mockery.StrictMock<ILegalEntityCompany>();

            SetupResult.For(attorney_LE_Company_your_mother_is_err_hamster.RegisteredName).Return("RegisteredName");
            SetupResult.For(attorney_LE_Company_your_mother_is_err_hamster.WorkPhoneCode).Return("123");
            SetupResult.For(attorney_LE_Company_your_mother_is_err_hamster.WorkPhoneNumber).Return("1234567");
            SetupResult.For(attorney_LE_Company_your_mother_is_err_hamster.EmailAddress).Return("test@tester.com");

            SetupResult.For(attorney_your_mother_is_err_hamster.AttorneyContact).Return("AttorneyContact");
            SetupResult.For(attorney_your_mother_is_err_hamster.AttorneyWorkFlowEnabled).Return(1);
            SetupResult.For(attorney_your_mother_is_err_hamster.AttorneyMandate).Return(1000000.00);
            SetupResult.For(attorney_your_mother_is_err_hamster.AttorneyRegistrationInd).Return(true);
            SetupResult.For(attorney_your_mother_is_err_hamster.AttorneyLitigationInd).Return(true);

            SetupResult.For(attorney_your_mother_is_err_hamster.LegalEntity).Return(attorney_LE_Company_your_mother_is_err_hamster as ILegalEntity);

            ExecuteRule(rule, 0, attorney_your_mother_is_err_hamster);
        }

        [NUnit.Framework.Test]
        public void AttorneyMandatoryFieldsTestFail()
        {
            AttorneyMandatoryFields rule = new AttorneyMandatoryFields();

            //Attorney mock setup
            IAttorney attorney_your_mother_is_err_hamster = _mockery.StrictMock<IAttorney>();
            ILegalEntityCompany attorney_LE_Company_your_mother_is_err_hamster = _mockery.StrictMock<ILegalEntityCompany>();

            SetupResult.For(attorney_LE_Company_your_mother_is_err_hamster.RegisteredName).Return(string.Empty);
            SetupResult.For(attorney_LE_Company_your_mother_is_err_hamster.WorkPhoneCode).Return(string.Empty);
            SetupResult.For(attorney_LE_Company_your_mother_is_err_hamster.WorkPhoneNumber).Return(string.Empty);
            SetupResult.For(attorney_LE_Company_your_mother_is_err_hamster.EmailAddress).Return(string.Empty);

            SetupResult.For(attorney_your_mother_is_err_hamster.AttorneyContact).Return(string.Empty);
            SetupResult.For(attorney_your_mother_is_err_hamster.AttorneyWorkFlowEnabled).Return(null);
            SetupResult.For(attorney_your_mother_is_err_hamster.AttorneyMandate).Return(null);
            SetupResult.For(attorney_your_mother_is_err_hamster.AttorneyRegistrationInd).Return(null);
            SetupResult.For(attorney_your_mother_is_err_hamster.AttorneyLitigationInd).Return(null);

            SetupResult.For(attorney_your_mother_is_err_hamster.LegalEntity).Return(attorney_LE_Company_your_mother_is_err_hamster as ILegalEntity);

            ExecuteRule(rule, 8, attorney_your_mother_is_err_hamster);
        }

        #endregion AttorneyMandatoryFields

        [Test]
        public void ApplicatonDuplicateInstructionCheckTestPass()
        {
            using (new SessionScope())
            {
                ApplicatonDuplicateInstructionCheck rule = new ApplicatonDuplicateInstructionCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                IProperty prop = _mockery.StrictMock<IProperty>();

                string query = @"select top 1 oml1.OfferKey
                from [2am].dbo.OfferMortgageLoan oml1 (nolock)
                inner join [2am].[dbo].Offer ofr1
	                on
		                ofr1.offerKey = oml1.OfferKey
                inner join [2am].dbo.OfferMortgageLoan oml2 (nolock)
	                on
		                oml1.PropertyKey = oml2.PropertyKey and oml1.OfferKey <> oml2.OfferKey
                inner join [2am].dbo.StageTransitionComposite stc (nolock)
	                on
		                stc.GenericKey = oml2.OfferKey
                inner join [2am].dbo.Offer ofr (nolock)
	                on
		                ofr.offerKey = oml2.OfferKey
                left join [2am].dbo.Account acc (nolock)
	                on
		                acc.accountKey = ofr.reservedAccountKey and acc.accountStatusKey in (1,2,5)
                where
	                ofr1.offerTypeKey not in (2,3,4) -- Exclude rule for a Further Lending Application
                and
	                ofr.offerStatusKey in (1,3) -- checking against active Applications only
                and
	                acc.accountKey is not null -- exclude Application that exist against Open/Close/Dormant Account
                and
	                stc.StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey";

                ParameterCollection pc = new ParameterCollection();
                pc.Add(new SqlParameter("@StageDefinitionStageDefinitionGroupKey", (int)StageDefinitionStageDefinitionGroups.InstructAttorney));
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
                if (o != null)
                {
                    int appKey = Convert.ToInt32(o);
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication app = appRepo.GetApplicationByKey(appKey);
                    ExecuteRule(rule, 0, app);
                }
            }
        }

        [Test]
        public void AttorneyStatusInactive()
        {
            AttorneyStatusInactiveHelper(GeneralStatuses.Active, null, 0);
            AttorneyStatusInactiveHelper(GeneralStatuses.Active, true, 0);
            AttorneyStatusInactiveHelper(null, false, 0);
            AttorneyStatusInactiveHelper(null, null, 0);
            AttorneyStatusInactiveHelper(GeneralStatuses.Inactive, true, 1);
            AttorneyStatusInactiveHelper(null, true, 1);
            AttorneyStatusInactiveHelper(GeneralStatuses.Inactive, null, 0);
            AttorneyStatusInactiveHelper(GeneralStatuses.Inactive, false, 0);
        }

        private void AttorneyStatusInactiveHelper(GeneralStatuses? gS, bool? litIndicator, int msgCount)
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                AttorneyStatusInactive rule = new AttorneyStatusInactive();
                IAttorney att = _mockery.StrictMock<IAttorney>();
                if (gS != null)
                {
                    IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
                    SetupResult.For(gs.Key).Return((int)gS);
                    SetupResult.For(att.GeneralStatus).Return(gs);
                }
                else
                    SetupResult.For(att.GeneralStatus).Return(null);

                SetupResult.For(att.AttorneyLitigationInd).Return(litIndicator);

                ExecuteRule(rule, msgCount, att);
            }
        }

        [Test]
        public void ApplicatonDuplicateInstructionCheckTestFail()
        {
            using (new SessionScope())
            {
                ApplicatonDuplicateInstructionCheck rule = new ApplicatonDuplicateInstructionCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                //                string query = @"with cteOML (minOfferKey, maxOfferKey, PropertyKey) AS
                //                    (
                //	                    select min(oml.offerKey) ofr1, max(oml.offerKey) as ofr2,oml.PropertyKey
                //	                    from OfferMortgageLoan oml (nolock)
                //	                    join Offer o on oml.OfferKey = o.OfferKey
                //	                    where o.offerTypeKey not in (2,3,4) -- Exclude rule for a Further Lending Application
                //	                    and o.offerStatusKey in (1,3)
                //	                    group by oml.PropertyKey
                //	                    having count(oml.PropertyKey) > 1
                //                    )
                //
                //
                //                    select top 1 cteOML.minOfferKey
                //                    from cteOML
                //                    inner join offer o on o.offerkey = cteOML.maxOfferKey
                //                    inner join [2am].dbo.StageTransitionComposite stc (nolock) on stc.GenericKey = cteOML.maxOfferKey
                //                    left join [2am].dbo.Account acc (nolock) on acc.accountKey = o.reservedAccountKey and acc.accountStatusKey in (1,2,5)
                //                    where stc.StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey
                //                    order by 1";

                string query = @"SELECT MinOfferKey FROM
                                (
	                                select top 1 cteOML.minOfferKey as [MinOfferKey]
	                                from (
		                                select min(oml.offerKey) minOfferKey, max(oml.offerKey) as maxOfferKey ,oml.PropertyKey
		                                from OfferMortgageLoan oml (nolock)
		                                join Offer o on oml.OfferKey = o.OfferKey
		                                where o.offerTypeKey not in (2,3,4) -- Exclude rule for a Further Lending Application
		                                and o.offerStatusKey in (1,3)
		                                group by oml.PropertyKey
		                                having count(oml.PropertyKey) > 1
	                                )cteOML
	                                inner join offer o on o.offerkey = cteOML.maxOfferKey
	                                inner join [2am].dbo.StageTransitionComposite stc (nolock) on stc.GenericKey = cteOML.maxOfferKey
	                                left join [2am].dbo.Account acc (nolock) on acc.accountKey = o.reservedAccountKey and acc.accountStatusKey in (1,2,5)
	                                where stc.StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey and acc.AccountKey is null
	                                order by 1 desc
                                ) monOffer";

                ParameterCollection pc = new ParameterCollection();
                pc.Add(new SqlParameter("@StageDefinitionStageDefinitionGroupKey", (int)StageDefinitionStageDefinitionGroups.InstructAttorney));
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
                if (o != null)
                {
                    int appKey = Convert.ToInt32(o);
                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication app = appRepo.GetApplicationByKey(appKey);
                    ExecuteRule(rule, 1, app);
                }
            }
        }
    }
}