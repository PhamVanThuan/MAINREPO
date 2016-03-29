using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO
    /// </summary>
    public partial interface IRateAdjustmentElement : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.ElementMinValue
        /// </summary>
        Double? ElementMinValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.ElementMaxValue
        /// </summary>
        Double? ElementMaxValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.ElementText
        /// </summary>
        System.String ElementText
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.RateAdjustmentValue
        /// </summary>
        System.Double RateAdjustmentValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.EffectiveDate
        /// </summary>
        System.DateTime EffectiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.RateAdjustmentElementType
        /// </summary>
        IRateAdjustmentElementType RateAdjustmentElementType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.RateAdjustmentGroup
        /// </summary>
        IRateAdjustmentGroup RateAdjustmentGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.FinancialAdjustmentTypeSource
        /// </summary>
        IFinancialAdjustmentTypeSource FinancialAdjustmentTypeSource
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.RuleItem
        /// </summary>
        IRuleItem RuleItem
        {
            get;
            set;
        }
    }
}