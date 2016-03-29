using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Presenters.CommonReason;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;

using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceStrategies;

namespace SAHL.Web.Views.Life.Presenters
{
    public class LifeNTU : SAHLCommonBasePresenter<ILifeNTU>
    {
        private InstanceNode _node;
        private IApplicationLife _applicationLife;
        private IApplicationRepository _applicationRepo;
        private IReasonRepository _reasonRepo;
        private IStageDefinitionRepository _stageDefinitionRepo;
        private ICorrespondenceRepository _correspondenceRepo;
        private IReportRepository _reportRepo;
        private ILookupRepository _lookupRepo;
        private ILifeRepository _lifeRepo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LifeNTU(ILifeNTU view, SAHLCommonBaseController controller)
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

            if (!_view.ShouldRunPage) 
                return;

            // get the instance node
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
            _stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            _correspondenceRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
            _reportRepo = RepositoryFactory.GetRepository<IReportRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();

            // get the life application
            _applicationLife = _applicationRepo.GetApplicationLifeByKey(_node.GenericKey);

            // get the list of ntu reason definitions
            IReadOnlyEventList<IReasonDefinition> reasonDefinitions  = _reasonRepo.GetReasonDefinitionsByReasonType(SAHL.Common.Globals.ReasonTypes.LifeNTU);
            IDictionary<int, string> dicDefinitions = new Dictionary<int, string>();
            foreach (IReasonDefinition def in reasonDefinitions)
            {
                dicDefinitions.Add(def.Key, def.ReasonDescription.Description);
            }
            
            // bind the reason definitions
            _view.BindReasonDefinitions(dicDefinitions);
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
            
            _view.CancelButtonVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCancelButtonClicked(object sender, EventArgs e)
        {
            // Cancel the activity
            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;

            IX2Info x2Info = svc.GetX2Info(_view.CurrentPrincipal);
            if (x2Info != null && !String.IsNullOrEmpty(x2Info.ActivityName))
                svc.CancelActivity(_view.CurrentPrincipal);

            // Navigate back to the calling page
            _view.Navigator.Navigate(GlobalCacheData[ViewConstants.NavigateTo].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // validate selection
            if (_view.SelectedReasonDefinitionKey==-1)
                _view.Messages.Add(new Error("A reason must be selected before submitting.", "A reason must be selected before submitting."));

            if (_view.IsValid)
            {
                TransactionScope txn = new TransactionScope();

                try
                {
                    // complete the x2 activity - Life helper will update the status of the offer to NTU 
                    X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                    X2ServiceResponse x2Response = svc.CompleteActivity(_view.CurrentPrincipal, null, false);
                    if (x2Response.IsError == true)
                        throw new Exception();

                    ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                    IADUser adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);

                    // get the selected reason definition
                    IReasonDefinition reasonDefinition = _reasonRepo.GetReasonDefinitionByKey(_view.SelectedReasonDefinitionKey);
                    
                    // save the stage transition here and not in the map, so we can use the reason in the stage transition description
                    IStageTransition stageTransition = _stageDefinitionRepo.SaveStageTransition(_applicationLife.Key, (int)SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination, SAHL.Common.Constants.StageDefinitionConstants.ApplicationNTUd, reasonDefinition.ReasonDescription.Description, adUser);

                    // populate the reason data
                    IReason reason = _reasonRepo.CreateEmptyReason();
                    reason.GenericKey = _applicationLife.Key;
                    reason.ReasonDefinition = reasonDefinition;
                    reason.StageTransition = stageTransition;
                    //save the reason record
                    _reasonRepo.SaveReason(reason);

                    // insert the NTU Letter into the correspondence table
                    // do not produce a NTU Letter if the Reason is "Loan NTU'd"
                    if (reasonDefinition.ReasonDescription.Description != "Loan NTU'd")
                    {
                        // Get the ReportData using the CorrespondenceStrategyWorker
                        List<ReportData> reportDataList = CorrespondenceStrategyWorker.GetReportData("Life_Correspondence_NTULetter", _applicationLife.Account.OriginationSourceProduct.Key);
                        if (reportDataList.Count > 0)
                        {
                            ReportData reportData = reportDataList[0];

                            #region set the parameter values
                            // get the policyholder details
                            int legalEntityKey = -1, addressKey = -1;
                            _lifeRepo.GetPolicyHolderDetails(_applicationLife.Account.Key, out legalEntityKey, out addressKey);
                            ILegalEntity legalEntity = RepositoryFactory.GetRepository<ILegalEntityRepository>().GetLegalEntityByKey(legalEntityKey);
                            // set the parameter values
                            foreach (ReportDataParameter parm in reportData.ReportParameters)
                            {
                                if (parm.ParameterName.ToLower() == reportData.GenericKeyParameterName.ToLower())
                                    parm.ParameterValue = _applicationLife.Account.Key.ToString();
                                else if (parm.ParameterName.ToLower() == reportData.MailingTypeParameterName.ToLower())
                                    parm.ParameterValue = Convert.ToString((int)SAHL.Common.Globals.CorrespondenceMediums.Post);
                                else if (parm.ParameterName.ToLower() == reportData.LegalEntityParameterName.ToLower())
                                    parm.ParameterValue = legalEntityKey > 0 ? legalEntityKey.ToString() : null;
                                else if (parm.ParameterName.ToLower() == reportData.AddressParameterName.ToLower())
                                    parm.ParameterValue = addressKey > 0 ? addressKey.ToString() : null;
                            }
                            #endregion

                            #region insert correspondence
                            ICorrespondence correspondence = _correspondenceRepo.CreateEmptyCorrespondence();
                            correspondence.GenericKey = _applicationLife.Account.Key;
                            correspondence.GenericKeyType = _lookupRepo.GenericKeyType.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.GenericKeyTypes.Account)];
                            correspondence.ReportStatement = _reportRepo.GetReportStatementByKey(reportData.ReportStatementKey);
                            correspondence.CorrespondenceMedium = _lookupRepo.CorrespondenceMediums.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.CorrespondenceMediums.Post)];
                            correspondence.DueDate = DateTime.Now;
                            correspondence.ChangeDate = DateTime.Now;
                            correspondence.UserID = _view.CurrentPrincipal.Identity.Name;
                            correspondence.LegalEntity = legalEntity;

                            foreach (ReportDataParameter parm in reportData.ReportParameters)
                            {
                                ICorrespondenceParameters correspondenceParm = _correspondenceRepo.CreateEmptyCorrespondenceParameter();
                                correspondenceParm.Correspondence = correspondence;
                                correspondenceParm.ReportParameter = _reportRepo.GetReportParameterByKey(parm.ReportParameterKey);
                                if (parm.ParameterValue!=null)
                                    correspondenceParm.ReportParameterValue = parm.ParameterValue.ToString();
                                // add the correspondenceparameter object to the correspondence object
                                correspondence.CorrespondenceParameters.Add(_view.Messages, correspondenceParm);
                            }

                            _correspondenceRepo.SaveCorrespondence(correspondence);
                            #endregion

                            // write the stage transition  record
                            _stageDefinitionRepo.SaveStageTransition(_applicationLife.Account.Key, Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeAdmin), SAHL.Common.Constants.StageDefinitionConstants.DocumentProcessed, reportData.ReportName, adUser);

                        }
                    }


                    txn.VoteCommit();

                    // navigate
                    svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);

                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    txn.Dispose();
                }
            }
        }
    }
}
