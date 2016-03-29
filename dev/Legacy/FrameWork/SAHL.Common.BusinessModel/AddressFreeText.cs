using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Free Text format.
    /// </summary>
    public partial class AddressFreeText : Address, IAddressFreeText
    {
        protected new SAHL.Common.BusinessModel.DAO.AddressFreeText_DAO _DAO;

        public AddressFreeText(SAHL.Common.BusinessModel.DAO.AddressFreeText_DAO AddressFreeText)
            : base(AddressFreeText)
        {
            this._DAO = AddressFreeText;
        }

        /// <summary>
        /// Free Text Line 1
        /// </summary>
        public String FreeText1
        {
            get { return _DAO.FreeText1; }
            set { _DAO.FreeText1 = value; }
        }

        /// <summary>
        /// Free Text Line 2
        /// </summary>
        public String FreeText2
        {
            get { return _DAO.FreeText2; }
            set { _DAO.FreeText2 = value; }
        }

        /// <summary>
        /// Free Text Line 3
        /// </summary>
        public String FreeText3
        {
            get { return _DAO.FreeText3; }
            set { _DAO.FreeText3 = value; }
        }

        /// <summary>
        /// Free Text Line 4
        /// </summary>
        public String FreeText4
        {
            get { return _DAO.FreeText4; }
            set { _DAO.FreeText4 = value; }
        }

        /// <summary>
        /// Free Text Line 5
        /// </summary>
        public String FreeText5
        {
            get { return _DAO.FreeText5; }
            set { _DAO.FreeText5 = value; }
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