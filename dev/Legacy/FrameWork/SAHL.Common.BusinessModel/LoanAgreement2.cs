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
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class LoanAgreement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LoanAgreement_DAO>, ILoanAgreement
	{
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("MortgageLoanAgreementBondValue");
            Rules.Add("MortgageLoanAgreementSum");
            Rules.Add("MortgageLoanAgreementAmount");
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime AgreementDate
        {
            get { return _DAO.AgreementDate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Double Amount
        {
            get { return _DAO.Amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String UserName
        {
            get { return _DAO.UserName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ChangeDate
        {
            get { return _DAO.ChangeDate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IBond Bond
        {
            get
            {
                if (null == _DAO.Bond) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBond, Bond_DAO>(_DAO.Bond);
                }
            }
        }
    
    }
}


