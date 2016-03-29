using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ClientAnswerValue_DAO
    /// </summary>
    public partial class ClientAnswerValue : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ClientAnswerValue_DAO>, IClientAnswerValue
    {
        public ClientAnswerValue(SAHL.Common.BusinessModel.DAO.ClientAnswerValue_DAO ClientAnswerValue)
            : base(ClientAnswerValue)
        {
            this._DAO = ClientAnswerValue;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswerValue_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswerValue_DAO.Value
        /// </summary>
        public String Value
        {
            get { return _DAO.Value; }
            set { _DAO.Value = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswerValue_DAO.ClientAnswer
        /// </summary>
        public IClientAnswer ClientAnswer
        {
            get
            {
                if (null == _DAO.ClientAnswer) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IClientAnswer, ClientAnswer_DAO>(_DAO.ClientAnswer);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ClientAnswer = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ClientAnswer = (ClientAnswer_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}