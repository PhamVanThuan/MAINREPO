using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO
    /// </summary>
    public partial interface ILifePremiumHistory : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.DeathPremium
        /// </summary>
        System.Double DeathPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.IPBPremium
        /// </summary>
        System.Double IPBPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.SumAssured
        /// </summary>
        System.Double SumAssured
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.YearlyPremium
        /// </summary>
        System.Double YearlyPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.PolicyFactor
        /// </summary>
        System.Double PolicyFactor
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.DiscountFactor
        /// </summary>
        System.Double DiscountFactor
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.MonthlyPremium
        /// </summary>
        System.Double MonthlyPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.UserName
        /// </summary>
        System.String UserName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }
    }
}