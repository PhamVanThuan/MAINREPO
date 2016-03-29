using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO
    /// </summary>
    public partial class AccountAttorneyInvoice : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO>, IAccountAttorneyInvoice
    {
        /// <summary>
        /// Returns the Registered Name for the LegalEntity on the Attorney.
        /// </summary>
        public string AttorneyRegisteredName
        {
            get
            {
                IRegistrationRepository regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
                IAttorney att = regRepo.GetAttorneyByKey(this.AttorneyKey);
                if (att != null && att.LegalEntity != null)
                {
                    switch ((int)att.LegalEntity.LegalEntityType.Key)
                    {
                        case (int)LegalEntityTypes.CloseCorporation:
                            return ((ILegalEntityCloseCorporation)att.LegalEntity).RegisteredName;
                        case (int)LegalEntityTypes.Company:
                            return ((ILegalEntityCompany)att.LegalEntity).RegisteredName;
                        case (int)LegalEntityTypes.Trust:
                            return ((ILegalEntityCompany)att.LegalEntity).RegisteredName;
                        default:
                            break;
                    }
                }

                return "";
            }
        }
    }
}