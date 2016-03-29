using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.InterestOnly_DAO
    /// </summary>
    public partial interface IInterestOnly : IEntityValidation, IBusinessModelObject
    {
		System.Int32 Key
		{
			get;
			set;
		}

        System.DateTime EntryDate
        {
            get;
            set;
        }

        System.DateTime MaturityDate
        {
            get;
            set;
        }

        IFinancialServiceAttribute FinancialServiceAttribute
        {
            get;
            set;
        }
    }
}