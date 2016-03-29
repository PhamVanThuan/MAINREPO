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
    /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO
    /// </summary>
    public partial class ContextMenu : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ContextMenu_DAO>, IContextMenu
    {
        public ContextMenu(SAHL.Common.BusinessModel.DAO.ContextMenu_DAO ContextMenu)
            : base(ContextMenu)
        {
            this._DAO = ContextMenu;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.CBOMenu
        /// </summary>
        public ICBOMenu CBOMenu
        {
            get
            {
                if (null == _DAO.CBOMenu) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICBOMenu, CBOMenu_DAO>(_DAO.CBOMenu);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CBOMenu = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CBOMenu = (CBOMenu_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.URL
        /// </summary>
        public String URL
        {
            get { return _DAO.URL; }
            set { _DAO.URL = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.Feature
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
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.Sequence
        /// </summary>
        public Int32 Sequence
        {
            get { return _DAO.Sequence; }
            set { _DAO.Sequence = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.ChildMenus
        /// </summary>
        private DAOEventList<ContextMenu_DAO, IContextMenu, ContextMenu> _ChildMenus;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.ChildMenus
        /// </summary>
        public IEventList<IContextMenu> ChildMenus
        {
            get
            {
                if (null == _ChildMenus)
                {
                    if (null == _DAO.ChildMenus)
                        _DAO.ChildMenus = new List<ContextMenu_DAO>();
                    _ChildMenus = new DAOEventList<ContextMenu_DAO, IContextMenu, ContextMenu>(_DAO.ChildMenus);
                    _ChildMenus.BeforeAdd += new EventListHandler(OnChildMenus_BeforeAdd);
                    _ChildMenus.BeforeRemove += new EventListHandler(OnChildMenus_BeforeRemove);
                    _ChildMenus.AfterAdd += new EventListHandler(OnChildMenus_AfterAdd);
                    _ChildMenus.AfterRemove += new EventListHandler(OnChildMenus_AfterRemove);
                }
                return _ChildMenus;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ContextMenu_DAO.ParentMenu
        /// </summary>
        public IContextMenu ParentMenu
        {
            get
            {
                if (null == _DAO.ParentMenu) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IContextMenu, ContextMenu_DAO>(_DAO.ParentMenu);
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
                    _DAO.ParentMenu = (ContextMenu_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ChildMenus = null;
        }
    }
}