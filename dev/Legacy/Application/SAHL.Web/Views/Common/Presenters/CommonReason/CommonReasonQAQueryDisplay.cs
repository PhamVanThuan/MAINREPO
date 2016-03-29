using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;


namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    public class CommonReasonQAQueryDisplay : SAHLCommonBasePresenter<ICommonReason>
    {
        int _generickey;
        protected CBOMenuNode _node;

        public CommonReasonQAQueryDisplay(ICommonReason view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// OnViewInitialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // get the instance node
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as InstanceNode;

            if (_node != null) _generickey = _node.GenericKey;

            _view.HistoryDisplay = true;
            _view.ShowHistoryPanel = true;
            _view.ShowUpdatePanel = false;

            IReasonRepository reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            int reasonTypeKey = Convert.ToInt32(_view.ViewAttributes["reasontypekey"]);

            IReasonType reasonType = reasonRepo.GetReasonTypeByKey(reasonTypeKey);

            switch (reasonType.GenericKeyType.Key)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.OfferInformation:
                    IApplicationInformation latestApplicationInformation = RepositoryFactory.GetRepository<IApplicationRepository>().GetApplicationByKey(_generickey).GetLatestApplicationInformation();
                    _generickey = latestApplicationInformation.Key;
                    break;
                default:
                    break;
            }

            _view.SetHistoryPanelGroupingText = lookupRepo.ReasonTypes.ObjectDictionary[Convert.ToString(reasonTypeKey)].Description;

           // int[] Reasongroups = {(int) SAHL.Common.Globals.ReasonTypeGroups.NTU, reasonTypeKey};


            //IReadOnlyEventList<IReason> reasons = reasonRepo.GetReasonByGenericKeyAndReasonTypeKey(_generickey, (int)SAHL.Common.Globals.ReasonTypes.OriginationNTU);
            IReadOnlyEventList<IReason> reasons = reasonRepo.GetReasonByGenericKeyAndReasonTypeKey(_generickey, reasonTypeKey);

            _view.BindReasonHistoryGrid(reasons);
        }
    }
}
