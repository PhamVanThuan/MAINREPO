using System;
using SAHL.Internet.SAHL.Web.Services.Survey;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Internet.Components.Survey
{
    /// <summary>
    /// Acts as a wrapper around a SA Home Loans survey.
    /// </summary>
    public partial class Survey : UserControl
    {
        private readonly SurveyBase _surveyBase = new SurveyBase();
        private ClientQuestionnaire _clientQuestionnaire;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lblMessage.Text = "Thank you for your interest in us. Please fill out the below survey and help us to improve and better serve you!";
            bttnSubmit.Visible = false;

            if (IsPostBack) return;

            // check if there is a variable and get the GUID
            var guid = Request.QueryString["survey"] ?? String.Empty; // if something was passed to the file querystring
            if (String.IsNullOrEmpty(guid)) return;

            _clientQuestionnaire = _surveyBase.GetClientQuestionnaireByGuid(guid);
            if (_clientQuestionnaire == null || _clientQuestionnaire.QuestionnaireQuestions == null || _clientQuestionnaire.QuestionnaireQuestions.Length <= 0)
            {
                lblMessage.Text = "The specified survey could not be found or the survey has no questions.";
                return;
            }

            if (_clientQuestionnaire.DateReceived.HasValue)
            {
                lblMessage.Text = "Survey has already been submitted";
                return;
            }
            
            // survey hasnt been taken so lets show it
            bttnSubmit.Visible = true;
            surveyControl.Questionnaire = _clientQuestionnaire;
        }

        /// <summary>
        /// Submits the survey.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitSurvey(object sender, EventArgs e)
        {
            var guid = Request.QueryString["survey"];
            var surveySubmitted = Session[guid] != null;

            if (!surveySubmitted)
            {
                var surveyQuestionAnswers = new List<SurveyQuestionAnswer>();

                foreach (var controlKey in Request.Form.AllKeys)
                {
                    if (!controlKey.Contains("SAHLSurveyAnswer")) continue;
                    var controlValues = Request.Form.GetValues(controlKey);
                    if (controlValues == null) continue;

                    var controlValue = controlValues[0];

                    // split the QuestionaireQuestionKey, AnswerTypKey & AnswerKey out of the controlName
                    var nameParts = controlKey.Split('_').ToList();
                    var questionaireQuestionKey = Convert.ToInt32(nameParts[nameParts.Count - 3]);
                    var answerTypeKey = Convert.ToInt32(nameParts[nameParts.Count - 2]);
                    var answerKey = Convert.ToInt32(nameParts[nameParts.Count - 1]);
                    var answerValue = string.Empty;

                    if (answerKey == 0)
                    {
                        // use the value attribute which contains the value
                        answerKey = Convert.ToInt32(controlValue);
                        answerValue = string.Empty;
                    }
                    else // textboxes, ranking list, MultiSelect
                    {
                        if (answerTypeKey != 9) // MultiSelect = 9
                            answerValue = controlValue;
                    }

                    surveyQuestionAnswers.Add(new SurveyQuestionAnswer
                                                  {
                                                      QuestionnaireQuestionKey = questionaireQuestionKey,
                                                      AnswerTypeKey = answerTypeKey,
                                                      AnswerKey = answerKey,
                                                      AnswerValue = answerValue
                                                  });
                }

                var surveyResult = new SurveyResult();
                // get the guid from the session
                if (!string.IsNullOrEmpty(guid))
                {
                    surveyResult.GUID = guid;
                    surveyResult.SurveyQuestionAnswers = surveyQuestionAnswers.ToArray();
                    _surveyBase.SaveClientQuestionnaire(surveyResult);

                    Session[guid] = guid;

                    surveyControl.Visible = false;
                    bttnSubmit.Visible = false;
                    lblMessage.Text = "Thank you for your feedback.";
                }
            }
            else
            {
                surveyControl.Visible = false;
                bttnSubmit.Visible = false;
                lblMessage.Text = "Survey has already been submitted - Thank you for your feedback.";
            }
        }
    }
}