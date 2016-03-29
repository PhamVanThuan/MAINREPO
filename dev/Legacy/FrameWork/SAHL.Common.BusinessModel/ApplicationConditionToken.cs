using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO
    /// </summary>
    public partial class ApplicationConditionToken : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO>, IApplicationConditionToken
    {
        public ApplicationConditionToken(SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO ApplicationConditionToken)
            : base(ApplicationConditionToken)
        {
            this._DAO = ApplicationConditionToken;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO.Token
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

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO.TranslatableItem
        /// </summary>
        public ITranslatableItem TranslatableItem
        {
            get
            {
                if (null == _DAO.TranslatableItem) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ITranslatableItem, TranslatableItem_DAO>(_DAO.TranslatableItem);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.TranslatableItem = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.TranslatableItem = (TranslatableItem_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO.TokenValue
        /// </summary>
        public String TokenValue
        {
            get { return _DAO.TokenValue; }
            set { _DAO.TokenValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationConditionToken_DAO.ApplicationCondition
        /// </summary>
        public IApplicationCondition ApplicationCondition
        {
            get
            {
                if (null == _DAO.ApplicationCondition) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationCondition, ApplicationCondition_DAO>(_DAO.ApplicationCondition);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationCondition = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationCondition = (ApplicationCondition_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}