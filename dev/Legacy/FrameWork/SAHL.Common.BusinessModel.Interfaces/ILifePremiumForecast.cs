using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO
    /// </summary>
    public partial interface ILifePremiumForecast : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.LoanYear
        /// </summary>
        System.Int16 LoanYear
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.Age
        /// </summary>
        System.Int16 Age
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.SumAssured
        /// </summary>
        System.Double SumAssured
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.MonthlyPremium
        /// </summary>
        System.Double MonthlyPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.YearlyPremium
        /// </summary>
        System.Double YearlyPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.MonthlyComm
        /// </summary>
        System.Double MonthlyComm
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.EntryDate
        /// </summary>
        System.DateTime EntryDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.LifePolicy
        /// </summary>
        IAccountLifePolicy LifePolicy
        {
            get;
            set;
        }
    }
}