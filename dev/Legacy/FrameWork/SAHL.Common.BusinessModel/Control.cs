using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// The Control DAO Object
    /// </summary>
    public partial class Control : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Control_DAO>, IControl
    {
        public Control(SAHL.Common.BusinessModel.DAO.Control_DAO Control)
            : base(Control)
        {
            this._DAO = Control;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Control_DAO.ControlDescription
        /// </summary>
        public String ControlDescription
        {
            get { return _DAO.ControlDescription; }
            set { _DAO.ControlDescription = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Control_DAO.ControlNumeric
        /// </summary>
        public Double? ControlNumeric
        {
            get { return _DAO.ControlNumeric; }
            set { _DAO.ControlNumeric = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Control_DAO.ControlText
        /// </summary>
        public String ControlText
        {
            get { return _DAO.ControlText; }
            set { _DAO.ControlText = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Control_DAO.ControlGroup
        /// </summary>
        public IControlGroup ControlGroup
        {
            get
            {
                if (null == _DAO.ControlGroup) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IControlGroup, ControlGroup_DAO>(_DAO.ControlGroup);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ControlGroup = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ControlGroup = (ControlGroup_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Control_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}