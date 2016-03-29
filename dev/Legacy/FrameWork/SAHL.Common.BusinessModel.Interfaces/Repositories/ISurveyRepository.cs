using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ISurveyRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IClientQuestionnaire GetClientQuestionnaireByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        IClientQuestionnaire GetClientQuestionnaireByGUID(string guid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="guid"></param>
        void RemoveClientQuestionnaireByGUID(string guid);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IClientQuestionnaire CreateEmptyClientQuestionnaire();

        /// <summary>
        ///
        /// </summary>
        /// <param name="cq"></param>
        void SaveClientQuestionnaire(IClientQuestionnaire cq);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IClientAnswerValue CreateEmptyClientAnswerValue();

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IQuestionnaire GetQuestionnaireByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IQuestionnaireQuestion GetQuestionnaireQuestionByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IClientAnswer CreateEmptyClientAnswer();

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IAnswer GetAnswerByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        IEventList<IClientQuestionnaire> GetClientQuestionnairesByLegalEntityKey(int legalEntityKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="clientSurveyStatus"></param>
        /// <returns></returns>
        IEventList<IClientQuestionnaire> GetClientQuestionnairesByLegalEntityKey(int legalEntityKey, Globals.ClientSurveyStatus clientSurveyStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="clientSurveyStatus"></param>
        /// <param name="latestFirst"></param>
        /// <returns></returns>
        IEventList<IClientQuestionnaire> GetClientQuestionnairesByLegalEntityKey(int legalEntityKey, Globals.ClientSurveyStatus clientSurveyStatus, bool latestFirst);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <returns></returns>
        IEventList<IClientQuestionnaire> GetClientQuestionnairesByGenericKey(int genericKey, int genericKeyTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="clientSurveyStatus"></param>
        /// <returns></returns>
        IEventList<IClientQuestionnaire> GetClientQuestionnairesByGenericKey(int genericKey, int genericKeyTypeKey, Globals.ClientSurveyStatus clientSurveyStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="clientSurveyStatus"></param>
        /// <param name="latestFirst"></param>
        /// <returns></returns>
        IEventList<IClientQuestionnaire> GetClientQuestionnairesByGenericKey(int genericKey, int genericKeyTypeKey, Globals.ClientSurveyStatus clientSurveyStatus, bool latestFirst);

        /// <summary>
        /// Get Business Event Questionnaire By BusinessEventQuestionnaireKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IBusinessEventQuestionnaire GetBusinessEventQuestionnaireByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="clientQuestionnaire"></param>
        /// <param name="questionnaireQuestion"></param>
        /// <returns></returns>
        IList<IClientAnswer> GetClientAnswersForQuestion(IClientQuestionnaire clientQuestionnaire, IQuestionnaireQuestion questionnaireQuestion);

        /// <summary>
        /// Returns a List of unanswered questionnaires
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <returns></returns>
        List<SurveyBindableObject> GetUnansweredandAdHocClientQuestionnairesByGenericKey(int genericKey, int genericKeyTypeKey);

        /// <summary>
        /// Sends a Client Survey to the legal entity's email address.
        /// </summary>
        /// <param name="businessEventQuestionnaireKey">Business Event Questionnaire Key</param>
        /// <param name="applicationKey">Application Key</param>
        /// <param name="adUserName"></param>
        /// <param name="externalEmail">Indicates whether this is to be sent to an Internal or External email address</param>
        void SendClientSurveyEmail(int businessEventQuestionnaireKey, int applicationKey, string adUserName, bool externalEmail);

        /// <summary>
        /// Sends a Client Survey to the legal entity's email address.
        /// </summary>
        /// <param name="qKey">Questionnaire Key</param>
        /// <param name="leKey">lekey</param>
        /// <param name="gKey">gKey</param>
        /// <param name="gkTypekey">gkType</param>
        /// <param name="ADUserName">ADUserName</param>
        /// <param name="be">be</param>
        void SendSurveyToClient(int qKey, int leKey, int gKey, int gkTypekey, string ADUserName, IBusinessEvent be);

        /// <summary>
        /// Sends a Client Survey to the legal entity's email address.
        /// </summary>
        /// <param name="qKey">Questionnaire Key</param>
        /// <param name="leKey">lekey</param>
        /// <param name="gKey">gKey</param>
        /// <param name="gkTypekey">gkType</param>
        /// <param name="ADUserName">ADUserName</param>

        void SendSurveyToClient(int qKey, int leKey, int gKey, int gkTypekey, string ADUserName);
    }

    /// <summary>
    /// Simple class for binding surveys to the grid.
    /// </summary>
    public class SurveyBindableObject
    {
        private int _questionnaireKey;
        private string _questionnaireDescription;
        private int _businessEventKey;
        private string _businessEventDescription;

        private int _clientQuestionnaireKey;
        private int _businessEventQuestionnaireKey;
        private string _datePresented;
        private int _adUserKey;
        private int _genericKey;
        private int _genericKeyTypeKey;
        private string _genericKeyDescription;
        private string _dateReceived;
        private int _legalEntityKey;
        private string _legalEntityName;
        private string _clientSurveyStatus;
        private string _guid;

        public SurveyBindableObject(int questionnaireKey, string questionnaireDescription, int businessEventKey, string businessEventDescription,
            int clientQuestionnaireKey, int businessEventQuestionnaireKey, string datePresented, int adUserKey,
            int genericKey, int genericKeyTypeKey, string genericKeyDescription, string dateReceived, int legalEntityKey, string legalEntityName, string clientSurveyStatus, string guid)
        {
            _questionnaireKey = questionnaireKey;
            _questionnaireDescription = questionnaireDescription;
            _businessEventKey = businessEventKey;
            _businessEventDescription = businessEventDescription;

            _clientQuestionnaireKey = clientQuestionnaireKey;
            _businessEventQuestionnaireKey = businessEventQuestionnaireKey;
            _datePresented = datePresented;
            _adUserKey = adUserKey;
            _genericKey = genericKey;
            _genericKeyTypeKey = genericKeyTypeKey;
            _genericKeyDescription = genericKeyDescription;
            _dateReceived = dateReceived;
            _legalEntityKey = legalEntityKey;
            _legalEntityName = legalEntityName;
            _clientSurveyStatus = clientSurveyStatus;
            _guid = guid;
        }

        public int QuestionnaireKey
        {
            get { return _questionnaireKey; }
            set { _questionnaireKey = value; }
        }

        public string QuestionnaireDescription
        {
            get { return _questionnaireDescription; }
            set { _questionnaireDescription = value; }
        }

        public int BusinessEventKey
        {
            get { return _businessEventKey; }
            set { _businessEventKey = value; }
        }

        public string BusinessEventDescription
        {
            get { return _businessEventDescription; }
            set { _businessEventDescription = value; }
        }

        public int ClientQuestionnaireKey
        {
            get { return _clientQuestionnaireKey; }
            set { _clientQuestionnaireKey = value; }
        }

        public int BusinessEventQuestionnaireKey
        {
            get { return _businessEventQuestionnaireKey; }
            set { _businessEventQuestionnaireKey = value; }
        }

        public string DatePresented
        {
            get { return _datePresented; }
            set { _datePresented = value; }
        }

        public int ADUserKey
        {
            get { return _adUserKey; }
            set { _adUserKey = value; }
        }

        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        public int GenericKeyTypeKey
        {
            get { return _genericKeyTypeKey; }
            set { _genericKeyTypeKey = value; }
        }

        public string GenericKeyDescription
        {
            get { return _genericKeyDescription; }
            set { _genericKeyDescription = value; }
        }

        public string DateReceived
        {
            get { return _dateReceived; }
            set { _dateReceived = value; }
        }

        public int LegalEntityKey
        {
            get { return _legalEntityKey; }
            set { _legalEntityKey = value; }
        }

        public string LegalEntity
        {
            get { return _legalEntityName; }
            set { _legalEntityName = value; }
        }

        public string ClientSurveyStatus
        {
            get { return _clientSurveyStatus; }
            set { _clientSurveyStatus = value; }
        }

        public string GUID
        {
            get { return _guid; }
            set { _guid = value; }
        }
    }

    /// <summary>
    /// A Class to hold the Survey Answers
    /// </summary>
    public class SurveyQuestionAnswer
    {
        private int _questionnaireQuestionKey;
        private int _answerTypeKey;
        private int _answerKey;
        private string _answerValue;

        public SurveyQuestionAnswer()
        {
        }

        public int QuestionnaireQuestionKey
        {
            get
            {
                return _questionnaireQuestionKey;
            }
            set
            {
                _questionnaireQuestionKey = value;
            }
        }

        public int AnswerTypeKey
        {
            get
            {
                return _answerTypeKey;
            }
            set
            {
                _answerTypeKey = value;
            }
        }

        public int AnswerKey
        {
            get
            {
                return _answerKey;
            }
            set
            {
                _answerKey = value;
            }
        }

        public string AnswerValue
        {
            get
            {
                return _answerValue;
            }
            set
            {
                _answerValue = value;
            }
        }
    }
}