using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class ApplicationModelManager : IApplicationModelManager
    {
        private IValidationUtils validationUtils;
        private IApplicantModelManager applicantModelManager;
        private IApplicationDebitOrderModelManager applicationDebitOrderModelManager;
        private IAddressModelManager addressModelManager;
        private IPropertyDataModelManager propertyDataModelManager;
        private IApplicationMailingAddressModelManager applicationMailingAddressModelManager;

        public ApplicationModelManager(IValidationUtils validationUtils, IApplicantModelManager applicantModelManager, IApplicationDebitOrderModelManager applicationDebitOrderModelManager,
            IAddressModelManager addressModelManager, IPropertyDataModelManager propertyDataModelManager, IApplicationMailingAddressModelManager applicationMailingAddressModelManager)
        {
            this.validationUtils = validationUtils;
            this.applicantModelManager = applicantModelManager;
            this.applicationDebitOrderModelManager = applicationDebitOrderModelManager;
            this.addressModelManager = addressModelManager;
            this.propertyDataModelManager = propertyDataModelManager;
            this.applicationMailingAddressModelManager = applicationMailingAddressModelManager;
        }

        public DomainProcessManager.Models.NewPurchaseApplicationCreationModel PopulateNewPurchaseApplicationCreationModel(Objects.Application comcorpApplication)
        {
            var applicationModel = PopulateApplicationCreationModel(comcorpApplication, OfferType.NewPurchaseLoan);
            NewPurchaseApplicationCreationModel newpurchaseApplicationCreationModel = new NewPurchaseApplicationCreationModel(
                                        applicationModel.ApplicationStatus, comcorpApplication.ApplicationCode.ToString(), applicationModel.ApplicationSourceKey,
                                        applicationModel.OriginationSource, applicationModel.ConsultantFirstName, applicationModel.ConsultantSurname,
                                        applicationModel.Applicants.ToList(), comcorpApplication.DepositCash, comcorpApplication.PropertyPurchasePrice, applicationModel.EstimatedPropertyValue, 
                                        applicationModel.ComcorpApplicationPropertyDetail, applicationModel.Term, applicationModel.Product, 
                                        applicationModel.ApplicationDebitOrder, applicationModel.ApplicationMailingAddress, applicationModel.VendorCode, 
                                        comcorpApplication.LoanAmountRequired, comcorpApplication.Property.SellerIDNo, comcorpApplication.SahlTransferAtt, comcorpApplication.NamePropertyRegistered, 
                                        applicationModel.PropertyAddress);
            return newpurchaseApplicationCreationModel;
        }

        public DomainProcessManager.Models.SwitchApplicationCreationModel PopulateSwitchApplicationCreationModel(Objects.Application comcorpApplication)
        {
            var applicationModel = PopulateApplicationCreationModel(comcorpApplication, OfferType.SwitchLoan);
            SwitchApplicationCreationModel switchApplicationCreationModel = new SwitchApplicationCreationModel(applicationModel.ApplicationStatus, comcorpApplication.ApplicationCode.ToString(),
                    applicationModel.ApplicationSourceKey, applicationModel.OriginationSource, applicationModel.ConsultantFirstName, 
                    applicationModel.ConsultantSurname, applicationModel.Applicants.ToList(), comcorpApplication.OutstandingLoan, applicationModel.EstimatedPropertyValue, 
                    applicationModel.ComcorpApplicationPropertyDetail, applicationModel.Term, comcorpApplication.CashOutAmt, comcorpApplication.CashOutReason, applicationModel.Product,
                    comcorpApplication.HigherBond, comcorpApplication.TotalEstLoan, comcorpApplication.QuickCashIndicator, comcorpApplication.CapitaliseFees, applicationModel.ApplicationDebitOrder,
                    applicationModel.ApplicationMailingAddress, applicationModel.VendorCode,comcorpApplication.NamePropertyRegistered, applicationModel.PropertyAddress);
            return switchApplicationCreationModel;
        }

        public DomainProcessManager.Models.RefinanceApplicationCreationModel PopulateRefinanceApplicationCreationModel(Objects.Application comcorpApplication)
        {
            var applicationModel = PopulateApplicationCreationModel(comcorpApplication, OfferType.RefinanceLoan);
            RefinanceApplicationCreationModel refinanceApplicationCreationModel = new RefinanceApplicationCreationModel(applicationModel.ApplicationStatus,
                        comcorpApplication.ApplicationCode.ToString(), applicationModel.ApplicationSourceKey, applicationModel.OriginationSource,
                        applicationModel.ConsultantFirstName, applicationModel.ConsultantSurname, applicationModel.Applicants.ToList(),
                        applicationModel.EstimatedPropertyValue, applicationModel.Term, comcorpApplication.CashOutAmt, comcorpApplication.CashOutReason, applicationModel.Product,
                        comcorpApplication.HigherBond, comcorpApplication.TotalEstLoan, applicationModel.ComcorpApplicationPropertyDetail, comcorpApplication.QuickCashIndicator, 
                        comcorpApplication.CapitaliseFees, applicationModel.ApplicationDebitOrder, applicationModel.ApplicationMailingAddress, applicationModel.VendorCode, 
                        comcorpApplication.NamePropertyRegistered, applicationModel.PropertyAddress);
            return refinanceApplicationCreationModel;
        }

        public ApplicationCreationModel PopulateApplicationCreationModel(Application comcorpApplication, OfferType applicationType)
        {

            ResidentialAddressModel propertyAddress = addressModelManager.PopulateResidentialAddressFromComcorpProperty(comcorpApplication.Property);
            PropertyAddressModel propertyAddressModel = addressModelManager.PopulatePropertyAddressFromComcorpProperty(comcorpApplication.Property);
            ComcorpApplicationPropertyDetailsModel comcorpPropertyDetails = propertyDataModelManager.PopulateComcorpPropertyDetails(comcorpApplication.SAHLOccupancyType,
                comcorpApplication.SAHLPropertyType, comcorpApplication.SAHLTitleType, comcorpApplication.SectionalTitleUnitNo, comcorpApplication.NamePropertyRegistered,
                comcorpApplication.Property);
            ApplicationMailingAddressModel applicationMailingAddressModel = null;
            if (comcorpApplication.Property.UsePropertyAddressAsLoanMailingAddress)
            {
                applicationMailingAddressModel = applicationMailingAddressModelManager.PopulateApplicationMailingAddress(propertyAddress, null);
            }


            List<ApplicantModel> applicants = new List<ApplicantModel>();

            ApplicantModel applicant = applicantModelManager.PopulateApplicantDetails(comcorpApplication.MainApplicant, propertyAddress);
            applicants.Add(applicant);

            if (comcorpApplication.MainApplicant.Spouse!=null)
            {
                applicant = applicantModelManager.PopulateApplicantDetails(comcorpApplication.MainApplicant.Spouse, propertyAddress);
                applicants.Add(applicant);
            }

            foreach (var comcorpApplicant in comcorpApplication.CoApplicants)
            {
                applicant = applicantModelManager.PopulateApplicantDetails(comcorpApplicant, propertyAddress);
                applicants.Add(applicant);

                if (comcorpApplicant.Spouse != null)
                {
                    applicant = applicantModelManager.PopulateApplicantDetails(comcorpApplicant.Spouse, propertyAddress);
                    applicants.Add(applicant);
                }
            }

            ApplicationDebitOrderModel applicationDebitOrderModel = applicationDebitOrderModelManager.PopulateApplicationDebitOrder(applicants);
            ApplicationCreationModel applicationCreationModel = new ApplicationCreationModel(
                applicationType,
                OfferStatus.Open,
                comcorpApplication.ApplicationCode.ToString(),  // reference
                null,                                           // application source key
                OriginationSource.Comcorp,
                comcorpApplication.ConsultantFirstName,
                comcorpApplication.ConsultantSurname,
                applicants,
                (int)comcorpApplication.TermOfLoan * 12, // comcorp are sending this in years so we need to mutliply by 12
                comcorpApplication.PropertyMarketValue,
                comcorpPropertyDetails,
                Product.NewVariableLoan,
                applicationDebitOrderModel,
                applicationMailingAddressModel,
                comcorpApplication.SAHLVendorCode,
                comcorpApplication.SahlTransferAtt,
                comcorpApplication.NamePropertyRegistered,
                propertyAddressModel
                );

            return applicationCreationModel;
        }
    }
}