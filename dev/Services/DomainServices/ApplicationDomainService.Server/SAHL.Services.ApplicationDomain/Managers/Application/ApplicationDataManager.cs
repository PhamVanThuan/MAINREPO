using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.Application.Statements;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Managers.Application
{
    public class ApplicationDataManager : IApplicationDataManager
    {
        private IDbFactory dbFactory;

        public ApplicationDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int SaveApplication(OfferDataModel offer)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferDataModel>(offer);
                db.Complete();
                return offer.OfferKey;
            }
        }

        public void SaveApplicationMortgageLoan(OfferMortgageLoanDataModel offerMortgageLoan)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferMortgageLoanDataModel>(offerMortgageLoan);
                db.Complete();
            }
        }

        public int SaveApplicationInformation(OfferInformationDataModel offerInformation)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferInformationDataModel>(offerInformation);
                db.Complete();
                return offerInformation.OfferInformationKey;
            }
        }

        public void SaveApplicationInformationVariableLoan(OfferInformationVariableLoanDataModel offerInformationVariableLoan)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferInformationVariableLoanDataModel>(offerInformationVariableLoan);
                db.Complete();
            }
        }

        public void SaveApplicationInformationInterestOnly(OfferInformationInterestOnlyDataModel offerInformationInterestOnly)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferInformationInterestOnlyDataModel>(offerInformationInterestOnly);
                db.Complete();
            }
        }

        public int GetReservedAccountNumber()
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                AccountSequenceDataModel accountSequence = new AccountSequenceDataModel(true);

                db.Insert<AccountSequenceDataModel>(accountSequence);
                db.Complete();
                return accountSequence.AccountKey;
            }
        }

        public void SaveExternalOriginatorAttribute(OfferAttributeDataModel offerAttribute)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferAttributeDataModel>(offerAttribute);
                db.Complete();
            }
        }

        public bool DoesOpenApplicationExist(int applicationNumber)
        {
            var doesOpenApplicationExistStatment = new DoesOpenApplicationExistStatement(applicationNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.Select(doesOpenApplicationExistStatment);
                return results.First() == 1;
            }
        }

        public int SaveApplicationMailingAddress(ApplicationMailingAddressModel model, int clientKey, int addressKey)
        {
            var offerMailingAddressDataModel = new OfferMailingAddressDataModel(model.ApplicationNumber, addressKey, model.OnlineStatementRequired,
                (int)model.OnlineStatementFormat, (int)model.CorrespondenceLanguage, clientKey, (int)model.CorrespondenceMedium);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferMailingAddressDataModel>(offerMailingAddressDataModel);
                db.Complete();
                return offerMailingAddressDataModel.OfferMailingAddressKey;
            }
        }

        public IEnumerable<OfferMailingAddressDataModel> GetApplicationMailingAddress(int applicationNumber)
        {
            IEnumerable<OfferMailingAddressDataModel> offerMailingAddress = Enumerable.Empty<OfferMailingAddressDataModel>();
            GetApplicationMailingAddressStatement appMaillingAddressStatement = new GetApplicationMailingAddressStatement(applicationNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                offerMailingAddress = db.Select<OfferMailingAddressDataModel>(appMaillingAddressStatement);
            }
            return offerMailingAddress;
        }

        public bool DoesApplicationMailingAddressExist(int applicationNumber)
        {
            var doesApplicationMailingAddressExist = new DoesApplicationMailingAddressExistStatement(applicationNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.Select(doesApplicationMailingAddressExist);
                return results.First() == 1;
            }
        }

        public bool DoesApplicationExist(int applicationNumber)
        {
            bool response = false;
            DoesApplicationExistStatement doesApplicationExistQuery = new DoesApplicationExistStatement(applicationNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne<int>(doesApplicationExistQuery);
                response = (results > 0);
            }
            return response;
        }

        public int SaveApplicationDebitOrder(Interfaces.ApplicationDomain.Models.ApplicationDebitOrderModel applicationDebitOrderModel, int bankAccountKey)
        {
            int percentage = 0;
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                var offerDebitOrderDataModel = new OfferDebitOrderDataModel(applicationDebitOrderModel.ApplicationNumber, bankAccountKey, percentage, applicationDebitOrderModel.DebitOrderDay,
                    (int)applicationDebitOrderModel.PaymentType);
                db.Insert<OfferDebitOrderDataModel>(offerDebitOrderDataModel);
                db.Complete();
                return offerDebitOrderDataModel.OfferDebitOrderKey;
            }
        }

        public IEnumerable<OfferDebitOrderDataModel> GetApplicationDebitOrder(int applicationNumber)
        {
            GetApplicationDebitOrderStatement applicationDebitOrderQuery = new GetApplicationDebitOrderStatement(applicationNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select<OfferDebitOrderDataModel>(applicationDebitOrderQuery);
            }
        }

        public OfferInformationVariableLoanDataModel GetApplicationInformationVariableLoan(int applicationInformationNumber)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.GetByKey<OfferInformationVariableLoanDataModel, int>(applicationInformationNumber);
            }
        }

        public OfferInformationDataModel GetLatestApplicationOfferInformation(int applicationNumber)
        {
            var statement = new GetLatestApplicationOfferInformationStatement(applicationNumber);
            OfferInformationDataModel offerInformationDataModel = null;

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                offerInformationDataModel = db.SelectOne<OfferInformationDataModel>(statement);
            }

            return offerInformationDataModel;
        }

        public void UpdateApplicationInformationVariableLoan(OfferInformationVariableLoanDataModel offerInformationVariableLoanDataModel)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<OfferInformationVariableLoanDataModel>(offerInformationVariableLoanDataModel);
                db.Complete();
            }
        }

        public int SaveExternalVendorOfferRole(OfferRoleDataModel offerRole)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert<OfferRoleDataModel>(offerRole);
                db.Complete();
                return offerRole.OfferRoleKey;
            }
        }

        public VendorModel GetExternalVendorForVendorCode(string vendorCode)
        {
            GetExternalVendorForVendorCodeStatement getLegalEntityKeyForVendorQuery = new GetExternalVendorForVendorCodeStatement(vendorCode);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select<VendorModel>(getLegalEntityKeyForVendorQuery).FirstOrDefault();
            }
        }

        public void LinkPropertyToApplication(int applicationNumber, int propertyKey)
        {
            LinkPropertyToApplicationStatement query = new LinkPropertyToApplicationStatement(applicationNumber, propertyKey);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update<OfferMortgageLoanDataModel>(query);
                db.Complete();
            }
        }

        public bool DoesOpenApplicationExistForPropertyAndClient(int propertyKey, string clientIDNumber)
        {
            bool response = false;
            DoesOpenApplicationExistForPropertyAndClientStatement query = new DoesOpenApplicationExistForPropertyAndClientStatement(propertyKey, clientIDNumber);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne<int>(query);
                response = (results > 0);
            }
            return response;
        }

        public void SaveOfferInformationQuickCash(OfferInformationQuickCashDataModel offerInformationQuickCashDataModel)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(offerInformationQuickCashDataModel);
                db.Complete();
            }
        }

        public decimal GetMinimumLoanAmountForMortgageLoanPurpose(MortgageLoanPurpose mortgageLoanPurpose)
        {
            var results = 0M;
            GetMinimumLoanAmountForApplicationStatement query = new GetMinimumLoanAmountForApplicationStatement((int)mortgageLoanPurpose);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                results = db.SelectOne<decimal>(query);
            }
            return results;
        }

        public OfferDataModel GetApplication(int applicationNumber)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.GetByKey<OfferDataModel, int>(applicationNumber);
            }
        }
    }
}