using System;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
	public class CallSummaryDisplay : CallSummary
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CallSummaryDisplay(ICallSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

			IHelpDeskRepository hdRepo = RepositoryFactory.GetRepository<IHelpDeskRepository>();

            _view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridIndexChanged);

			InstanceNode _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
			if (_node == null)
				throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

			_instanceID = _node.InstanceID;

			if (_node.GenericKeyTypeKey == (int)GenericKeyTypes.LegalEntity)
			{
				_legalEntityKey = _node.GenericKey;
				_view.SetLegalEntityKey = _legalEntityKey;
			}

			_helpDeskQueryList = hdRepo.GetHelpDeskCallSummaryByLegalEntityKey(_legalEntityKey);

			if (_helpDeskQueryList != null)
			{
				_view.BindGrid(_helpDeskQueryList);

				if (!_view.IsPostBack && _helpDeskQueryList.Count > 0)
					_view.BindLabels(_helpDeskQueryList[0]);
			}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.SetupControlsForDisplay();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnGridIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                _view.BindLabels(_helpDeskQueryList[Convert.ToInt32( e.Key )]);
            }
        }
    }
}
