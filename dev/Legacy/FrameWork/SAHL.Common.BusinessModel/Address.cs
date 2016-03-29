using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Address_DAO is the base class from which the format specific addresses are derived. The discrimination is based on the
    /// Address Format (Street, Box etc).
    /// </summary>
    public partial class Address : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Address_DAO>, IAddress
    {
        public Address(SAHL.Common.BusinessModel.DAO.Address_DAO Address)
            : base(Address)
        {
            this._DAO = Address;
        }

        /// <summary>
        /// The UserID of the last person who updated information on the Address.
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// The date when the Address record was last changed.
        /// </summary>
        public DateTime? ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// The Country property of an address.
        /// </summary>
        public String RRR_CountryDescription
        {
            get { return _DAO.RRR_CountryDescription; }
        }

        /// <summary>
        /// The Province property of an address.
        /// </summary>
        public String RRR_ProvinceDescription
        {
            get { return _DAO.RRR_ProvinceDescription; }
        }

        /// <summary>
        /// The City property of an address.
        /// </summary>
        public String RRR_CityDescription
        {
            get { return _DAO.RRR_CityDescription; }
        }

        /// <summary>
        /// The Suburb property of an address.
        /// </summary>
        public String RRR_SuburbDescription
        {
            get { return _DAO.RRR_SuburbDescription; }
        }

        /// <summary>
        /// The Postal Code property of an address.
        /// </summary>
        public String RRR_PostalCode
        {
            get { return _DAO.RRR_PostalCode; }
        }
    }
}