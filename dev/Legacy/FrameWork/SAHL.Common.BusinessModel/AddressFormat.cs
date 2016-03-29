using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AddressFormat_DAO
    /// </summary>
    public partial class AddressFormat : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AddressFormat_DAO>, IAddressFormat
    {
        public AddressFormat(SAHL.Common.BusinessModel.DAO.AddressFormat_DAO AddressFormat)
            : base(AddressFormat)
        {
            this._DAO = AddressFormat;
        }

        /// <summary>
        /// The description of the Address Format (Street/Cluster Box etc)
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