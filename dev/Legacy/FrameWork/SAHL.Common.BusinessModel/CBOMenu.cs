using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO
    /// </summary>
    public partial class CBOMenu : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CBOMenu_DAO>, ICBOMenu
    {
        public CBOMenu(SAHL.Common.BusinessModel.DAO.CBOMenu_DAO CBOMenu)
            : base(CBOMenu)
        {
            this._DAO = CBOMenu;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.NodeType
        /// </summary>
        public Char NodeType
        {
            get { return _DAO.NodeType; }
            set { _DAO.NodeType = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.URL
        /// </summary>
        public String URL
        {
            get { return _DAO.URL; }
            set { _DAO.URL = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.StatementNameKey
        /// </summary>
        public String StatementNameKey
        {
            get { return _DAO.StatementNameKey; }
            set { _DAO.StatementNameKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.Sequence
        /// </summary>
        public Int32 Sequence
        {
            get { return _DAO.Sequence; }
            set { _DAO.Sequence = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.MenuIcon
        /// </summary>
        public String MenuIcon
        {
            get { return _DAO.MenuIcon; }
            set { _DAO.MenuIcon = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.Feature
        /// </summary>
        public IFeature Feature
        {
            get
            {
                if (null == _DAO.Feature) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFeature, Feature_DAO>(_DAO.Feature);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Feature = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Feature = (Feature_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.GenericKeyType
        /// </summary>
        public IGenericKeyType GenericKeyType
        {
            get
            {
                if (null == _DAO.GenericKeyType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GenericKeyType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.HasOriginationSource
        /// </summary>
        public Boolean HasOriginationSource
        {
            get { return _DAO.HasOriginationSource; }
            set { _DAO.HasOriginationSource = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.IsRemovable
        /// </summary>
        public Boolean IsRemovable
        {
            get { return _DAO.IsRemovable; }
            set { _DAO.IsRemovable = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.ExpandLevel
        /// </summary>
        public Int32 ExpandLevel
        {
            get { return _DAO.ExpandLevel; }
            set { _DAO.ExpandLevel = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.IncludeParentHeaderIcons
        /// </summary>
        public Boolean IncludeParentHeaderIcons
        {
            get { return _DAO.IncludeParentHeaderIcons; }
            set { _DAO.IncludeParentHeaderIcons = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.ParentMenu
        /// </summary>
        public ICBOMenu ParentMenu
        {
            get
            {
                if (null == _DAO.ParentMenu) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICBOMenu, CBOMenu_DAO>(_DAO.ParentMenu);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ParentMenu = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ParentMenu = (CBOMenu_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.InputGenericTypes
        /// </summary>
        private DAOEventList<InputGenericType_DAO, IInputGenericType, InputGenericType> _InputGenericTypes;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CBOMenu_DAO.InputGenericTypes
        /// </summary>
        public IEventList<IInputGenericType> InputGenericTypes
        {
            get
            {
                if (null == _InputGenericTypes)
                {
                    if (null == _DAO.InputGenericTypes)
                        _DAO.InputGenericTypes = new List<InputGenericType_DAO>();
                    _InputGenericTypes = new DAOEventList<InputGenericType_DAO, IInputGenericType, InputGenericType>(_DAO.InputGenericTypes);
                    _InputGenericTypes.BeforeAdd += new EventListHandler(OnInputGenericTypes_BeforeAdd);
                    _InputGenericTypes.BeforeRemove += new EventListHandler(OnInputGenericTypes_BeforeRemove);
                    _InputGenericTypes.AfterAdd += new EventListHandler(OnInputGenericTypes_AfterAdd);
                    _InputGenericTypes.AfterRemove += new EventListHandler(OnInputGenericTypes_AfterRemove);
                }
                return _InputGenericTypes;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _InputGenericTypes = null;
        }
    }
}