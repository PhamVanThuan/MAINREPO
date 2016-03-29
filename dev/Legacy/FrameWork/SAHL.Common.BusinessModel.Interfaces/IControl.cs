using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The Control DAO Object
    /// </summary>
    public partial interface IControl : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Control_DAO.ControlDescription
        /// </summary>
        System.String ControlDescription
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Control_DAO.ControlNumeric
        /// </summary>
        Double? ControlNumeric
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Control_DAO.ControlText
        /// </summary>
        System.String ControlText
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Control_DAO.ControlGroup
        /// </summary>
        IControlGroup ControlGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Control_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}