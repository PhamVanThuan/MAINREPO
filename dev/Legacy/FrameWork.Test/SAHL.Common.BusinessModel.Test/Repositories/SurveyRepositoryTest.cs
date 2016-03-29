using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class SurveyRepositoryTest : TestBase
    {
        static private ISurveyRepository _surveyRepo = RepositoryFactory.GetRepository<ISurveyRepository>();
        static private IOrganisationStructureRepository _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
        static private ILegalEntityRepository _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
        static private ILookupRepository _lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

        [Test]
        public void GetClientQuestionnairesByKey()
        {
            using (new SessionScope())
            {
                // get first record in ClientQuestionnaire table
                string query = "select top 1 cq.ClientQuestionnaireKey from [2am].[survey].ClientQuestionnaire cq (nolock)";
                DataTable DT = base.GetQueryResults(query);
                if (DT == null || DT.Rows.Count <= 0)
                    Assert.Ignore("Unable to find any ClientQuestionnaire records.");
                else
                {
                    //test using repo method
                    int clientQuestionnaireKey = Convert.ToInt32(DT.Rows[0][0].ToString());
                    IClientQuestionnaire clientQuestionnaire = _surveyRepo.GetClientQuestionnaireByKey(clientQuestionnaireKey);

                    Assert.IsTrue(clientQuestionnaireKey == clientQuestionnaire.Key);
                }
            }
        }

        [Test]
        public void GetClientQuestionnairesByGUID()
        {
            using (new SessionScope())
            {
                // get first record in ClientQuestionnaire table
                string query = "select top 1 cq.ClientQuestionnaireKey, cq.GUID from [2am].[survey].ClientQuestionnaire cq (nolock)";
                DataTable DT = base.GetQueryResults(query);
                if (DT == null || DT.Rows.Count <= 0)
                    Assert.Ignore("Unable to find any ClientQuestionnaire records.");
                else
                {
                    //test using repo method
                    int clientQuestionnaireKey = Convert.ToInt32(DT.Rows[0][0].ToString());
                    string guid = DT.Rows[0][1].ToString();
                    IClientQuestionnaire clientQuestionnaire = _surveyRepo.GetClientQuestionnaireByGUID(guid);

                    Assert.IsTrue(clientQuestionnaireKey == clientQuestionnaire.Key);
                }
            }
        }

        [Test]
        public void CaptureTest()
        {
            //setup some stuff to use in the test
            IBusinessEventQuestionnaire beq = null;
            DateTime dtPresented = DateTime.Now.AddDays(-7);
            IADUser adu = null;
            int accKey = 0;
            ILegalEntity le = null;
            IClientQuestionnaire cQ = _surveyRepo.CreateEmptyClientQuestionnaire(); ;

            using (new SessionScope(FlushAction.Auto))
            {
                string sql = @"select top 1 beq.BusinessEventQuestionnaireKey, ad.ADUserKey, a.AccountKey, r.LegalEntityKey
                                from Account a
                                join [Role] r on a.AccountKey = r.AccountKey and r.GeneralStatusKey = 1
                                join survey.BusinessEventQuestionnaire beq on 1=1
                                join survey.Questionnaire q on beq.QuestionnaireKey = q.QuestionnaireKey and q.GeneralStatusKey = 1
                                join ADUser ad on 1=1 and ad.LegalEntityKey is not null
                                where a.AccountStatusKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    beq = _surveyRepo.GetBusinessEventQuestionnaireByKey(Convert.ToInt32(dr[0]));
                    adu = _osRepo.GetADUserByKey(Convert.ToInt32(dr[1]));
                    accKey = Convert.ToInt32(dr[2]);
                    le = _leRepo.GetLegalEntityByKey(Convert.ToInt32(dr[3]));

                    //if we dont have test data, fail the test to show the code is untested
                    if (beq == null || adu == null || accKey == 0 || le == null)
                    {
                        Assert.Fail("No data found to use for a test");
                        return;
                    }

                    //all we need, create the IClientQuestionnaire
                    cQ.BusinessEventQuestionnaire = beq;
                    cQ.DatePresented = dtPresented;
                    cQ.GenericKey = accKey;
                    cQ.GenericKeyType = _lkRepo.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.Account).ToString()];

                    _surveyRepo.SaveClientQuestionnaire(cQ);
                }
            }

            using (new SessionScope())
            {
                //get an existing unanswered IClientQuestionnaire, created above
                //this is in a separate session, so needs to be re-loaded here to avoid lazy loading exceptions
                IClientQuestionnaire cQr = _surveyRepo.GetClientQuestionnaireByKey(cQ.Key);
                IQuestionnaire q = cQr.BusinessEventQuestionnaire.Questionnaire;

                //check the type of Answer
                foreach (IQuestionnaireQuestion qq in q.Questions)
                {
                    if (qq.QuestionAnswers.Count > 0)
                    {
                        switch (qq.QuestionAnswers[0].Answer.AnswerType.Key)
                        {
                            case (int)Globals.AnswerTypes.Comment: // 1
                                AddAnswer(cQ, qq, qq.QuestionAnswers[0].Answer.Key, "comment long string");
                                break;

                            case (int)Globals.AnswerTypes.Numeric: // 2
                                AddAnswer(cQ, qq, qq.QuestionAnswers[0].Answer.Key, qq.QuestionAnswers[0].Answer.Key.ToString());
                                break;

                            case (int)Globals.AnswerTypes.Alphanumeric: // 3
                                AddAnswer(cQ, qq, qq.QuestionAnswers[0].Answer.Key, qq.QuestionAnswers[0].Answer.Key.ToString() + " and some string");
                                break;

                            case (int)Globals.AnswerTypes.Boolean: // 4
                                AddAnswer(cQ, qq, qq.QuestionAnswers[0].Answer.Key, null);
                                break;

                            case (int)Globals.AnswerTypes.Date: // 5
                                AddAnswer(cQ, qq, qq.QuestionAnswers[0].Answer.Key, DateTime.Now.ToShortDateString());
                                break;

                            case (int)Globals.AnswerTypes.Ranking: // 6

                                //need to add one for each option...
                                foreach (IQuestionnaireQuestionAnswer qa in qq.QuestionAnswers)
                                {
                                    AddAnswer(cQ, qq, qa.Answer.Key, qa.Sequence.ToString());
                                }
                                break;

                            case (int)Globals.AnswerTypes.Rating: // 7
                                AddAnswer(cQ, qq, qq.QuestionAnswers[0].Answer.Key, "Rating");
                                break;

                            case (int)Globals.AnswerTypes.SingleSelect: // 8
                                AddAnswer(cQ, qq, qq.QuestionAnswers[0].Answer.Key, "Single Select");
                                break;

                            case (int)Globals.AnswerTypes.MultiSelect: // 9
                                AddAnswer(cQ, qq, qq.QuestionAnswers[0].Answer.Key, "Multi Select");
                                break;

                            case (int)Globals.AnswerTypes.None: // 10

                                //do nothing
                                break;

                            default:
                                throw new NotFoundException("type not found");
                        }
                    }
                }

                // add the detail required when answering the Questionnaire
                cQ.ADUser = adu;
                cQ.DateReceived = DateTime.Now;
                cQ.LegalEntity = le;

                Assert.Greater(cQ.ClientAnswers.Count, 0);
            }
        }

        private static void AddAnswer(IClientQuestionnaire cs, IQuestionnaireQuestion question, int AnswerKey, string value)
        {
            IClientAnswer ca = _surveyRepo.CreateEmptyClientAnswer();
            IAnswer answer = _surveyRepo.GetAnswerByKey(AnswerKey);

            ca.Answer = answer;
            ca.QuestionnaireQuestion = question;

            if (!String.IsNullOrEmpty(value))
            {
                IClientAnswerValue cav = _surveyRepo.CreateEmptyClientAnswerValue();
                cav.Value = value;

                //bi directional keys, just in case
                cav.ClientAnswer = ca;
                ca.ClientAnswerValue = cav;
            }

            //bi-directional keys
            ca.ClientSurvey = cs;
            cs.ClientAnswers.Add(null, ca);
        }

        [Test]
        public void GetClientQuestionnairesByLegalEntityKey()
        {
            using (new SessionScope())
            {
                // get first record in ClientQuestionnaire table with a legalentitykey
                string query = "select top 1 cq.LegalEntityKey from [2am].[survey].ClientQuestionnaire cq (nolock) where LegalEntityKey is not null";
                DataTable DT = base.GetQueryResults(query);
                if (DT == null || DT.Rows.Count <= 0)
                    Assert.Ignore("Unable to find any ClientQuestionnaire records with a LegalEntityKey.");
                else
                {
                    //test using repo method
                    int legalEntityKey = Convert.ToInt32(DT.Rows[0][0].ToString());
                    IEventList<IClientQuestionnaire> clientQuestionnaires = _surveyRepo.GetClientQuestionnairesByLegalEntityKey(legalEntityKey);

                    Assert.IsTrue(clientQuestionnaires.Count > 0);
                }
            }
        }

        [Test]
        public void GetClientQuestionnairesByGenericKey()
        {
            using (new SessionScope())
            {
                // get first record in ClientQuestionnaire table
                string query = "select top 1 cq.GenericKey, cq.GenericKeyTypeKey from [2am].[survey].ClientQuestionnaire cq (nolock)";
                DataTable DT = base.GetQueryResults(query);
                if (DT == null || DT.Rows.Count <= 0)
                    Assert.Ignore("Unable to find any ClientQuestionnaire records.");
                else
                {
                    //test using repo method
                    int genericKey = Convert.ToInt32(DT.Rows[0][0].ToString());
                    int genericKeyTypeKey = Convert.ToInt32(DT.Rows[0][1].ToString());
                    IEventList<IClientQuestionnaire> clientQuestionnaires = _surveyRepo.GetClientQuestionnairesByGenericKey(genericKey, genericKeyTypeKey);

                    Assert.IsTrue(clientQuestionnaires.Count > 0);
                }
            }
        }

        /// <summary>
        /// Send a Client Internal Email Pass
        /// </summary>
        [Test]
        public void SendClientSurveyInternalEmailPass()
        {
            using (new SessionScope(FlushAction.Never))
            {
                //exclude PL accounts for development
                string query = @"select top 1
	                                offer.OfferKey
                                from
	                                [2am]..Account account (nolock)
                                join
	                                [2am]..Offer offer (nolock) on account.AccountKey = offer.ReservedAccountKey
                                join
	                                [2am]..OfferMailingAddress oma (nolock) on oma.OfferKey = offer.OfferKey and oma.LegalEntityKey is not null
                                where
	                                offer.OfferStatusKey = 1
                                and
                                    rrr_productkey != 12";
                DataTable DT = base.GetQueryResults(query);

                if (DT == null || DT.Rows.Count <= 0)
                    Assert.Ignore("Unable to find any Offer records.");
                else
                {
                    int offerKey = Convert.ToInt32(DT.Rows[0][0].ToString());

                    query = @"select top 1 ADUserName from [2am]..ADUser where ADUserName is not null and ADUserName <> ''";
                    DT = base.GetQueryResults(query);
                    string adUsername = DT.Rows[0][0].ToString();

                    BusinessEventQuestionnaire_DAO be = BusinessEventQuestionnaire_DAO.FindFirst();
                    if (be == null || be.Key <= 0)
                        Assert.Ignore("Unable to find any BusinessEventQuestionnaire records.");
                    else
                        _surveyRepo.SendClientSurveyEmail(be.Key, offerKey, adUsername, false);
                }
            }
        }

        /// <summary>
        /// Send a Client Survey External Email
        /// </summary>
        [Test]
        public void SendClientSurveyExternalEmailPass()
        {
            //There is no need to test the message service as we don't want to send emails,
            //it is however also not neccesary to mock the entire test... correct me if i'm wrong
            //so, let's get an account
            using (new SessionScope(FlushAction.Never))
            {
                //exclude PL accounts for development
                string query = @"select top 1
	                                offer.OfferKey
                                from
	                                [2am]..Account account (nolock)
                                join
	                                [2am]..Offer offer (nolock) on account.AccountKey = offer.ReservedAccountKey
                                join
	                                [2am]..OfferMailingAddress oma (nolock) on oma.OfferKey = offer.OfferKey and oma.LegalEntityKey is not null
                                where
	                                offer.OfferStatusKey = 1
                                and
                                    rrr_productkey != 12";
                DataTable DT = base.GetQueryResults(query);

                if (DT == null || DT.Rows.Count <= 0)
                    Assert.Ignore("Unable to find any Offer records.");
                else
                {
                    int offerKey = Convert.ToInt32(DT.Rows[0][0].ToString());

                    query = @"select top 1 ADUserName from [2am]..ADUser where ADUserName is not null and ADUserName <> ''";
                    DT = base.GetQueryResults(query);
                    string adUsername = DT.Rows[0][0].ToString();

                    BusinessEventQuestionnaire_DAO be = BusinessEventQuestionnaire_DAO.FindFirst();
                    if (be == null || be.Key <= 0)
                        Assert.Ignore("Unable to find any BusinessEventQuestionnaire records.");
                    else
                        _surveyRepo.SendClientSurveyEmail(be.Key, offerKey, adUsername, true);
                }
            }
        }
    }
}