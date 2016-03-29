using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO
    /// </summary>
    public partial interface ILifeCommissionTargets : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.Consultant
        /// </summary>
        System.String Consultant
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.EffectiveYear
        /// </summary>
        Int32? EffectiveYear
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.EffectiveMonth
        /// </summary>
        Int32? EffectiveMonth
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.TargetPolicies
        /// </summary>
        Int32? TargetPolicies
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.MinPoliciesToQualify
        /// </summary>
        Int32? MinPoliciesToQualify
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}