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
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;


namespace SAHL.Web.Views.Common.Presenters
{
    public class X2NonUserStatePresenter : SAHLCommonBasePresenter<IX2NonUserState>
    {

        public X2NonUserStatePresenter(IX2NonUserState view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            CBONode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);
            InstanceNode iNode = node as InstanceNode;
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            IInstance instance = x2Repo.GetInstanceByKey(iNode.InstanceID);

            switch (instance.State.StateType.ID)
            {
                case 1: //user
					if (instance.State.Forms.Count > 0)
					{
//						IX2Service x2Service = RepositoryFactory.GetRepository<IX2Service>();

						Navigator.Navigate(instance.State.Forms[0].Name);
					}
					else
						_view.SetText(String.Format("This is the X2NonUserState view, but the instance you are working on is at a User state: '{0}'.\nThis usually indicates that the case has moved to a state you do not have access to.", instance.State.Name), false);
                    break;
                 case 5: //archive
                     _view.SetText(String.Format("The instance you are working on has moved to an Archive state: '{0}'.\nThe instance can no longer be worked on, please select another node.", instance.State.Name), false);
                     break;
                default:
                    _view.SetText(String.Format("The instance you are working on is at a System or Non-User state: '{0}'.\nYou can click Refresh to see if it has moved to the next state, or select another node to work on.", instance.State.Name), true);
                    break;

            }
        }


    }
}
