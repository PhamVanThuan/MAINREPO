using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// The Control Group DAO Object specifies what type of control is being used.
    /// </summary>
    public partial class ControlGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ControlGroup_DAO>, IControlGroup
    {
        public ControlGroup(SAHL.Common.BusinessModel.DAO.ControlGroup_DAO ControlGroup)
            : base(ControlGroup)
        {
            this._DAO = ControlGroup;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}