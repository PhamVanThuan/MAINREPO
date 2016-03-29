using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Drawing;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Displays Client Survey Details.
    /// </summary>
    public class ClientQuestionnaireDetails : Panel, INamingContainer
    {
        #region private variables
        private string _titleText = "";
        private int _clientQuestionnaireKey;
        private bool _readOnly;
        private HtmlTable _questionnaireTableHeading; 
        private HtmlTable _questionnaireTableDetails;
        private IList<SAHLSurveyAnswer> _surveyAnswerControls; // = new List<SAHLSurveyAnswer>();
        #endregion

        #region public properties
        /// <summary>
        /// Determines whether we are in design mode (standard DesignMode not reliable).
        /// </summary>
        protected static new bool DesignMode
        {
            get
            {
                return (HttpContext.Current == null);
            }
        }

        /// <summary>
        /// Sets the TitleText of the panel.
        /// </summary>
        public string TitleText
        {
            set { _titleText = value; }
            get { return _titleText; }
        }

        public int ClientQuestionnaireKey 
        {
            set { _clientQuestionnaireKey = value; }
            get { return _clientQuestionnaireKey; }
        }

        public bool ReadOnly
        {
            set { _readOnly = value; }
            get { return _readOnly; }
        }

        public IList<SAHLSurveyAnswer> SurveyAnswerControls
        {
            get 
            {
                _surveyAnswerControls = new List<SAHLSurveyAnswer>();

                //string controlID1 = "ctl00_Main_ClientQuestionnaireControl_ctl13__answerControlRbnList";
                //string controlID2 = "ctl00_Main_ClientQuestionnaireControl_ctl13__answerControlRbnList";

                //Control ctl = FindControlRecursive(this, controlID1);
                //Control ct2 = FindControlRecursive(this, controlID2);

                //Control ct3 = FindControl(controlID1);
                //Control ct4 = FindControl(controlID2);



                


                // foreach (Control control in this.Controls)
                //{
                //    //if the control is of type SAHLSurveyAnswer then add to list
                //    if (control is SAHLSurveyAnswer)
                //    {
                //        _surveyAnswerControls.Add(control as SAHLSurveyAnswer);
                //    }

                //}

                //get a list of the contols and add to list
                return _surveyAnswerControls; 
            }
        }

        /// <summary>
        /// Finds a Control recursively. Note finds the first match and exists
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static Control FindControlRecursive(Control Root, string Id)
        {
            if (Root.ID == Id)
                return Root;

            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);
                if (FoundCtl != null)
                    return FoundCtl;
            }

            return null;
        }

        public T FindControl<T>(string id) where T : Control
        {
            return FindControl<T>(Page, id);
        }

        public static T FindControl<T>(Control startingControl, string id) where T : Control
        {
            T found = null;

            foreach (Control activeControl in startingControl.Controls)
            {
                found = activeControl as T;

                if (found == null)
                {
                    found = FindControl<T>(activeControl, id);
                }
                else if (string.Compare(id, found.ID, true) != 0)
                {
                    found = null;
                }

                if (found != null)
                {
                    break;
                }
            }

            return found;
        }
        #endregion

        /// <summary>
        /// Constructor. Initializes a number of controls used.
        /// </summary>
        public ClientQuestionnaireDetails()
        {
            base.GroupingText = _titleText;

            // panel setup
            base.Width = new Unit(100, UnitType.Percentage);
            base.Height = new Unit(50, UnitType.Percentage);
            //base.BackColor = Color.Bisque;
            base.ScrollBars = System.Web.UI.WebControls.ScrollBars.Auto;

            // questionnaire header table setup
            _questionnaireTableHeading = new HtmlTable();
            _questionnaireTableHeading.Width = "100%";
            _questionnaireTableHeading.BgColor = "Silver";
            _questionnaireTableHeading.Attributes.Add("class", "tableStandard");

            // questionnaire details table setup
            _questionnaireTableDetails = new HtmlTable();
            _questionnaireTableDetails.Attributes.Add("class", "tableStandard");
            _questionnaireTableDetails.Width = "100%";
            //_questionnaireTableDetails.BgColor = "Green";

            if (DesignMode)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_clientQuestionnaireKey <= 0)
                return;

            // lets get the ClientQuestionnaire object from our domain
            ISurveyRepository surveyRepo = RepositoryFactory.GetRepository<ISurveyRepository>();
            IClientQuestionnaire clientQuestionnaire = surveyRepo.GetClientQuestionnaireByKey(_clientQuestionnaireKey);

            #region setup questionnaire header
            HtmlTableRow headerRow = new HtmlTableRow();

            // column 1
            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.Attributes.Add("class", "TitleText");
            cell1.Width = "50%"; 
            SAHLLabel lbl = new SAHLLabel();
            lbl.Text = "Questionnaire : " + clientQuestionnaire.BusinessEventQuestionnaire.Questionnaire.Description; 
            lbl.Font.Size = FontUnit.Larger;
            lbl.TextAlign = TextAlign.Left;
            cell1.Controls.Add(lbl);
            headerRow.Cells.Add(cell1);

            // column 2
            HtmlTableCell cell2 = new HtmlTableCell();
            cell2.Attributes.Add("class", "TitleText");
            cell2.Align = "Right"; 
            cell2.Width = "50%"; 
            lbl = new SAHLLabel();
            lbl.Text = "Legal Entity : " + clientQuestionnaire.LegalEntity.GetLegalName(LegalNameFormat.Full); ;
            lbl.Font.Size = FontUnit.Larger;
            lbl.TextAlign = TextAlign.Right;
            cell2.Controls.Add(lbl);
            headerRow.Cells.Add(cell2);

            // add the header row to the header table
            _questionnaireTableHeading.Rows.Add(headerRow);

            // add the header table to the controls panel
            base.Controls.Add(_questionnaireTableHeading);

            #endregion

            #region setup questionnaire details

            foreach (IQuestionnaireQuestion questionnaireQuestion in clientQuestionnaire.BusinessEventQuestionnaire.Questionnaire.Questions)
            {
                // get the Client Answer collection
                IList<IClientAnswer> clientAnswers = surveyRepo.GetClientAnswersForQuestion(clientQuestionnaire, questionnaireQuestion);              

                int questionSequence = questionnaireQuestion.Sequence;
                string questionText = questionnaireQuestion.Question.Description;
                
                #region add the question row
                
                HtmlTableRow tableRow = new HtmlTableRow();

                cell1 = new HtmlTableCell();
                cell1.Attributes.Add("class", "TitleText");
                cell1.BgColor = "Bisque"; 
                cell1.Width = "20px";
                cell1.InnerText = Convert.ToString(questionSequence) + ".";
                tableRow.Cells.Add(cell1);

                cell2 = new HtmlTableCell();
                cell2.Attributes.Add("class", "TitleText");
                cell2.BgColor = "Bisque"; 
                cell2.InnerHtml = questionText;
                tableRow.Cells.Add(cell2);

                _questionnaireTableDetails.Rows.Add(tableRow);

                #endregion

                #region add answer control row

                SAHLSurveyAnswer answerControl = new SAHLSurveyAnswer();
                answerControl.QuestionaireQuestionAnswers = questionnaireQuestion.QuestionAnswers;
                answerControl.ReadOnly = _readOnly;

                tableRow = new HtmlTableRow();

                cell1 = new HtmlTableCell();
                cell1.Width = "20px";
                cell1.InnerText = "";
                tableRow.Cells.Add(cell1);

                cell2 = new HtmlTableCell();
                cell2.Controls.Add(answerControl);
                tableRow.Cells.Add(cell2);

                _questionnaireTableDetails.Rows.Add(tableRow);

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
                            default:
                                break;
                        }


                        tableRow = new HtmlTableRow();

                        cell1 = new HtmlTableCell();
                        cell1.Width = "20px";
                        cell1.InnerText = "";
                        tableRow.Cells.Add(cell1);

                        cell2 = new HtmlTableCell();
                        cell2.InnerText = answerText;
                        //cell2.BgColor = "Yellow";
                        //cell2.Controls.Add(surveyAnswerControl);
                        tableRow.Cells.Add(cell2);

                        _questionnaireTableDetails.Rows.Add(tableRow);
                    }
                }

                #endregion

                #region add a blank row
                HtmlTableRow rowBlank = new HtmlTableRow();
                HtmlTableCell cellBlank = new HtmlTableCell();
                Label lblBlank = new Label();
                lblBlank.Text = " ";
                cellBlank.Controls.Add(lblBlank);
                rowBlank.Cells.Add(cellBlank);

                _questionnaireTableDetails.Rows.Add(rowBlank);

                #endregion
            }

            base.Controls.Add(_questionnaireTableDetails);

            #endregion
        }

        ///// <summary>
        ///// A helper method to render html row for Question & Answers
        ///// </summary>
        ///// <param name="questionSequence"></param>
        ///// <param name="questionText"></param>
        ///// <param name="answerText"></param>
        ///// <param name="IList<IClientAnswer> clientAnswers"></param>
        //protected void AddSurveyQuestionAnswer(IQuestionnaireQuestion questionnaireQuestion, IList<IClientAnswer> clientAnswers)
        //{
        //    int questionSequence = questionnaireQuestion.Sequence;
        //    string questionText = questionnaireQuestion.Question.Description;          

        //    //
        //    // add the question row
        //    //
        //    HtmlTableRow tableRow = new HtmlTableRow();

        //    HtmlTableCell cell1 = new HtmlTableCell();
        //    cell1.Attributes.Add("class", "TitleText");
        //    cell1.BgColor = "Bisque"; // "#80abe8";
        //    cell1.Width = "20px";
        //    cell1.InnerText = Convert.ToString(questionSequence) + ".";
        //    tableRow.Cells.Add(cell1);

        //    HtmlTableCell cell2 = new HtmlTableCell();
        //    cell2.Attributes.Add("class", "TitleText");
        //    cell2.BgColor = "Bisque"; // "#80abe8";
        //    cell2.InnerHtml = questionText;
        //    tableRow.Cells.Add(cell2);

        //    _questionnaireTableDetails.Rows.Add(tableRow);

        //    //
        //    // add answer control row
        //    //
        //    SAHLSurveyAnswer answerControl = new SAHLSurveyAnswer();
        //    answerControl.QuestionaireQuestionAnswers = questionnaireQuestion.QuestionAnswers;
        //    answerControl.ReadOnly = _readOnly;
        //    //WebControl answerControl = GetSurveyAnswerControl(questionnaireQuestion.QuestionAnswers);

        //    //_surveyAnswerControls.Add(answerControl);

        //    tableRow = new HtmlTableRow();

        //    cell1 = new HtmlTableCell();
        //    cell1.Width = "20px";
        //    cell1.InnerText = "";
        //    tableRow.Cells.Add(cell1);

        //    cell2 = new HtmlTableCell();
        //    //cell2.InnerText = answerText;
        //    //cell2.BgColor = "Yellow";
        //    cell2.Controls.Add(answerControl);
        //    tableRow.Cells.Add(cell2);

        //    _questionnaireTableDetails.Rows.Add(tableRow);

        //    // add the answer row(s) - without the actual answers

        //    //foreach (IQuestionaireQuestionAnswer questionaireQuestionAnswer in questionnaireQuestion.QuestionAnswers)
        //    //{
        //    //    string answerText = "IQuestionaireQuestionAnswer.Answer.Description - " + questionaireQuestionAnswer.Answer.Description;
        //    //    //check the type of answer
        //    //    switch (questionaireQuestionAnswer.Answer.AnswerType.Key)
        //    //    {
        //    //        case (int)SAHL.Common.Globals.AnswerTypes.Alphanumeric:
        //    //            answerText += " (TextBox - Alphanumeric)";
        //    //            break;
        //    //        case (int)SAHL.Common.Globals.AnswerTypes.Boolean:
        //    //            answerText += " (RadioButtons - Boolean)";
        //    //            break;
        //    //        case (int)SAHL.Common.Globals.AnswerTypes.Comment:
        //    //            answerText += " (TextBox - Comment)";
        //    //            break;
        //    //        case (int)SAHL.Common.Globals.AnswerTypes.Date:
        //    //            answerText += " (TextBox - Date)";
        //    //            break;
        //    //        case (int)SAHL.Common.Globals.AnswerTypes.Rating:
        //    //            answerText += " (RadioButton - Rating)";
        //    //            break;
        //    //        case (int)SAHL.Common.Globals.AnswerTypes.None:
        //    //            answerText += " (None)";
        //    //            break;
        //    //        case (int)SAHL.Common.Globals.AnswerTypes.Numeric:
        //    //            answerText += " (TextBox - Numeric)";
        //    //            break;
        //    //        case (int)SAHL.Common.Globals.AnswerTypes.Ranking:
        //    //            answerText += " IQuestionaireQuestionAnswer.Sequence: " + questionaireQuestionAnswer.Sequence + " (Ranking List - Ranking)";
        //    //            break;
        //    //        default:
        //    //            break;
        //    //    }

        //    //    tableRow = new HtmlTableRow();

        //    //    cell1 = new HtmlTableCell();
        //    //    cell1.Width = "20px";
        //    //    cell1.InnerText = "";
        //    //    tableRow.Cells.Add(cell1);

        //    //    cell2 = new HtmlTableCell();
        //    //    cell2.InnerText = answerText;
        //    //    //cell2.Controls.Add(surveyAnswerControl);
        //    //    tableRow.Cells.Add(cell2);

        //    //    _questionnaireTableDetails.Rows.Add(tableRow);
        //    //}

        //    // add the clients answer row(s)
        //    if (clientAnswers != null)
        //    {

        //        foreach (IClientAnswer clientAnswer in clientAnswers)
        //        {
        //            string answerText = "IClientAnswer.Answer.Description - " + clientAnswer.Answer.Description;

        //            //check the type of answer
        //            switch (clientAnswer.Answer.AnswerType.Key)
        //            {
        //                case (int)SAHL.Common.Globals.AnswerTypes.Alphanumeric:
        //                    answerText += " (TextBox - Alphanumeric)";
        //                    break;
        //                case (int)SAHL.Common.Globals.AnswerTypes.Boolean:
        //                    answerText += " (RadioButtons - Boolean)";
        //                    break;
        //                case (int)SAHL.Common.Globals.AnswerTypes.Comment:
        //                    answerText += " (TextBox - Comment)";
        //                    break;
        //                case (int)SAHL.Common.Globals.AnswerTypes.Date:
        //                    answerText += " (TextBox - Date)";
        //                    break;
        //                case (int)SAHL.Common.Globals.AnswerTypes.Rating:
        //                    answerText += " (RadioButtons - Rating)";
        //                    break;
        //                case (int)SAHL.Common.Globals.AnswerTypes.None:
        //                    answerText += " (None)";
        //                    break;
        //                case (int)SAHL.Common.Globals.AnswerTypes.Numeric:
        //                    answerText += " (TextBox - Numeric)";
        //                    break;
        //                case (int)SAHL.Common.Globals.AnswerTypes.Ranking:
        //                    answerText += " IClientAnswer.ClientAnswerValue.Value " + clientAnswer.ClientAnswerValue.Value + " (Ranking List - Ranking)";
        //                    break;
        //                default:
        //                    break;
        //            }


        //            tableRow = new HtmlTableRow();

        //            cell1 = new HtmlTableCell();
        //            cell1.Width = "20px";
        //            cell1.InnerText = "";
        //            tableRow.Cells.Add(cell1);

        //            cell2 = new HtmlTableCell();
        //            cell2.InnerText = answerText;
        //            //cell2.BgColor = "Yellow";
        //            //cell2.Controls.Add(surveyAnswerControl);
        //            tableRow.Cells.Add(cell2);

        //            _questionnaireTableDetails.Rows.Add(tableRow);
        //        }
        //    }

        //    // add a blank row
        //    HtmlTableRow rowBlank = new HtmlTableRow();
        //    HtmlTableCell cellBlank = new HtmlTableCell();
        //    cellBlank.InnerText = "";
        //    rowBlank.Cells.Add(cellBlank);

        //    _questionnaireTableDetails.Rows.Add(rowBlank);
        //}

        //private WebControl GetSurveyAnswerControl(IEventList<IQuestionaireQuestionAnswer> questionaireQuestionAnswers)
        //{
        //    // sort the questionaireQuestionAnswers by sequence


        //    // get the answer type from the first answer
        //    int answerTypeKey = questionaireQuestionAnswers[0].Answer.AnswerType.Key;

        //    // go build the control
        //    switch (answerTypeKey)
        //    {
        //        case (int)SAHL.Common.Globals.AnswerTypes.Alphanumeric:
        //            SAHLTextBox answerControlText = new SAHLTextBox();
        //            answerControlText.ReadOnly = _readOnly;
        //            //answerControlText.ID = "answerControlText";
        //            return answerControlText;
        //        case (int)SAHL.Common.Globals.AnswerTypes.Boolean:
        //        case (int)SAHL.Common.Globals.AnswerTypes.Rating:
        //            // check to see how many radiobuttons we need
        //            RadioButtonList answerControlRbnList = new RadioButtonList();
        //            answerControlRbnList.Enabled = !_readOnly;
        //            //answerControlRbnList.ID = "answerControlRbnList";
        //            answerControlRbnList.RepeatDirection = RepeatDirection.Horizontal;
        //            foreach (IQuestionaireQuestionAnswer questionaireQuestionAnswer in questionaireQuestionAnswers)
        //            {
        //                ListItem li = new ListItem(questionaireQuestionAnswer.Answer.Description, questionaireQuestionAnswer.Answer.Key.ToString());
        //                answerControlRbnList.Items.Add(li);
        //            }
        //            return answerControlRbnList;
        //        case (int)SAHL.Common.Globals.AnswerTypes.Comment:
        //            SAHLTextBox answerControlComment = new SAHLTextBox();
        //            answerControlComment.ReadOnly = _readOnly;
        //            //answerControlComment.ID = "answerControlComment";
        //            answerControlComment.TextMode = TextBoxMode.MultiLine;
        //            answerControlComment.Height = new Unit(70, UnitType.Pixel);
        //            answerControlComment.Width = new Unit(500, UnitType.Pixel);
        //            return answerControlComment;
        //        case (int)SAHL.Common.Globals.AnswerTypes.Date:
        //            SAHLDateBox answerControlDate = new SAHLDateBox();
        //            answerControlDate.ReadOnly = _readOnly;
        //            //answerControlDate.ID = "answerControlDate";
        //            return answerControlDate;
        //        case (int)SAHL.Common.Globals.AnswerTypes.Numeric:
        //            SAHLTextBox answerControlNumeric = new SAHLTextBox();
        //            answerControlNumeric.ReadOnly = _readOnly;
        //            answerControlNumeric.DisplayInputType = InputType.Number;
        //            //answerControlNumeric.ID = "answerControlNumeric";
        //            return answerControlNumeric;
        //        case (int)SAHL.Common.Globals.AnswerTypes.Ranking:
        //            break;
        //        case (int)SAHL.Common.Globals.AnswerTypes.None:
        //            break;
        //        default:
        //            break;
        //    }

        //    //foreach (IQuestionaireQuestionAnswer questionaireQuestionAnswer in questionaireQuestionAnswers)
        //    //{
        //    //}

        //    SAHLLabel answerControlNone = new SAHLLabel();
        //    return answerControlNone;
        //}
    }
}