using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;

namespace SAHL.Services.Interfaces.DomainProcessManager.Tests.Mocks
{
    public class PurchaseApplicationFactory
    {
        protected readonly Dictionary<OfferType, Func<IEnumerable<ApplicantModel>, ApplicationCreationModel>> modelCreationMethodMapper = new Dictionary
            <OfferType, Func<IEnumerable<ApplicantModel>, ApplicationCreationModel>>();

        public PurchaseApplicationFactory()
        {
            modelCreationMethodMapper.Add(OfferType.NewPurchaseLoan, this.CreateNewPurchaseModel);
            modelCreationMethodMapper.Add(OfferType.SwitchLoan, this.CreateNewSwitchModel);
            modelCreationMethodMapper.Add(OfferType.RefinanceLoan, this.CreateNewRefinanceModel);
        }

        public virtual ApplicationCreationModel Create(OfferType offerType, IEnumerable<ApplicantModel> applicants)
        {
            Func<IEnumerable<ApplicantModel>, ApplicationCreationModel> functionToCreateModel;
            return modelCreationMethodMapper.TryGetValue(offerType, out functionToCreateModel)
                ? functionToCreateModel(applicants)
                : null;
        }

        protected ApplicationCreationModel CreateNewRefinanceModel(IEnumerable<ApplicantModel> applicants)
        {
            return new RefinanceApplicationCreationModel(OfferStatus.Open,
                "Ref1234",
                1,
                OriginationSource.Comcorp,
                "John",
                "Smith",
                applicants.ToList(),
                20000,
                240,
                25000,
                "cash out reason",
                Product.NewVariableLoan,
                500,
                600000,
                ApplicationCreationTestHelper.PopulateComcorpPropertyDetailsModel(),
                false,
                false,
                new ApplicationDebitOrderModel(FinancialServicePaymentType.DebitOrderPayment,
                    1,
                    ApplicationCreationTestHelper.PopulateBankAccountModel()),
                ApplicationCreationTestHelper.PopulateApplicationMailingAddressModel(
                    ApplicationCreationTestHelper.PopulateFreeTextPostalAddressModel()),
                "SAHL1",
                "Name to Register Property",
                ApplicationCreationTestHelper.CreatePropertyAddressModel());
        }

        protected ApplicationCreationModel CreateNewSwitchModel(IEnumerable<ApplicantModel> applicants)
        {
            return new SwitchApplicationCreationModel(OfferStatus.Open,
                "Ref1234",
                1,
                OriginationSource.Comcorp,
                "John",
                "Smith",
                applicants.ToList(),
                20000,
                500000,
                ApplicationCreationTestHelper.PopulateComcorpPropertyDetailsModel(),
                240,
                0,
                "cash out reason",
                Product.NewVariableLoan,
                500,
                600000,
                false,
                false,
                new ApplicationDebitOrderModel(FinancialServicePaymentType.DebitOrderPayment,
                    1,
                    ApplicationCreationTestHelper.PopulateBankAccountModel()),
                ApplicationCreationTestHelper.PopulateApplicationMailingAddressModel(
                    ApplicationCreationTestHelper.PopulateFreeTextPostalAddressModel()),
                "SAHL1",
                "Name to Register Property",
                ApplicationCreationTestHelper.CreatePropertyAddressModel());
        }

        protected ApplicationCreationModel CreateNewPurchaseModel(IEnumerable<ApplicantModel> applicants)
        {
            return new NewPurchaseApplicationCreationModel(OfferStatus.Open,
                "Reference1234",
                1,
                OriginationSource.Comcorp,
                "John",
                "Smith",
                applicants.ToList(),
                100000,
                500000,
                600000,
                ApplicationCreationTestHelper.PopulateComcorpPropertyDetailsModel(),
                240,
                Product.NewVariableLoan,
                new ApplicationDebitOrderModel(FinancialServicePaymentType.DebitOrderPayment,
                    1,
                    ApplicationCreationTestHelper.PopulateBankAccountModel()),
                ApplicationCreationTestHelper.PopulateApplicationMailingAddressModel(
                    ApplicationCreationTestHelper.PopulateFreeTextPostalAddressModel()),
                "SAHL1",
                400000,
                "6812095219087",
                "Transfer Attorney Name",
                "Name to Register Property",
                ApplicationCreationTestHelper.CreatePropertyAddressModel());
        }
    }
}
