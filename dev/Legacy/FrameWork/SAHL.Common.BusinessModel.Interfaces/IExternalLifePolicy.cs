using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO
	/// </summary>
	public partial interface IExternalLifePolicy : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.PolicyNumber
		/// </summary>
		System.String PolicyNumber
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.Insurer
		/// </summary>
		IInsurer Insurer
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.CommencementDate
		/// </summary>
		System.DateTime CommencementDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.LifePolicyStatus
		/// </summary>
		ILifePolicyStatus LifePolicyStatus
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.CloseDate
		/// </summary>
		System.DateTime? CloseDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.SumInsured
		/// </summary>
		System.Double SumInsured
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.PolicyCeded
		/// </summary>
		System.Boolean PolicyCeded
		{
			get;
			set;
		}
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }
    }
}


