namespace DomainService2.SharedServices.Common
{
    public class SendClientSurveyInternalEmailCommand : StandardDomainServiceCommand
    {
        public SendClientSurveyInternalEmailCommand(int businessEventQuestionnaireKey, int applicationKey, string aDUserName)
        {
            this.BusinessEventQuestionnaireKey = businessEventQuestionnaireKey;
            this.ApplicationKey = applicationKey;
            this.ADUserName = aDUserName;
        }

        public int BusinessEventQuestionnaireKey
        {
            get;
            protected set;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public string ADUserName
        {
            get;
            protected set;
        }
    }
}