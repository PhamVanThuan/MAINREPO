using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Postnet Suite format.
    /// </summary>
    public partial class AddressPostnetSuite : Address, IAddressPostnetSuite
    {
        protected new SAHL.Common.BusinessModel.DAO.AddressPostnetSuite_DAO _DAO;

        public AddressPostnetSuite(SAHL.Common.BusinessModel.DAO.AddressPostnetSuite_DAO AddressPostnetSuite)
            : base(AddressPostnetSuite)
        {
            this._DAO = AddressPostnetSuite;
        }

        /// <summary>
        /// The Postnet Box Number of the Address.
        /// </summary>
        public String PrivateBagNumber
        {
            get { return _DAO.PrivateBagNumber; }
            set { _DAO.PrivateBagNumber = value; }
        }

        /// <summary>
        /// The Postnet Suite Number of the Address.
        /// </summary>
        public String SuiteNumber
        {
            get { return _DAO.SuiteNumber; }
            set { _DAO.SuiteNumber = value; }
        }

        /// <summary>
        /// The Post Office which the Address belongs to.
        /// </summary>
        public IPostOffice PostOffice
        {
            get
            {
                if (null == _DAO.PostOffice) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IPostOffice, PostOffice_DAO>(_DAO.PostOffice);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.PostOffice = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.PostOffice = (PostOffice_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}