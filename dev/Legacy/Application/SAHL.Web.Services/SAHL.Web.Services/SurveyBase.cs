using System;
using System.Collections.Generic;
using System.Threading;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord;
using System.Reflection;
using System.Text;
using System.Web;
using SAHL.Common.Logging;
using SAHL.Communication;

namespace SAHL.Web.Services
{
    public class SurveyBase
    {
        private ISurveyRepository _surveyRepo;
        ///<summary>
        ///</summary>
        public ISurveyRepository SurveyRepo
        {
            get
            {
                if (_surveyRepo == null)
                    _surveyRepo = RepositoryFactory.GetRepository<ISurveyRepository>();

                return _surveyRepo;
            }
        }

        public ClientQuestionnaire GetClientQuestionnaireByGUID(string GUID)
        {
            ClientQuestionnaire clientQuestionnaire = new ClientQuestionnaire();

            using (new SessionScope())
            {
                IClientQuestionnaire cq = SurveyRepo.GetClientQuestionnaireByGUID(GUID);

                if (cq != null)
                {
                    clientQuestionnaire.GUID = GUID;
                    clientQuestionnaire.DateReceived = cq.DateReceived;
                    clientQuestionnaire.QuestionnaireQuestions = new List<QuestionnaireQuestion>();

                    foreach (IQuestionnaireQuestion qq in cq.BusinessEventQuestionnaire.Questionnaire.Questions)
                    {
                        QuestionnaireQuestion questionnaireQuestion = new QuestionnaireQuestion();
                        questionnaireQuestion.Key = qq.Key;
                        questionnaireQuestion.Sequence = qq.Sequence;
                        questionnaireQuestion.Description = qq.Question.Description;
                        questionnaireQuestion.QuestionAnswers = new List<QuestionnaireAnswer>();

                        foreach (IQuestionnaireQuestionAnswer qqa in qq.QuestionAnswers)
                        {
                            QuestionnaireAnswer questionnaireAnswer = new QuestionnaireAnswer();
                            questionnaireAnswer.AnswerKey = qqa.Answer.Key;
                            questionnaireAnswer.AnswerTypeKey = qqa.Answer.AnswerType.Key;
                            questionnaireAnswer.AnswerDescription = qqa.Answer.Description;

                            questionnaireQuestion.QuestionAnswers.Add(questionnaireAnswer);
                        }


                        clientQuestionnaire.QuestionnaireQuestions.Add(questionnaireQuestion);
                    }
                }
            }

            return clientQuestionnaire;
        }

        /// <summary>
        /// Save Client Questionnaire
        /// </summary>
        /// <param name="surveyResult"></param>
        /// <returns></returns>
        public bool SaveClientQuestionnaire(SurveyResult surveyResult)
        {
            LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Attempting to save Client Survey");

            bool success = true;

            using (new SessionScope())
            {
                LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Getting the GUID from the Survey");
                string GUID = surveyResult.GUID;

                if (!String.IsNullOrEmpty(GUID))
                {
                    LogPlugin.Logger.LogFormattedInfo(MethodInfo.GetCurrentMethod().Name, "GUID : {0} and Calling : SurveyRepo.GetClientQuestionnaireByGUID(GUID)", GUID);

                    // get the Client Questionnaire record
                    IClientQuestionnaire clientQuestionnaire = SurveyRepo.GetClientQuestionnaireByGUID(GUID);
                    if (clientQuestionnaire != null)
                    {
                        LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Starting a new Transaction");
                        TransactionScope txn = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback);
                        try
                        {
                            // update the ClientQuestionnaire Table
                            //clientQuestionnaire.ADUser = _orgStructureRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                            clientQuestionnaire.DateReceived = DateTime.Now;
                            //clientQuestionnaire.LegalEntity = legalEntity;

                            LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "We are going to loop through each of the Survey Result Question Answers and saving them");
                            foreach (SurveyQuestionAnswer sqa in surveyResult.SurveyQuestionAnswers)
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
                                        LogPlugin.Logger.LogFormattedInfo(MethodInfo.GetCurrentMethod().Name, "QuestionnaireQuestionKey : {0}, AnswerKey : {1}", sqa.QuestionnaireQuestionKey, sqa.AnswerKey);
                                        IClientAnswer clientAnswer = SurveyRepo.CreateEmptyClientAnswer();
                                        clientAnswer.ClientSurvey = clientQuestionnaire;
                                        clientAnswer.QuestionnaireQuestion = SurveyRepo.GetQuestionnaireQuestionByKey(sqa.QuestionnaireQuestionKey);
                                        clientAnswer.Answer = SurveyRepo.GetAnswerByKey(sqa.AnswerKey);

                                        // add the ClientAnswerVaue records (if any)
                                        if (String.IsNullOrEmpty(sqa.AnswerValue) == false)
                                        {
                                            IClientAnswerValue clientAnswerValue = SurveyRepo.CreateEmptyClientAnswerValue();
                                            clientAnswerValue.Value = sqa.AnswerValue.Replace("'", "''");
                                            clientAnswerValue.ClientAnswer = clientAnswer;

                                            clientAnswer.ClientAnswerValue = clientAnswerValue;

                                            LogPlugin.Logger.LogFormattedInfo(MethodInfo.GetCurrentMethod().Name, "Survey Question Answer : {0}", sqa.AnswerValue);
                                            LogPlugin.Logger.LogFormattedInfo(MethodInfo.GetCurrentMethod().Name, "Client Answer Value : {0}", clientAnswerValue.Value);
                                        }

                                        clientQuestionnaire.ClientAnswers.Add(null, clientAnswer);
                                        LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Added Client Answer");
                                    }
                                }
                            }

                            //Add the Rule Exclusion Set
                            SAHLPrincipalCache spc = GetSAHLPrinciple();
                            if (spc != null)
                            {
                                spc.ExclusionSets.Add(Common.Globals.RuleExclusionSets.WebServiceClientQuestionnaire);
                            }
                            // save the ClientQuestionnaire record

                            LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Calling : SurveyRepo.SaveClientQuestionnaire");
                            SurveyRepo.SaveClientQuestionnaire(clientQuestionnaire);

                            LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Committing the transaction");
                            txn.VoteCommit();

                            //Remove the Rule Exclusion Set
                            if (spc != null)
                            {
                                spc.ExclusionSets.Remove(Common.Globals.RuleExclusionSets.WebServiceClientQuestionnaire);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "We failed to save and printing exception information");
                            try
                            {
                                StringBuilder errorBuilder = new StringBuilder();
                                SAHLPrincipalCache spc = GetSAHLPrinciple();
                                if (spc.DomainMessages.HasErrorMessages)
                                {
                                    errorBuilder.AppendLine("Domain Messages : Errors");
                                    foreach (IDomainMessage message in spc.DomainMessages.ErrorMessages)
                                    {
                                        errorBuilder.AppendFormat("Message : {0}{1}", message.Message, Environment.NewLine);
                                        errorBuilder.AppendFormat("Detail : {0}{1}", message.Details, Environment.NewLine);
                                    }
                                }

                                if (spc.DomainMessages.HasWarningMessages)
                                {
                                    errorBuilder.AppendLine("Domain Messages : Warnings");
                                    foreach (IDomainMessage message in spc.DomainMessages.WarningMessages)
                                    {
                                        errorBuilder.AppendFormat("Message : {0}{1}", message.Message, Environment.NewLine);
                                        errorBuilder.AppendFormat("Detail : {0}{1}", message.Details, Environment.NewLine);
                                    }
                                }

                                errorBuilder.AppendFormat("GUID : {0}", GUID);

                                //Send the first report
                                LogPlugin.Logger.LogErrorMessage(MethodInfo.GetCurrentMethod().Name, errorBuilder.ToString());

                            }
                            catch(Exception innerException)
                            {
                                LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "We Failed attempting to get any additional SPC or Survey information");
                                LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, innerException.Message, innerException);
                            }

                            //Send the exception
                            LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, ex.Message, ex);

                            txn.VoteRollBack();
                        }
                        finally
                        {
                            txn.Dispose();
                        }
                    }
                }

            }
            return success;
        }

        /// <summary>
        /// Get SAHL Principle
        /// </summary>
        /// <returns></returns>
        private SAHLPrincipalCache GetSAHLPrinciple()
        {
            LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Getting SAHL Principle");
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            try
            {
                LogPlugin.Logger.LogFormattedInfo(MethodInfo.GetCurrentMethod().Name, "SAHL Principle Name : {0}", spc.Principal.Identity.Name);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogFormattedErrorWithException(MethodInfo.GetCurrentMethod().Name, "We failed to retrieve any SPC related information",ex);
                LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, ex.ToString());
            }

            return spc;
        }
    }
}
