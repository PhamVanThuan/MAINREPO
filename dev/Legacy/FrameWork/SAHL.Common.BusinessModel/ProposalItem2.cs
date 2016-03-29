using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Utils;
namespace SAHL.Common.BusinessModel

{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO
	/// </summary>
	public partial class ProposalItem : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ProposalItem_DAO>, IProposalItem
	{

        /// <summary>
        /// The number of complete months
        /// </summary>
        public int Period
        {
            get
            {				
				return _DAO.StartDate.MonthDifference(_DAO.EndDate, 1);
            }
        }

        /// <summary>
        /// The sum of the selected MaretRate value and the InterestRate 
        /// as a percentage
        /// </summary>
        public double TotalInterestRate
        {
            get
            {
                return (_DAO.MarketRate == null ? 0D : _DAO.MarketRate.Value) + _DAO.InterestRate;
            }
        }

        /// <summary>
        /// The sum of all amounts
        /// Amount + AdditionalAmount
        /// </summary>
        public double TotalPayment
        {
            get
            {
                return _DAO.Amount + _DAO.AdditionalAmount;
            }
        }
	}
}


