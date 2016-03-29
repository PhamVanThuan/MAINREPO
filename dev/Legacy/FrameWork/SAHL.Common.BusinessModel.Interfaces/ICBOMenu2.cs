using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO
    /// </summary>
    public partial interface ICBOMenu : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.ChildMenus
        /// </summary>
        IEventList<ICBOMenu> ChildMenus
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.ContextMenus
        /// </summary>
        IEventList<IContextMenu> ContextMenus
        {
            get;
        }
    }
}