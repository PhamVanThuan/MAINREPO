using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Survey.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Security;

namespace SAHL.Web.Views.Survey.Presenters
{
    public class Capture : SAHLCommonBasePresenter<ICapture>
    {

        protected CBOMenuNode _node;
        protected int _genericKey;
        protected int _genericKeyTypeKey;
        protected ISurveyRepository _surveyRepo;
        protected IOrganisationStructureRepository _orgStructureRepo;
        protected ILookupRepository _lookupRepo;
        protected ILegalEntityRepository _legalEntityRepo;
        protected IAccount _account;
        protected IApplication _application;
        protected IAccountRepository _accRepo;
        protected IApplicationRepository _appRepo;
        protected IQuestionnaire _selectedQuestionnaire;
        protected List<SurveyBindableObject> surveys;
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Capture(ICapture view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            // Get the CBO Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _genericKey = _node.GenericKey;
            _genericKeyTypeKey = _node.GenericKeyTypeKey;

            if (_genericKeyTypeKey == (int)GenericKeyTypes.ParentAccount)
                _genericKeyTypeKey = (int)GenericKeyTypes.Account;

            _view.DoSurveyButtonClicked += new EventHandler(_view_DoSurveyButtonClicked);
            _view.SendSurveyButtonClicked += new EventHandler(_view_SendSurveyButtonClicked);
            _view.SaveSurveyButtonClicked += new KeyChangedEventHandler(_view_SaveSurveyButtonClicked);
            _view.CancelSurveyButtonClicked += new EventHandler(_view_CancelSurveyButtonClicked);

            _surveyRepo = RepositoryFactory.GetRepository<ISurveyRepository>();
            _orgStructureRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

            // get the legalentity roles
            IReadOnlyEventList<ILegalEntity> legalEntities = null;

            switch (_genericKeyTypeKey)
            {
                case (int)GenericKeyTypes.Account:
                    _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    _account = _accRepo.GetAccountByKey(_genericKey);

                    var accRoleTypes = new int[] { (int)RoleTypes.MainApplicant, (int)RoleTypes.Suretor };
                    legalEntities = _account.GetLegalEntitiesByRoleType(_view.Messages, accRoleTypes, GeneralStatusKey.Active);
                    break;
                case (int)GenericKeyTypes.Offer:
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    _application = _appRepo.GetApplicationByKey(_genericKey);

                    OfferRoleTypes[] appRoleTypes = new OfferRoleTypes[] { OfferRoleTypes.MainApplicant, OfferRoleTypes.Suretor };
                    legalEntities = _application.GetLegalEntitiesByRoleType(appRoleTypes, GeneralStatusKey.Active);
                    break;
                default:
                    break;
            }
           
            // bind the dropdown list
            _view.BindLegalEntityDropdownList(legalEntities);

            //bind the grid
            BindSurveyGrid();

            _view.DoSurveyButtonVisible = true;
            _view.SaveSurveyButtonVisible = false;
            _view.CancelSurveyButtonVisible = false;
            _view.LegalEntityEnabled = true;
        }

        private void BindSurveyGrid()
        {
            surveys = _surveyRepo.GetUnansweredandAdHocClientQuestionnairesByGenericKey(_genericKey, _genericKeyTypeKey);
            _view.BindSurveyGrid(surveys);
        }

        void _view_CancelSurveyButtonClicked(object sender, EventArgs e)
        {
            _view.SaveSurveyButtonVisible = false;
            _view.CancelSurveyButtonVisible = false;

            // if we have just created a clientquestionnaire record then remove it
            if (PrivateCacheData.ContainsKey("NEWCLIENTQUESTIONNAIREGUID"))
            {
                string guid = Convert.ToString(PrivateCacheData["NEWCLIENTQUESTIONNAIREGUID"]);
                PrivateCacheData.Remove("NEWCLIENTQUESTIONNAIREGUID");
                RemoveClientQuestionnaire(guid);
            }

            // clear cache items
            PrivateCacheData.Remove("SELECTEDCLIENTQUESTIONNAIRE");
            PrivateCacheData.Remove("SELECTEDLEGALENTITYKEY");

            // repopulate the list
            BindSurveyGrid();

            //// this will remove the control from the page
            //_view.BindSurveyDetail(null, false, false);
        }

        void _view_SendSurveyButtonClicked(object sender, EventArgs e)
        {
            // perform screen validation
            if (ValidateSurveySelection(_view.SelectedClientQuestionnaireGUID, _view.SelectedLegalEntityKey) == false)
                return;

            ILegalEntity le = _legalEntityRepo.GetLegalEntityByKey(_view.SelectedLegalEntityKey);
            if (String.IsNullOrEmpty(le.EmailAddress))
            {
                string errorMessage = "The legal entity requires a valid email address.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                return;
            }

			SAHLPrincipal principal = SAHLPrincipal.GetCurrent();
			_surveyRepo.SendSurveyToClient(_view.SelectedQuestionnaireKey, _view.SelectedLegalEntityKey, _genericKey, _genericKeyTypeKey, principal.Identity.Name.ToString());

            // repopulate the list
            BindSurveyGrid();
		}

        void _view_DoSurveyButtonClicked(object sender, EventArgs e)
        {
            // perform screen validation
            if (ValidateSurveySelection(_view.SelectedClientQuestionnaireGUID, _view.SelectedLegalEntityKey) == false)
                return;

            string clientQuestionnaireGUID = _view.SelectedClientQuestionnaireGUID;

            if (_view.SelectedClientQuestionnaireKey <= 0)
            {           
                // insert Client Questionnaire record here if requires
                IQuestionnaire selectedQuestionnaire = _surveyRepo.GetQuestionnaireByKey(_view.SelectedQuestionnaireKey);
                clientQuestionnaireGUID = CreateClientQuestionnaire(selectedQuestionnaire);

                PrivateCacheData.Remove("NEWCLIENTQUESTIONNAIREGUID");
                PrivateCacheData.Add("NEWCLIENTQUESTIONNAIREGUID", clientQuestionnaireGUID);

                // repopulate the list
                BindSurveyGrid();
            }

            // get the Client Questionnaire record
            IClientQuestionnaire _selectedClientQuestionnaire = _surveyRepo.GetClientQuestionnaireByGUID(clientQuestionnaireGUID);

            PrivateCacheData.Remove("SELECTEDCLIENTQUESTIONNAIREKEY");
            PrivateCacheData.Add("SELECTEDCLIENTQUESTIONNAIREKEY", _selectedClientQuestionnaire.Key);

            PrivateCacheData.Remove("SELECTEDLEGALENTITYKEY");
            PrivateCacheData.Add("SELECTEDLEGALENTITYKEY", _view.SelectedLegalEntityKey);

            _view.DoSurveyButtonVisible = false;
            _view.SaveSurveyButtonVisible = true;
            _view.CancelSurveyButtonVisible = true;
            _view.LegalEntityEnabled = false;

            _view.BindSurveyDetail(_selectedClientQuestionnaire, true, false);
        }

        void _view_SaveSurveyButtonClicked(object sender, KeyChangedEventArgs e)
        {
            if (e == null)
                return;

            IList<SurveyQuestionAnswer> surveyQuestionAnswers = e.Key as List<SurveyQuestionAnswer>;

            IClientQuestionnaire clientQuestionnaire = null;
            ILegalEntity legalEntity = null;

            // get the selected clientquestionnaire object from the cache
            if (PrivateCacheData.ContainsKey("SELECTEDCLIENTQUESTIONNAIREKEY") && PrivateCacheData.ContainsKey("SELECTEDLEGALENTITYKEY"))
            {
                int clientQuestionnaireKey = Convert.ToInt32(PrivateCacheData["SELECTEDCLIENTQUESTIONNAIREKEY"]);
                PrivateCacheData.Remove("SELECTEDCLIENTQUESTIONNAIREKEY");

                int legalentityKey = Convert.ToInt32(PrivateCacheData["SELECTEDLEGALENTITYKEY"]);
                PrivateCacheData.Remove("SELECTEDLEGALENTITYKEY");

                // get the Client Questionnaire record
                clientQuestionnaire = _surveyRepo.GetClientQuestionnaireByKey(clientQuestionnaireKey);

                // get the LegalEntity record
                legalEntity = _legalEntityRepo.GetLegalEntityByKey(legalentityKey);
            }

            if (clientQuestionnaire == null)
                return;

            TransactionScope txn = new TransactionScope();
            try
            {
                if (_view.IsValid)
                {
                    // update theClientQuestionnaire Table
                    clientQuestionnaire.ADUser = _orgStructureRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                    clientQuestionnaire.DateReceived = DateTime.Now;
                    clientQuestionnaire.LegalEntity = legalEntity;

                    foreach (SurveyQuestionAnswer sqa in surveyQuestionAnswers)
                    {
                        // add the ClientAnswer record
                        bool clientAnswerValueRequired = false;
                        switch (sqa.AnswerTypeKey)
                        {
                            // these types have to have a ClientAnswerValue
                            case (int)SAHL.Common.Globals.AnswerTypes.Comment:
                            case (int)SAHL.Common.Globals.AnswerTypes.Numeric:
                            case (int)SAHL.Common.Globals.AnswerTypes.Alphanumeric:
                            case (int)SAHL.Common.Globals.AnswerTypes.Date:
                            case (int)SAHL.Common.Globals.AnswerTypes.Ranking:
                                clientAnswerValueRequired = true;
                                break;
                            default:
                                break;
                        }

                        if (sqa.AnswerKey > 0)
                        {
                            if (clientAnswerValueRequired == false || (clientAnswerValueRequired == true && String.IsNullOrEmpty(sqa.AnswerValue) == false))
                            {
                                IClientAnswer clientAnswer = _surveyRepo.CreateEmptyClientAnswer();
                                clientAnswer.ClientSurvey = clientQuestionnaire;
                                clientAnswer.QuestionnaireQuestion = _surveyRepo.GetQuestionnaireQuestionByKey(sqa.QuestionnaireQuestionKey);
                                clientAnswer.Answer = _surveyRepo.GetAnswerByKey(sqa.AnswerKey);

                                // add the ClientAnswerVaue records (if any)
                                if (String.IsNullOrEmpty(sqa.AnswerValue) == false)
                                {
                                    IClientAnswerValue clientAnswerValue = _surveyRepo.CreateEmptyClientAnswerValue();
                                    clientAnswerValue.Value = sqa.AnswerValue;
                                    clientAnswerValue.ClientAnswer = clientAnswer;

                                    clientAnswer.ClientAnswerValue = clientAnswerValue;
                                }

                                clientQuestionnaire.ClientAnswers.Add(_view.Messages, clientAnswer);
                            }
                        }
                    }

                    // save the ClientQuestionnaire record
                    _surveyRepo.SaveClientQuestionnaire(clientQuestionnaire);

                    txn.VoteCommit();

                    // repopulate the list
                    BindSurveyGrid();
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

        private string CreateClientQuestionnaire(IQuestionnaire selectedQuestionnaire)
        {
            IClientQuestionnaire clientQuestionnaire = _surveyRepo.CreateEmptyClientQuestionnaire();

            string guid = System.Guid.NewGuid().ToString();

            TransactionScope txn = new TransactionScope();
            try
            {
                if (_view.IsValid)
                {
                    // get the BusinessEventQuestionnaire with a null (adhoc) businessevent 
                    IBusinessEventQuestionnaire businessEventQuestionnaire = null;
                    foreach (IBusinessEventQuestionnaire beq in selectedQuestionnaire.BusinessEventQuestionnaires)
                    {
                        if (beq.BusinessEvent == null)
                        {
                            businessEventQuestionnaire = beq;
                            break;
                        }
                    }

                    clientQuestionnaire.BusinessEventQuestionnaire = businessEventQuestionnaire;
                    clientQuestionnaire.DatePresented = DateTime.Now;
                    clientQuestionnaire.ADUser = _orgStructureRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                    clientQuestionnaire.GenericKey = _genericKey;
                    clientQuestionnaire.GenericKeyType = _lookupRepo.GenericKeyType.ObjectDictionary[_genericKeyTypeKey.ToString()];
                    clientQuestionnaire.DateReceived = null;
                    clientQuestionnaire.LegalEntity = _legalEntityRepo.GetLegalEntityByKey(_view.SelectedLegalEntityKey);
                    clientQuestionnaire.GUID = guid;

                    _surveyRepo.SaveClientQuestionnaire(clientQuestionnaire);

                    txn.VoteCommit();
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

            return guid;
        }

        private void RemoveClientQuestionnaire(string guid)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                if (_view.IsValid)
                {
                    _surveyRepo.RemoveClientQuestionnaireByGUID(guid);

                    txn.VoteCommit();
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

        private bool ValidateSurveySelection(string clientQuestionnaireGUID, int legalEntityKey)
        {
            bool valid = true;
            string errorMessage = "";

            if (String.IsNullOrEmpty(clientQuestionnaireGUID) && legalEntityKey <= 0)
            {
                errorMessage = "Must select a Questionnaire and Legal Entity before continuing.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }
            else if (String.IsNullOrEmpty(clientQuestionnaireGUID))
            {
                errorMessage = "Must select a Questionnaire before continuing.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }
            else if (legalEntityKey <= 0)
            {
                errorMessage = "Must select a Legal Entity before continuing.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }
            return valid;
        }

    }
}