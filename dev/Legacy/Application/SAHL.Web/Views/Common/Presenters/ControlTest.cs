using System;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Web.Views.Common.Interfaces;
using System.Data;
using DevExpress.Web.ASPxTreeList;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// EstateAgent Base
    /// </summary>
    public class ControlTest : SAHLCommonBasePresenter<IControlTest>
    {


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ControlTest(IControlTest view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!_view.ShouldRunPage) 
                return;

            //get OS for EA Channel -- 2723
            IControl ct = RepositoryFactory.GetRepository<IControlRepository>().GetControlByDescription("EstateAgentChannelRoot");
            int osRootKey = Convert.ToInt32(ct.ControlNumeric.Value);

            _view.TreeNodeDragged += new TreeListNodeDragEventHandler(TreeNodeDragged);
            _view.RemoveButtonClicked += new EventHandler(RemoveButtonClicked);

            DataSet orgStructListDS = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetOrganisationStructureAllDSForKey(osRootKey);

            _view.BindOrganisationStructure(orgStructListDS);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage) 
                return;

        }


        void RemoveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                _view.Messages.Add(new DomainMessage("Delete Error", "Delete Error"));
                throw new Exception();
            }
            catch (Exception)
            {
                if (_view.IsValid)
                    throw;
            }
            finally
            {
            }
        }

        void TreeNodeDragged(object sender, TreeListNodeDragEventArgs e)
        {
            try
            {
                _view.Messages.Add(new DomainMessage("Drag Error", "Drag Error"));
                throw new Exception();
            }
            catch (Exception)
            {
                if (_view.IsValid)
                    throw;
            }
            finally
            {
            }

            e.Handled = true;
        }

        #endregion

        #region Methods


        #endregion

        #region Properties

        private IOrganisationStructureRepository _osRepo;

        public IOrganisationStructureRepository OSRepo
        {
            get
            {
                if (_osRepo == null)
                    _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

                return _osRepo;
            }
        }


        #endregion
    }
}
