using System.Collections.Generic;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from Account_DAO. Instantiated to represent a lifepolicy account.
    /// </summary>
    public partial class AccountLifePolicy : Account, IAccountLifePolicy
    {
        protected new SAHL.Common.BusinessModel.DAO.AccountLifePolicy_DAO _DAO;

        public AccountLifePolicy(SAHL.Common.BusinessModel.DAO.AccountLifePolicy_DAO AccountLifePolicy)
            : base(AccountLifePolicy)
        {
            this._DAO = AccountLifePolicy;
        }

        /// <summary>
        /// The Parent Mortgage Loan Account associated to the Life Policy.
        /// </summary>
        public IAccount ParentMortgageLoan
        {
            get
            {
                if (null == _DAO.ParentMortgageLoan) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.ParentMortgageLoan);
                }
            }
        }

        /// <summary>
        /// The Financial Service associated to the Life Policy
        /// </summary>
        public ILifePolicy LifePolicyFinancialService
        {
            get
            {
                if (null == _DAO.LifePolicyFinancialService) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILifePolicy, LifePolicy_DAO>(_DAO.LifePolicyFinancialService);
                }
            }
        }

        /// <summary>
        /// Each of the Legal Entities playing a Role on a Life Policy Account require an Insurable Interest. This relationship is defined
        /// in the LifeInsurableInterest table, which relates the Account to the Legal Entity and the type of Insurable Interest that the Legal
        /// Entity has on the Account.
        /// </summary>
        private DAOEventList<LifeInsurableInterest_DAO, ILifeInsurableInterest, LifeInsurableInterest> _LifeInsurableInterests;

        /// <summary>
        /// Each of the Legal Entities playing a Role on a Life Policy Account require an Insurable Interest. This relationship is defined
        /// in the LifeInsurableInterest table, which relates the Account to the Legal Entity and the type of Insurable Interest that the Legal
        /// Entity has on the Account.
        /// </summary>
        public IEventList<ILifeInsurableInterest> LifeInsurableInterests
        {
            get
            {
                if (null == _LifeInsurableInterests)
                {
                    if (null == _DAO.LifeInsurableInterests)
                        _DAO.LifeInsurableInterests = new List<LifeInsurableInterest_DAO>();
                    _LifeInsurableInterests = new DAOEventList<LifeInsurableInterest_DAO, ILifeInsurableInterest, LifeInsurableInterest>(_DAO.LifeInsurableInterests);
                    _LifeInsurableInterests.BeforeAdd += new EventListHandler(OnLifeInsurableInterests_BeforeAdd);
                    _LifeInsurableInterests.BeforeRemove += new EventListHandler(OnLifeInsurableInterests_BeforeRemove);
                    _LifeInsurableInterests.AfterAdd += new EventListHandler(OnLifeInsurableInterests_AfterAdd);
                    _LifeInsurableInterests.AfterRemove += new EventListHandler(OnLifeInsurableInterests_AfterRemove);
                }
                return _LifeInsurableInterests;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _LifeInsurableInterests = null;
        }
    }
}