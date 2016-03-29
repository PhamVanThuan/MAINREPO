using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.X2.Framework;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceStrategies;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CancelPolicy : SAHLCommonBasePresenter<ICancelPolicy>
    {
        private CBOMenuNode _node;
        private IAccountLifePolicy _lifePolicyAccount;
        private ILifePolicy _lifePolicy;
        private int _accountKey, _daysSinceAcceptedDate;
        private IReadOnlyEventList<IReasonDefinition> _reasonDefinitions;
        private ILifeRepository _lifeRepo;
        private IAccountRepository _accountRepo;
        private IReasonRepository _reasonRepo;
        private ILookupRepository _lookupRepo;
        private ICorrespondenceRepository _correspondenceRepo;
        private IReportRepository _reportRepo;
        private IStageDefinitionRepository _stageDefinitionRepo;
        private bool _lifeManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CancelPolicy(ICancelPolicy view, SAHLCommonBaseController controller)
            : base(view, controller)
        {     
            // Get the CBO Node 
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

           _accountKey = Convert.ToInt32(_node.GenericKey);
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;


            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);

            _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _correspondenceRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
            _reportRepo = RepositoryFactory.GetRepository<IReportRepository>();
            _stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            IDictionary<string, int> cancellationTypes = _lifeRepo.GetCancellationTypes();

            _lifePolicyAccount = _accountRepo.GetAccountByKey(_accountKey) as IAccountLifePolicy;
            _lifePolicy = _lifePolicyAccount.LifePolicy;

            // Check if the user is a Life Manager user
            _lifeManager = _view.CurrentPrincipal.IsInRole("LifeManager");

            // Check if the user is an Admin user
            _view.AdminUser = _lifeRepo.IsAdminUser(_view.CurrentPrincipal);


            // Check if this user is allowed to cancel a policy
            _view.CancellationDisabledMessage = "";
            if (_view.AdminUser == false)
                _view.CancellationDisabledMessage = "Policy can only be Cancelled by a Life Admin user.";

            // Check if this policy can be cancelled
            if (_lifePolicy == null)
                _view.CancellationDisabledMessage = "Policy cannot be Cancelled - Policy is currently an 'Application'.";
            else if (_lifePolicy.LifePolicyStatus.Key != Convert.ToInt32(SAHL.Common.Globals.LifePolicyStatuses.Inforce)
             && _lifePolicy.LifePolicyStatus.Key != Convert.ToInt32(SAHL.Common.Globals.LifePolicyStatuses.Accepted)
             && _lifePolicy.LifePolicyStatus.Key != Convert.ToInt32(SAHL.Common.Globals.LifePolicyStatuses.Acceptedtocommenceon1st))
                _view.CancellationDisabledMessage = "Policy cannot be Cancelled - Status is currently '" + _lifePolicy.LifePolicyStatus.Description + "'.";

            // add reason definitions to dictionary to use in binding
            _reasonDefinitions = _reasonRepo.GetReasonDefinitionsByReasonType(ReasonTypes.LifePolicyCancellation);
            IDictionary<int, string> reasons = new Dictionary<int, string>();
            foreach (IReasonDefinition def in _reasonDefinitions)
            {
                reasons.Add(def.Key, def.ReasonDescription.Description);
            }

            // get the number of days since the policy has accepted 
            _daysSinceAcceptedDate = 0;
            bool policyHasCommenced = false;
            if (_lifePolicy != null)
            {
                TimeSpan diff = DateTime.Today - (_lifePolicy.DateOfAcceptance.HasValue ? _lifePolicy.DateOfAcceptance.Value : DateTime.Now);
                _daysSinceAcceptedDate = diff.Days;

                policyHasCommenced = _lifePolicy.DateOfCommencement != null ? true : false;
            }


            // Set the relevant properties for displaying controls within the view
            _view.CancelFromInceptionEnabled = true;
            _view.CancelWithAuthorizationEnabled = true;
            _view.CancelWithNoRefundEnabled = true;
            _view.CancelWithProRateEnabled = true;

            if (_daysSinceAcceptedDate > 30)
                _view.CancelFromInceptionEnabled = false;

            if (_lifeManager)
                _view.CancelWithAuthorizationEnabled = true;
            else
                _view.CancelWithAuthorizationEnabled = false;

            //If policy status is accepted or accepted to commence on 1st then force the "Cancel from inception option"
            if (_lifePolicy != null && (_lifePolicy.LifePolicyStatus.Key == (int)SAHL.Common.Globals.LifePolicyStatuses.Accepted || _lifePolicy.LifePolicyStatus.Key == (int)SAHL.Common.Globals.LifePolicyStatuses.Acceptedtocommenceon1st))
            {
                _view.CancelFromInceptionEnabled = true;
                _view.CancelWithAuthorizationEnabled = false;
                _view.CancelWithNoRefundEnabled = false;
                _view.CancelWithProRateEnabled = false;

            }

            if (!String.IsNullOrEmpty(_view.CancellationDisabledMessage))
            {
                _view.CancelFromInceptionEnabled = false;
                _view.CancelWithAuthorizationEnabled = false;
                _view.CancelWithNoRefundEnabled = false;
                _view.CancelWithProRateEnabled = false;
            }

            // Check if Life is condition of loan
            bool lifeIsConditionOfLoan = _lifeRepo.IsLifeConditionOfLoan(_lifePolicyAccount.ParentMortgageLoan.Key);

            //todo pass through loan conditions
            _view.BindControls(cancellationTypes, reasons, _daysSinceAcceptedDate, policyHasCommenced, lifeIsConditionOfLoan);
        }

        /// <summary>
        /// Hooks the OnCancelButtonClicked event fired by the view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // validate selection
            if (_view.CancellationTypeKey==-1)
                _view.Messages.Add(new Error("A Cancellation Type must be selected.", "A Cancellation Type must be selected."));
            if (_view.SelectedReason == "- Please select -")
                _view.Messages.Add(new Error("A Cancellation Reason must be selected.", "A Cancellation Reason must be selected."));

            if (_view.IsValid)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    
                    _lifeRepo.CancelLifePolicy(_lifePolicyAccount.Key, _view.CancellationTypeKey);

                    // write the stage transition  record
                    IStageDefinitionRepository stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                    ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                    IADUser adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);

                    string stageTransitionComments = "Reason: " + _view.SelectedReason + " - Cancellation Letter Received:";
                    if (_view.CancellationLetterReceived)
                        stageTransitionComments += " Yes";
                    else
                        stageTransitionComments += " No";

                    stageDefinitionRepo.SaveStageTransition(_lifePolicyAccount.Key, Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeAdmin), SAHL.Common.Constants.StageDefinitionConstants.PolicyCancelled, stageTransitionComments, adUser);

                    #region insert the Cancellations Letter into the correspondence table

                    // Get the ReportData using the CorrespondenceStrategyWorker
                    List<ReportData> reportDataList = CorrespondenceStrategyWorker.GetReportData("Life_Correspondence_CancellationLetter", _lifePolicyAccount.OriginationSourceProduct.Key);
                    if (reportDataList.Count > 0)
                    {
                        ReportData reportData = reportDataList[0];

                        #region set the parameter values
                        // get the policyholder details
                        int legalEntityKey = -1, addressKey = -1;
                        _lifeRepo.GetPolicyHolderDetails(_lifePolicyAccount.Key, out legalEntityKey, out addressKey);
                        ILegalEntity legalEntity = RepositoryFactory.GetRepository<ILegalEntityRepository>().GetLegalEntityByKey(legalEntityKey);
                        // set the parameter values
                        foreach (ReportDataParameter parm in reportData.ReportParameters)
                        {
                            if (parm.ParameterName.ToLower() == reportData.GenericKeyParameterName.ToLower())
                                parm.ParameterValue = _lifePolicyAccount.Key.ToString();
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
                        correspondence.GenericKey = _lifePolicyAccount.Key;
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
                            if (parm.ParameterValue != null)
                                correspondenceParm.ReportParameterValue = parm.ParameterValue.ToString();
                            // add the correspondenceparameter object to the correspondence object
                            correspondence.CorrespondenceParameters.Add(_view.Messages, correspondenceParm);
                        }

                        _correspondenceRepo.SaveCorrespondence(correspondence);
                        #endregion

                        // write the stage transition  record
                        _stageDefinitionRepo.SaveStageTransition(_lifePolicyAccount.Key, Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeAdmin), SAHL.Common.Constants.StageDefinitionConstants.DocumentProcessed, reportData.ReportName, adUser);

                    }
                    #endregion

                    txn.VoteCommit();

                    // Navigate to the correspondence screen
                    _view.Navigator.Navigate("Submit");
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
