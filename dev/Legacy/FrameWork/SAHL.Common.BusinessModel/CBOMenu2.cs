using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class CBOMenu : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CBOMenu_DAO>, ICBOMenu
    {
        /// <summary>
        ///
        /// </summary>
        private SortedDAOEventList<CBOMenu_DAO, ICBOMenu, CBOMenu> _ChildMenus;

        /// <summary>
        ///
        /// </summary>
        public IEventList<ICBOMenu> ChildMenus
        {
            get
            {
                if (null == _ChildMenus)
                {
                    if (null == _DAO.ChildMenus)
                        _DAO.ChildMenus = new List<CBOMenu_DAO>();
                    _ChildMenus = new SortedDAOEventList<CBOMenu_DAO, ICBOMenu, CBOMenu>(_DAO.ChildMenus, "Key", "Sequence", true);
                    _ChildMenus.BeforeAdd += new EventListHandler(OnChildMenus_BeforeAdd);
                    _ChildMenus.BeforeRemove += new EventListHandler(OnChildMenus_BeforeRemove);
                    _ChildMenus.AfterAdd += new EventListHandler(OnChildMenus_AfterAdd);
                    _ChildMenus.AfterRemove += new EventListHandler(OnChildMenus_AfterRemove);
                }
                return _ChildMenus;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private SortedDAOEventList<ContextMenu_DAO, IContextMenu, ContextMenu> _ContextMenus;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IContextMenu> ContextMenus
        {
            get
            {
                if (null == _ContextMenus)
                {
                    if (null == _DAO.ContextMenus)
                        _DAO.ContextMenus = new List<ContextMenu_DAO>();
                    _ContextMenus = new SortedDAOEventList<ContextMenu_DAO, IContextMenu, ContextMenu>(_DAO.ContextMenus, "Key", "Sequence", true);
                    _ContextMenus.BeforeAdd += new EventListHandler(OnContextMenus_BeforeAdd);
                    _ContextMenus.BeforeRemove += new EventListHandler(OnContextMenus_BeforeRemove);
                    _ContextMenus.AfterAdd += new EventListHandler(OnContextMenus_AfterAdd);
                    _ContextMenus.AfterRemove += new EventListHandler(OnContextMenus_AfterRemove);
                }
                return _ContextMenus;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnChildMenus_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnChildMenus_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnContextMenus_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnContextMenus_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnInputGenericTypes_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnInputGenericTypes_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnChildMenus_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnChildMenus_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnContextMenus_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnContextMenus_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnInputGenericTypes_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnInputGenericTypes_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}