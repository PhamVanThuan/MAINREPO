using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO
    /// </summary>
    public partial interface ILegalEntityMarketingOptionHistory : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.LegalEntityMarketingOptionKey
        /// </summary>
        ILegalEntityMarketingOption LegalEntityMarketingOptionKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.MarketingOptionKey
        /// </summary>
        System.Int32 MarketingOptionKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.ChangeAction
        /// </summary>
        System.Char ChangeAction
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityMarketingOptionHistory_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}