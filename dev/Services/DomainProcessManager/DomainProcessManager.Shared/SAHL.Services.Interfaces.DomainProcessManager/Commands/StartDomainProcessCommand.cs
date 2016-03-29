using SAHL.Core.Data;
using SAHL.DomainProcessManager.Models;
using System;
using System.Runtime.Serialization;

namespace SAHL.Services.Interfaces.DomainProcessManager
{
    [KnownType(typeof(AddressModel))]
    [KnownType(typeof(ApplicantAffordabilityModel))]
    [KnownType(typeof(ApplicantAssetLiabilityModel))]
    [KnownType(typeof(ApplicantDeclarationsModel))]
    [KnownType(typeof(ApplicantFixedLongTermLiabilityModel))]
    [KnownType(typeof(ApplicantFixedPropertyAssetModel))]
    [KnownType(typeof(ApplicantLiabilityLoanModel))]
    [KnownType(typeof(ApplicantLiabilitySuretyModel))]
    [KnownType(typeof(ApplicantLifeAssuranceAssetModel))]
    [KnownType(typeof(ApplicantListedInvestmentAssetModel))]
    [KnownType(typeof(ApplicantMarketingOptionModel))]
    [KnownType(typeof(ApplicantModel))]
    [KnownType(typeof(ApplicantOtherAssetModel))]
    [KnownType(typeof(ApplicantUnListedInvestmentAssetModel))]
    [KnownType(typeof(ApplicationCreationModel))]
    [KnownType(typeof(ApplicationDebitOrderModel))]
    [KnownType(typeof(ApplicationMailingAddressModel))]
    [KnownType(typeof(BankAccountModel))]
    [KnownType(typeof(ComcorpApplicationPropertyDetailsModel))]
    [KnownType(typeof(EmployerModel))]
    [KnownType(typeof(EmploymentModel))]
    [KnownType(typeof(FreeTextAddressModel))]
    [KnownType(typeof(NewPurchaseApplicationCreationModel))]
    [KnownType(typeof(PostalAddressBoxModel))]
    [KnownType(typeof(PostalAddressClusterBoxModel))]
    [KnownType(typeof(PostalAddressModel))]
    [KnownType(typeof(PostalAddressPostnetSuiteModel))]
    [KnownType(typeof(PostalAddressPrivateBagModel))]
    [KnownType(typeof(PostalAddressStreetModel))]
    [KnownType(typeof(PropertyAddressModel))]
    [KnownType(typeof(PropertyModel))]
    [KnownType(typeof(ResidentialAddressModel))]
    [KnownType(typeof(SalariedEmploymentModel))]
    [KnownType(typeof(SalaryDeductionEmploymentModel))]
    [KnownType(typeof(UnemployedEmploymentModel))]
    [KnownType(typeof(SwitchApplicationCreationModel))]
    [KnownType(typeof(ApplicationCreationReturnDataModel))]
    [KnownType(typeof(StartDomainProcessResponse))]
    [KnownType(typeof(RefinanceApplicationCreationModel))]
    [KnownType(typeof(ReceiveAttorneyInvoiceProcessModel))]
    [KnownType(typeof(PayThirdPartyInvoiceProcessModel))]
    [KnownType(typeof(PayThirdPartyInvoiceModel))]
    [DataContract]
    public class StartDomainProcessCommand : IStartDomainProcessCommand
    {
        public StartDomainProcessCommand(IDataModel dataModel, string startEventToWaitFor)
        {
            if (dataModel == null) { throw new ArgumentNullException("dataModel"); }
            if (string.IsNullOrWhiteSpace(startEventToWaitFor)) { throw new ArgumentNullException("startEventToWaitFor"); }

            this.DataModel = dataModel;
            this.StartEventToWaitFor = startEventToWaitFor;
        }

        [DataMember]
        public IDataModel DataModel { get; protected set; }

        [DataMember]
        public string StartEventToWaitFor { get; protected set; }

        [DataMember]
        public StartDomainProcessResponse Result { get; set; }
    }
}