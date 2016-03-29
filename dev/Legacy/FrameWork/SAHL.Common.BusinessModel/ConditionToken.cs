using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ConditionToken_DAO
    /// </summary>
    public partial class ConditionToken : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ConditionToken_DAO>, IConditionToken
    {
        public ConditionToken(SAHL.Common.BusinessModel.DAO.ConditionToken_DAO ConditionToken)
            : base(ConditionToken)
        {
            this._DAO = ConditionToken;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionToken_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionToken_DAO.Condition
        /// </summary>
        public ICondition Condition
        {
            get
            {
                if (null == _DAO.Condition) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICondition, Condition_DAO>(_DAO.Condition);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Condition = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Condition = (Condition_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionToken_DAO.Token
        /// </summary>
        public IToken Token
        {
            get
            {
                if (null == _DAO.Token) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IToken, Token_DAO>(_DAO.Token);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Token = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Token = (Token_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}