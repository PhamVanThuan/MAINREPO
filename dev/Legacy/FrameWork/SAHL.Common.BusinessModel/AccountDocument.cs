using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO
    /// </summary>
    public partial class AccountDocument : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountDocument_DAO>, IAccountDocument
    {
        public AccountDocument(SAHL.Common.BusinessModel.DAO.AccountDocument_DAO AccountDocument)
            : base(AccountDocument)
        {
            this._DAO = AccountDocument;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.LegalEntityKey
        /// </summary>
        public Int32? LegalEntityKey
        {
            get { return _DAO.LegalEntityKey; }
            set { _DAO.LegalEntityKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.DocumentReceived
        /// </summary>
        public Boolean? DocumentReceived
        {
            get { return _DAO.DocumentReceived; }
            set { _DAO.DocumentReceived = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.DocumentReceivedDate
        /// </summary>
        public DateTime? DocumentReceivedDate
        {
            get { return _DAO.DocumentReceivedDate; }
            set { _DAO.DocumentReceivedDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.DocumentReceivedBy
        /// </summary>
        public String DocumentReceivedBy
        {
            get { return _DAO.DocumentReceivedBy; }
            set { _DAO.DocumentReceivedBy = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.DocumentVersionNumber
        /// </summary>
        public String DocumentVersionNumber
        {
            get { return _DAO.DocumentVersionNumber; }
            set { _DAO.DocumentVersionNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.Account
        /// </summary>
        public IAccount Account
        {
            get
            {
                if (null == _DAO.Account) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Account = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Account = (Account_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountDocument_DAO.DocumentType
        /// </summary>
        public IDocumentType DocumentType
        {
            get
            {
                if (null == _DAO.DocumentType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDocumentType, DocumentType_DAO>(_DAO.DocumentType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.DocumentType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.DocumentType = (DocumentType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}