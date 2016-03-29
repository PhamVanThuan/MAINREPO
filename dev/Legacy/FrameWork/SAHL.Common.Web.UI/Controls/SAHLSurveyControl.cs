using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Web;

namespace SAHL.Common.Web.UI.Controls
{
    public class SAHLSurveyControl : SAHLPanel
    {
        #region private variables
        private bool _readOnly, _displaySurveyHeader;
        private ISurveyRepository _surveyRepo;
        //private IClientQuestionnaire _clientQuestionnaire;
        private HtmlTable _questionnaireTableHeading;
        private HtmlTable _questionnaireTableDetails;
        //private IList<SAHLSurveyAnswer> _surveyAnswerControls; // = new List<SAHLSurveyAnswer>();
        #endregion

        #region Properties

        /// <summary>
        /// This is the main property that allows us to build the whole control
        /// </summary>
        public IClientQuestionnaire ClientQuestionnaire
        {
            set
            {
                ViewState["ClientQuestionnaire"] = value;
            }
            get
            {
                IClientQuestionnaire cq = (IClientQuestionnaire)ViewState["ClientQuestionnaire"];
                return cq;
            }
        }

        /// <summary>
        /// True - no capture of answers will be allowed
        /// False - allow for the capture of answers
        /// </summary>
        public bool ReadOnly
        {
            set { _readOnly = value; }
            get { return _readOnly; }
        }

        /// <summary>
        /// Display Survey Header containing Survey Description & LegalEntity
        /// </summary>
        public bool DisplaySurveyHeader
        {
            set { _displaySurveyHeader = value; }
            get { return _displaySurveyHeader; }
        }

        #endregion

        #region Constructor

        ///// <summary>
        ///// Constructor.
        ///// </summary>
        //public SAHLSurveyControl(IClientQuestionnaire clientQuestionnaire)
        //{
        //    // set the attributes of the control itself
        //    this.ID = "SAHLSurveyControl";
        //    _clientQuestionnaire = clientQuestionnaire;
        //}
        #endregion

        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // questionnaire header table setup
            if (_displaySurveyHeader)
            {
                _questionnaireTableHeading = new HtmlTable();
                _questionnaireTableHeading.Width = "100%";
                _questionnaireTableHeading.BgColor = "Silver";
                _questionnaireTableHeading.Attributes.Add("class", "tableStandard");
            }

            // questionnaire details table setup
            _questionnaireTableDetails = new HtmlTable();
            _questionnaireTableDetails.Attributes.Add("class", "tableStandard");
            _questionnaireTableDetails.Width = "100%";
            //_questionnaireTableDetails.BgColor = "Green";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #endregion


        #region methods
        protected override void CreateChildControls() // this happens between Begin PreRender & End PreRender
        {
            Controls.Clear();

            if(ClientQuestionnaire!=null)
                CreateControlHierarchy();

            ClearChildViewState();
        }
        protected virtual void CreateControlHierarchy()
        {
            if (_surveyRepo==null)
                _surveyRepo = RepositoryFactory.GetRepository<ISurveyRepository>();

            if (_displaySurveyHeader)
            {
                #region add questionnaire header row
                HtmlTableRow headerRow = new HtmlTableRow();

                // column 1
                HtmlTableCell headerCell1 = new HtmlTableCell();
                headerCell1.Attributes.Add("class", "TitleText");
                headerCell1.Width = "50%";
                SAHLLabel lbl = new SAHLLabel();
                lbl.Text = "Questionnaire : " + ClientQuestionnaire.BusinessEventQuestionnaire.Questionnaire.Description;
                lbl.Font.Size = FontUnit.Larger;
                lbl.TextAlign = TextAlign.Left;
                headerCell1.Controls.Add(lbl);
                headerRow.Cells.Add(headerCell1);

                // column 2
                HtmlTableCell headerCell2 = new HtmlTableCell();
                headerCell2.Attributes.Add("class", "TitleText");
                headerCell2.Align = "Right";
                headerCell2.Width = "50%";
                lbl = new SAHLLabel();
                lbl.Text = "Legal Entity : " + ClientQuestionnaire.LegalEntity.GetLegalName(LegalNameFormat.Full); ;
                lbl.Font.Size = FontUnit.Larger;
                lbl.TextAlign = TextAlign.Right;
                headerCell2.Controls.Add(lbl);
                headerRow.Cells.Add(headerCell2);

                // add the header row to the header table
                _questionnaireTableHeading.Rows.Add(headerRow);

                // add the header table to the control
                Controls.Add(_questionnaireTableHeading);

                #endregion
            }
            AddBlankTableRow(_questionnaireTableDetails, 15);

            #region add questionnaire detail row(s)

            int controlCount = 0;
            foreach (IQuestionnaireQuestion questionnaireQuestion in ClientQuestionnaire.BusinessEventQuestionnaire.Questionnaire.Questions)
            {
                controlCount++;

                // get the Client Answer collection
                IList<IClientAnswer> clientAnswers = _surveyRepo.GetClientAnswersForQuestion(ClientQuestionnaire, questionnaireQuestion);

                int questionSequence = questionnaireQuestion.Sequence;
                string questionText = questionnaireQuestion.Question.Description;

                #region add the question row

                HtmlTableRow detailRow = new HtmlTableRow();

                HtmlTableCell detailCell1 = new HtmlTableCell();
                detailCell1.Attributes.Add("class", "TitleText");
                detailCell1.BgColor = "Bisque";
                detailCell1.Width = "20px";
                detailCell1.InnerText = Convert.ToString(questionSequence) + ".";
                detailRow.Cells.Add(detailCell1);

                HtmlTableCell detailCell2 = new HtmlTableCell();
                detailCell2.Attributes.Add("class", "TitleText");
                detailCell2.BgColor = "Bisque";
                detailCell2.InnerHtml = questionText;
                detailRow.Cells.Add(detailCell2);

                _questionnaireTableDetails.Rows.Add(detailRow);

                #endregion

                #region add answer control row

                SAHLSurveyAnswer answerControl = new SAHLSurveyAnswer(questionnaireQuestion.Sequence, questionnaireQuestion.QuestionAnswers, clientAnswers, _readOnly);

                detailRow = new HtmlTableRow();

                detailCell1 = new HtmlTableCell();
                detailCell1.Width = "20px";
                detailCell1.InnerText = "";
                detailRow.Cells.Add(detailCell1);

                detailCell2 = new HtmlTableCell();
                detailCell2.Controls.Add(answerControl);
                detailRow.Cells.Add(detailCell2);

                _questionnaireTableDetails.Rows.Add(detailRow);

                #endregion

                #region TODO : add the clients answer row(s)
                if (clientAnswers != null)
                {

                    foreach (IClientAnswer clientAnswer in clientAnswers)
                    {
                        string answerText = "IClientAnswer.Answer.Description - " + clientAnswer.Answer.Description;

                        //check the type of answer
                        switch (clientAnswer.Answer.AnswerType.Key)
                        {
                            case (int)SAHL.Common.Globals.AnswerTypes.Alphanumeric:
                                answerText += " (TextBox - Alphanumeric)";
                                break;
                            case (int)SAHL.Common.Globals.AnswerTypes.Boolean:
                                answerText += " (RadioButtons - Boolean)";
                                break;
                            case (int)SAHL.Common.Globals.AnswerTypes.Comment:
                                answerText += " (TextBox - Comment)";
                                break;
                            case (int)SAHL.Common.Globals.AnswerTypes.Date:
                                answerText += " (TextBox - Date)";
                                break;
                            case (int)SAHL.Common.Globals.AnswerTypes.Rating:
                                answerText += " (RadioButtons - Rating)";
                                break;
                            case (int)SAHL.Common.Globals.AnswerTypes.None:
                                answerText += " (None)";
                                break;
                            case (int)SAHL.Common.Globals.AnswerTypes.Numeric:
                                answerText += " (TextBox - Numeric)";
                                break;
                            case (int)SAHL.Common.Globals.AnswerTypes.Ranking:
                                answerText += " IClientAnswer.ClientAnswerValue.Value " + clientAnswer.ClientAnswerValue.Value + " (Ranking List - Ranking)";
                                break;
                            case (int)SAHL.Common.Globals.AnswerTypes.SingleSelect:
                                answerText += " (RadioButtons - Single Select)";
                                break;
                            case (int)SAHL.Common.Globals.AnswerTypes.MultiSelect:
                                answerText += " (CheckBocList - Multi Select)";
                                break;
                            default:
                                break;
                        }


                        //detailRow = new HtmlTableRow();

                        //detailCell1 = new HtmlTableCell();
                        //detailCell1.Width = "20px";
                        //detailCell1.InnerText = "";
                        //detailRow.Cells.Add(detailCell1);

                        //detailCell2 = new HtmlTableCell();
                        //detailCell2.InnerText = answerText;
                        ////detailCell2.BgColor = "Yellow";
                        ////detailCell2.Controls.Add(surveyAnswerControl);
                        //detailRow.Cells.Add(detailCell2);

                        //_questionnaireTableDetails.Rows.Add(detailRow);
                    }
                }

                #endregion

                AddBlankTableRow(_questionnaireTableDetails,15);
            }

            Controls.Add(_questionnaireTableDetails);

            #endregion
        }

        /// <summary>
        /// helper method to add a blank spacer row in tbale
        /// </summary>
        /// <param name="htmlTable"></param>
        /// <param name="rowHeightPixels"></param>
        private void AddBlankTableRow(HtmlTable htmlTable, int rowHeightPixels)
        {
            string height = rowHeightPixels + "px";
            HtmlTableRow rowBlank = new HtmlTableRow();
            rowBlank.Height = height;
            HtmlTableCell cellBlank = new HtmlTableCell();
            rowBlank.Cells.Add(cellBlank);
            htmlTable.Rows.Add(rowBlank);
        }

        #endregion

        //public HtmlTableCell detailCell2 { get; set; }
    }
}
