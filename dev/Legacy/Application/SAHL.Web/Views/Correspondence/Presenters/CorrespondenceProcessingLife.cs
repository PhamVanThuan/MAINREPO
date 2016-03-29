using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Correspondence.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Web.Views.Correspondence.Presenters.Correspondence;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
    /// <summary>
    /// 
    /// </summary>
    public class CorrespondenceProcessingLife : CorrespondenceProcessingBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CorrespondenceProcessingLife(ICorrespondenceProcessing view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
 
            if (!_view.ShouldRunPage)
                return;

            // set properties
            _view.ShowLifeWorkFlowHeader = false;
            _view.ShowCancelButton = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnSendButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();

            try
            {
                // render and save the correspondence in the background.
                base.OnSendButtonClicked(sender, e);

                // this will check if a rule error/warning has been thrown in the base class
                if (!_view.IsValid)
                    return;

                // write the stage transition  record
                IStageDefinitionRepository stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                stageDefinitionRepo.SaveStageTransition(base.StageDefinitionGenericKey, Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeAdmin), SAHL.Common.Constants.StageDefinitionConstants.DocumentProcessed, _view.CorrespondenceDocuments, CurrentADUser);

                ts.VoteCommit();

                _view.Navigator.Navigate("Submit");

            }
            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                ts.Dispose();
            }

        }
    }
}
