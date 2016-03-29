using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AddressType_DAO
    /// </summary>
    public partial class AddressType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AddressType_DAO>, IAddressType
    {
        public AddressType(SAHL.Common.BusinessModel.DAO.AddressType_DAO AddressType)
            : base(AddressType)
        {
            this._DAO = AddressType;
        }

        /// <summary>
        /// The Address Type Description (Residential/Postal)
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}