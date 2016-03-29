using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Street format.
    /// </summary>
    public partial class AddressStreet : Address, IAddressStreet
    {
        protected new SAHL.Common.BusinessModel.DAO.AddressStreet_DAO _DAO;

        public AddressStreet(SAHL.Common.BusinessModel.DAO.AddressStreet_DAO AddressStreet)
            : base(AddressStreet)
        {
            this._DAO = AddressStreet;
        }

        /// <summary>
        /// The Building Number of the Address.
        /// </summary>
        public String BuildingNumber
        {
            get { return _DAO.BuildingNumber; }
            set { _DAO.BuildingNumber = value; }
        }

        /// <summary>
        /// The Building Name of the Address.
        /// </summary>
        public String BuildingName
        {
            get { return _DAO.BuildingName; }
            set { _DAO.BuildingName = value; }
        }

        /// <summary>
        /// The Street Number of the Address.
        /// </summary>
        public String StreetNumber
        {
            get { return _DAO.StreetNumber; }
            set { _DAO.StreetNumber = value; }
        }

        /// <summary>
        /// The Street Name of the Address.
        /// </summary>
        public String StreetName
        {
            get { return _DAO.StreetName; }
            set { _DAO.StreetName = value; }
        }

        /// <summary>
        /// The Unit Number of the Address.
        /// </summary>
        public String UnitNumber
        {
            get { return _DAO.UnitNumber; }
            set { _DAO.UnitNumber = value; }
        }

        /// <summary>
        /// The Suburb which the Address belongs to.
        /// </summary>
        public ISuburb Suburb
        {
            get
            {
                if (null == _DAO.Suburb) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ISuburb, Suburb_DAO>(_DAO.Suburb);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Suburb = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Suburb = (Suburb_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}