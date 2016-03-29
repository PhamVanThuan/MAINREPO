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
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.NodeType
        /// </summary>
        System.Char NodeType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.URL
        /// </summary>
        System.String URL
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.StatementNameKey
        /// </summary>
        System.String StatementNameKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.Sequence
        /// </summary>
        System.Int32 Sequence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.MenuIcon
        /// </summary>
        System.String MenuIcon
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.Feature
        /// </summary>
        IFeature Feature
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.HasOriginationSource
        /// </summary>
        System.Boolean HasOriginationSource
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.IsRemovable
        /// </summary>
        System.Boolean IsRemovable
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.ExpandLevel
        /// </summary>
        System.Int32 ExpandLevel
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.IncludeParentHeaderIcons
        /// </summary>
        System.Boolean IncludeParentHeaderIcons
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.ParentMenu
        /// </summary>
        ICBOMenu ParentMenu
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.InputGenericTypes
        /// </summary>
        IEventList<IInputGenericType> InputGenericTypes
        {
            get;
        }
    }
}