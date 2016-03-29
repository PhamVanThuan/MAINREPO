using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.UI;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class ApplicantsBase : SAHLCommonBasePresenter<IApplicants>
    {
        protected CBOMenuNode _node;
        private ILookupRepository _lookupRepository;
        protected ILegalEntity _legalEntity;

        protected ILookupRepository LookupRepository
        {
            get { return _lookupRepository; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicantsBase(IApplicants view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnGridLegalEntitySelected += new KeyChangedEventHandler(OnGridLegalEntitySelected);

            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
        }

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

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnGridLegalEntitySelected(object sender, KeyChangedEventArgs e)
        {
             ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _legalEntity = legalEntityRepo.GetLegalEntityByKey(_view.SelectedLegalEntityKey);
            _view.BindLegalEntityDetails(_legalEntity);
        }
    }
}
