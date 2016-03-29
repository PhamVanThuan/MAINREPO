using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.CacheData;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using System.Text;
using SAHL.Common;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class RPAR : SAHLCommonBasePresenter<IRPAR>
    {
        private InstanceNode _node;
        private IApplicationLife _applicationLife;
        private const string _ActivityDoneString = "RPARDone";
        private bool _ActivityDoneValue;
        private long _instanceID = -1;
        private Dictionary<string, object> _x2Data;
        private IApplicationRepository _applicationRepo;
        private ILookupRepository _lookupRepo;
        private ILifeRepository _lifeRepo;
        private IStageDefinitionRepository _stageDefinitionRepo;
        private IADUser _consultantADUser;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RPAR(IRPAR view, SAHLCommonBaseController controller)
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

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo) == true)
                GlobalCacheData.Remove(ViewConstants.NavigateTo);

            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, new List<ICacheObjectLifeTime>());

            // Get the LifeOrigination Data to check if RPAR screen has been completed
            _x2Data = _node.X2Data as Dictionary<string, object>;
            _ActivityDoneValue = _x2Data == null || _x2Data[_ActivityDoneString] == null ? false : Convert.ToBoolean(_x2Data[_ActivityDoneString]);
            _instanceID = Convert.ToInt64(_x2Data["InstanceID"]);

            _view.OtherInsurerName = _x2Data["RPARInsurer"] != null ? _x2Data["RPARInsurer"].ToString() : "";
            _view.ReplacePolicyControlsVisible = _x2Data[_ActivityDoneString].ToString() == "1" ? true : false;

            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            _stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
            _consultantADUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);

            _view.OnConsideringButtonClicked += new EventHandler(OnConsideringButtonClicked);
            _view.OnNextButtonClicked += new EventHandler(OnNextButtonClicked);
            _view.OnNTUButtonClicked += new EventHandler(OnNTUButtonClicked);
            _view.OnRBNYesNoSelectedIndexChanged += new KeyChangedEventHandler(OnRBNYesNoSelectedIndexChanged);
            _view.OnDDLAssurerSelectedIndexChanged += new KeyChangedEventHandler(OnDDLAssurerSelectedIndexChanged);

            // Get the Life Offer Object
            _applicationLife = _applicationRepo.GetApplicationLifeByKey((int)_node.GenericKey);
            _view.RPARPolicyNumber = _applicationLife.RPARPolicyNumber != null ? _applicationLife.RPARPolicyNumber : "";
            _view.RPARInsurer = _applicationLife.RPARInsurer != null ? _applicationLife.RPARInsurer : "";
            if (_view.RPARInsurer.Length == 0)
                _view.RPARInsurerKey = Convert.ToInt32(SAHL.Common.Globals.Insurers.SAHLLife);
            else
                _view.RPARInsurerKey = _lifeRepo.GetInsurerByDescription(_view.RPARInsurer).Key;

            // bind insurers
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            IDictionary<string, string> insurers = lookups.Insurers.BindableDictionary;
            _view.BindInsurers(insurers);

            // get text statements
            ILifeRepository LR = RepositoryFactory.GetRepository<ILifeRepository>();
            int[] types = new int[] { (int)TextStatementTypes.RPAR };   
            IReadOnlyEventList<ITextStatement> lstStatements = LR.GetTextStatementsForTypes(types);

            _view.BindReplacePolicyConditions(lstStatements, _ActivityDoneValue);
        }

        void OnDDLAssurerSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (String.Compare(e.Key.ToString(), _lookupRepo.Insurers.ObjectDictionary[Convert.ToString(Convert.ToInt32(SAHL.Common.Globals.Insurers.Other))].Description, true) == 0)
            {
                _view.OtherInsurerVisible = true;
                _view.OtherInsurerName = "";
            }
            else
                _view.OtherInsurerVisible = false;
        }
        
        /// <summary>
        /// Handles the event fired by the view when the Radio Button List Selected Item is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRBNYesNoSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            _view.ReplacePolicyControlsVisible = Convert.ToBoolean(e.Key);
        }

        /// <summary>
        /// Handles the event fired by the view when the NTU Button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnNTUButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the next State
            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
            svc.StartActivity(_view.CurrentPrincipal, _instanceID, "NTU Policy", new Dictionary<string, string>(), false);

            _view.Navigator.Navigate("NTU");
        }


        /// <summary>
        /// Handles the event fired by the view when the Considering Button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnConsideringButtonClicked(object sender, EventArgs e)
        {
            // Insert Stage Transition
            _stageDefinitionRepo.SaveStageTransition(_applicationLife.Key, Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination), SAHL.Common.Constants.StageDefinitionConstants.RPAR_Considering, "",_consultantADUser);

            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
            svc.StartActivity(_view.CurrentPrincipal, _instanceID, "Create Callback", new Dictionary<string, string>(), false);
            _view.Navigator.Navigate("Callback");
        }

        void OnNextButtonClicked(object sender, EventArgs e)
        {
            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
            if (svc.IsViewDefaultFormForState(_view.CurrentPrincipal, _view.ViewName) == true)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    ValidateInput();

                    if (_view.IsValid)
                    {
                        string rparinsurer_x2 = "", rparinsurer_offer = "";
                        string comments = "no replacement";
                        bool replacementPolicy = false;

                        // Send email if other insurer has been entered. The receiipient of the email will then arrange for this insurer
                        // to be added to the database so that it appears in the list in future.
                        if (_view.ReplacePolicyControlsVisible == true)
                        {
                            replacementPolicy = true;
                            comments = "previous insurer: ";
                            string selectedInsurer = _lookupRepo.Insurers.ObjectDictionary[_view.RPARInsurerKey.ToString()].Description;

                            if (_view.OtherInsurerVisible)
                            {
                                rparinsurer_x2 = _view.OtherInsurerName;
                                comments += "Other (" + _view.OtherInsurerName + "), Policy Number: " + _view.RPARPolicyNumber;

                                try
                                {
                                    IMessageService messageService = ServiceFactory.GetService<IMessageService>();

                                    string from = _lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.RPARMailFrom].ControlText;
                                    string to = _lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.RPARMailTo].ControlText;
                                    string cc = _lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.RPARMailCC].ControlText;
                                    string subject = _lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.RPARMailSubject].ControlText;

                                    StringBuilder body = new StringBuilder();
                                    body.AppendLine("A new Life Insurer has been captured via the HALO Life RPAR screen.");
                                    body.AppendLine("------------------------------------------------------------------");
                                    body.AppendLine("");
                                    body.AppendLine("RPAR Insurer Name  : " + _view.OtherInsurerName);
                                    body.AppendLine("RPAR Policy Number : " + _view.RPARPolicyNumber);
                                    body.AppendLine("Policy Number      : " + _applicationLife.Account.Key.ToString());
                                    body.AppendLine("Application Number : " + _applicationLife.Key.ToString());
                                    body.AppendLine("Captured By        : " + _consultantADUser.ADUserName);
                                    body.AppendLine("");
                                    body.AppendLine("Please contact IT Support who need to do the following:");
                                    body.AppendLine("1. Add this insurer to the Insurer table.");
                                    body.AppendLine("2. Update the RPARInsurer column on the LifeOffer and LifePolicy record with the RPAR Insurer Name.");

                                    messageService.SendEmailInternal(from, to, cc, "", subject, body.ToString(),false);

                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                rparinsurer_x2 = selectedInsurer;
                                comments += selectedInsurer + ", Policy Number: " + _view.RPARPolicyNumber;
                            }

                            rparinsurer_offer = selectedInsurer;
                        }


                        // Update the RPAR data on the Application
                        //_application.ApplicationLife.Insurer = _lookupRepo.Insurers.ObjectDictionary[Convert.ToString(_view.RPARInsurerKey)];
                        _applicationLife.RPARPolicyNumber = _view.RPARPolicyNumber;
                        _applicationLife.RPARInsurer = rparinsurer_offer;
                        _applicationRepo.SaveApplication(_applicationLife);

                        // write the stage transition  record
                        IStageDefinitionRepository stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                        stageDefinitionRepo.SaveStageTransition(_applicationLife.Key, Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination), SAHL.Common.Constants.StageDefinitionConstants.RPAR_Completed, comments,_consultantADUser);

                        // Update the RPAR Insurer and RPAR Done on the X2DATA.LifeOrigination table
                        Dictionary<string, string> x2Data = new Dictionary<string, string>();
                        x2Data.Add(_ActivityDoneString, replacementPolicy == true ? "1" : "0");
                        x2Data.Add("RPARInsurer", rparinsurer_x2);

                        // Navigate to the next State
                        svc.WorkFlowWizardNext(_view.CurrentPrincipal, _view.ViewName, _view.Navigator, x2Data);
                    }

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
            else
            {
                // Navigate to the next State
                svc.WorkFlowWizardNext(_view.CurrentPrincipal, _view.ViewName, _view.Navigator);
            }

        }

        private void ValidateInput()
        {
            if (_view.ReplacePolicyControlsVisible == true)
            {
                // if we have selected "other" as insurer, then validate that the assurer name is entered
                string selectedInsurer = _lookupRepo.Insurers.ObjectDictionary[_view.RPARInsurerKey.ToString()].Description;
                if (String.Compare(selectedInsurer, "Other", true) == 0)
                {
                    if (String.IsNullOrEmpty(_view.OtherInsurerName))
                        _view.Messages.Add(new Error("Assurer name must be entered.", "Assurer name must be entered."));
                }
                if (String.IsNullOrEmpty(_view.RPARPolicyNumber))
                    _view.Messages.Add(new Error("Policy number must be entered.", "Policy number must be entered."));

                if (_view.AllOptionsChecked == false)
                    _view.Messages.Add(new Error("All points must be accepted before you can continue.", "All points must be accepted before you can continue."));
            }
        }
    }
}
