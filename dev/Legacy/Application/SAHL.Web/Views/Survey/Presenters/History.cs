using System;
using System.Collections.Generic;
using System.Web;
using SAHL.Web.Views.Survey.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Survey.Presenters
{
    public class History : SAHLCommonBasePresenter<IHistory>
    {
        private CBOMenuNode _node;
        private int _genericKeyValue;
        private int _genericKeyTypeKey;
        private ISurveyRepository _surveyRepo;
        private IEventList<IClientQuestionnaire> _clientQuestionnaires;
        private IClientQuestionnaire selectedClientQuestionnaire;
        private int _questionnaireCount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>

        public History(IHistory view, SAHLCommonBaseController controller)
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

            _surveyRepo = RepositoryFactory.GetRepository<ISurveyRepository>();

            _genericKeyValue = _node.GenericKey;
            _genericKeyTypeKey = _node.GenericKeyTypeKey;

            _view.ClientQuestionnaireSelected += new KeyChangedEventHandler(_view_ClientQuestionnaireSelected);

            if (_genericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.LegalEntity)
                _clientQuestionnaires = _surveyRepo.GetClientQuestionnairesByLegalEntityKey(_genericKeyValue, ClientSurveyStatus.All, true);
            else
            {
                if (_genericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount)
                    _genericKeyTypeKey = (int)SAHL.Common.Globals.GenericKeyTypes.Account;

                _clientQuestionnaires = _surveyRepo.GetClientQuestionnairesByGenericKey(_genericKeyValue, _genericKeyTypeKey, ClientSurveyStatus.All, true);
            }

            _questionnaireCount = _clientQuestionnaires.Count;

            _view.BindSurveyGrid(_clientQuestionnaires);


            if (PrivateCacheData["CLIENTQUESTIONNAIRE"] != null)
            {
                selectedClientQuestionnaire = PrivateCacheData["CLIENTQUESTIONNAIRE"] as IClientQuestionnaire;
                BindSurveyDetail(selectedClientQuestionnaire);
            }     
        }

        void BindSurveyDetail(IClientQuestionnaire selectedClientQuestionnaire)
        {
            int height = 390;
            int adjustment = 20;

            if (_questionnaireCount >= 6)
            {
                _questionnaireCount = 6;
                adjustment = 25;
            }

            height = height - (adjustment * _questionnaireCount);

            _view.BindSurveyDetail(selectedClientQuestionnaire, true, false, height);
        }

        void _view_ClientQuestionnaireSelected(object sender, KeyChangedEventArgs e)
        {
            // go and get questionnaire details and build up questionnaire to display on screen
            if (e == null)
                return;

            selectedClientQuestionnaire = _surveyRepo.GetClientQuestionnaireByGUID(e.Key.ToString());

            PrivateCacheData.Remove("CLIENTQUESTIONNAIRE");
            PrivateCacheData.Add("CLIENTQUESTIONNAIRE", selectedClientQuestionnaire);

            BindSurveyDetail(selectedClientQuestionnaire);
        }
    }
}