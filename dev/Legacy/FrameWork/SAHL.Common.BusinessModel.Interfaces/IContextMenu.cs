using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO
    /// </summary>
    public partial interface IContextMenu : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.CBOMenu
        /// </summary>
        ICBOMenu CBOMenu
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.URL
        /// </summary>
        System.String URL
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.Feature
        /// </summary>
        IFeature Feature
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.Sequence
        /// </summary>
        System.Int32 Sequence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.ChildMenus
        /// </summary>
        IEventList<IContextMenu> ChildMenus
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.ParentMenu
        /// </summary>
        IContextMenu ParentMenu
        {
            get;
            set;
        }
    }
}