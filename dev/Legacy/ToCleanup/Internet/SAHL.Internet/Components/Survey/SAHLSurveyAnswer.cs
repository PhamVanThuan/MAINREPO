using System;
using System.Linq;
using System.Web.UI.WebControls;
using SAHL.Internet.SAHL.Web.Services.Survey;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace SAHL.Internet.Components.Survey
{
    /// <summary>
    /// Represents an answer within a survey.
    /// </summary>
    public class SahlSurveyAnswer : CompositeControl
    {
        protected enum AnswerTypes
        {
            Comment = 1,
            Numeric = 2,
            Alphanumeric = 3,
            Boolean = 4,
            Date = 5,
            Ranking = 6,
            Rating = 7,
            SingleSelect = 8,
            MultiSelect = 9,
            None = 10
        }

        private readonly QuestionnaireQuestion _question;

        /// <summary>
        /// Gets the question associated with the answer.
        /// </summary>
        protected QuestionnaireQuestion Question
        {
            get { return _question; }
        }
        
        /// <summary>
        /// Gets the answer associated with the answer.
        /// </summary>
        protected QuestionnaireAnswer Answer
        {
            get
            {
                if (_question.QuestionAnswers == null || _question.QuestionAnswers.Length == 0) return null;
                return _question.QuestionAnswers[0];
            }
        }

        /// <summary>
        /// Gets the answer type.
        /// </summary>
        protected AnswerTypes AnswerType
        {
            get
            {
                return (_question.QuestionAnswers == null || _question.QuestionAnswers.Length == 0)
                           ? AnswerTypes.None
                           : (AnswerTypes) _question.QuestionAnswers[0].AnswerTypeKey;
            }
        }

        /// <summary>
        /// Gets or sets the programmatic identifier assigned to the server control.
        /// </summary>
        /// <returns>
        /// The programmatic identifier assigned to the control.
        /// </returns>
        public override string ID
        {
            get { return "SAHLSurveyAnswer" + "_" + _question.Sequence; }
            set { base.ID = value; }
        }
        
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            Control control;

            // go build the control
            switch (AnswerType)
            {
                case AnswerTypes.Alphanumeric:
                    #region TextBox control

                    control = new TextBox { ID = ID + "_answerControlText" + '_' + Question.Key + '_' + Answer.AnswerTypeKey + '_' + Answer.AnswerKey };

                    #endregion
                    break;
                case AnswerTypes.Boolean:
                case AnswerTypes.Rating:
                case AnswerTypes.SingleSelect:
                    #region RadioButtonList control

                    var buttonList = new RadioButtonList
                                         {
                                             ID = ID + "_answerControlRbnList" + '_' + Question.Key + '_' + Answer.AnswerTypeKey + "_0",
                                             RepeatDirection = AnswerType == AnswerTypes.SingleSelect ? RepeatDirection.Vertical : RepeatDirection.Horizontal
                                         };

                    foreach (var li in Question.QuestionAnswers.Select(answer => new ListItem(answer.AnswerDescription, answer.AnswerKey.ToString())))
                        buttonList.Items.Add(li);

                    control = buttonList;

                    #endregion
                    break;
                case AnswerTypes.Comment:
                    #region SAHLTextBox control

                    control = new TextBox
                                  {
                                      ID = ID + "_answerControlComment" + '_' + Question.Key + '_' + Answer.AnswerTypeKey + '_' + Answer.AnswerKey,
                                      TextMode = TextBoxMode.MultiLine,
                                      Height = Unit.Pixel(70),
                                      Width = Unit.Pixel(500)
                                  };

                    #endregion
                    break;
                case AnswerTypes.Date:
                    #region SAHLDateBox control

                    control = new Calendar { ID = ID + "_answerControlDate" + '_' + Question.Key + '_' + Answer.AnswerTypeKey + '_' + Answer.AnswerKey };

                    #endregion
                    break;
                case AnswerTypes.Numeric:
                    #region SAHLTextBox control

                    control = new TextBox { ID = ID + "_answerControlNumeric" + '_' + Question.Key + '_' + Answer.AnswerTypeKey + '_' + Answer.AnswerKey };

                    #endregion
                    break;
                case AnswerTypes.MultiSelect:
                    #region SAHLCheckBoxList control

                    var checkListTable = new Table();
                    foreach (var answer in Question.QuestionAnswers)
                    {
                        var id = ID + "answerControl_MultiSelect" + '_' + Question.Key + '_' + answer.AnswerTypeKey + "_" + answer.AnswerKey;

                        // create the verticle rows
                        var row = new TableRow {VerticalAlign = VerticalAlign.Middle};

                        // add checkbox to cell
                        var tc1 = new TableCell {Width = Unit.Pixel(10)};
                        tc1.Style.Add("vertical-align", "middle");
                        tc1.Controls.Add(new HtmlInputCheckBox {Name = id, ID = id});

                        // add label to cell
                        var tc2 = new TableCell {Text = answer.AnswerDescription};
                        tc2.Style.Add("vertical-align", "middle");

                        // add cells to row
                        row.Cells.Add(tc1);
                        row.Cells.Add(tc2);

                        // add row to table
                        checkListTable.Rows.Add(row);
                    }
                    control = checkListTable;

                    #endregion
                    break;
                case AnswerTypes.Ranking:
                    #region RadioButtonList Ranking control

                    var radioListTable = new Table();

                    for (var rankingRow = 0; rankingRow <= Question.QuestionAnswers.Length; rankingRow++)
                    {
                        var answerIndex = rankingRow - 1;

                        // create the verticle rows
                        var row = new TableRow {VerticalAlign = VerticalAlign.Middle};

                        // create a radiobutton list per row
                        var rbnList = new RadioButtonList();

                        if (rankingRow >= 1)
                        {
                            rbnList.ID = ID + "_answerControlRankingList" + '_' + Question.Key + '_' + Answer.AnswerTypeKey + "_" + Question.QuestionAnswers[answerIndex].AnswerKey;
                            rbnList.RepeatDirection = RepeatDirection.Horizontal;

                            // create each radiobutton 
                            for (var rankingCol = Question.QuestionAnswers.Length; rankingCol > 0; rankingCol--)
                            {
                                var li = new ListItem("", rankingCol.ToString());
                                rbnList.Items.Add(li);
                            }
                        }

                        var colHeader = Question.QuestionAnswers.Length;
                        for (var rankingCol = 0; rankingCol <= Question.QuestionAnswers.Length; rankingCol++)
                        {
                            var rankingCell = new TableCell {ColumnSpan = 1};

                            if (rankingRow == 0) // 1st row of the controls table - add column headers
                            {
                                if (rankingCol > 0)
                                {
                                    rankingCell.Text = colHeader.ToString();
                                    rankingCell.HorizontalAlign = HorizontalAlign.Center;
                                    rankingCell.Style.Add("vertical-align", "middle");
                                    colHeader--;
                                }
                            }
                            else
                            {
                                switch (rankingCol)
                                {
                                    case 0:
                                        rankingCell.Text = Question.QuestionAnswers[answerIndex].AnswerDescription;
                                        break;
                                    case 1:
                                        rankingCell.ColumnSpan = Question.QuestionAnswers.Length;
                                        rankingCell.Controls.Add(rbnList);
                                        row.Cells.Add(rankingCell);
                                        break;
                                }
                            }

                            row.Cells.Add(rankingCell);
                        }

                        // add each ranking control row to the table
                        radioListTable.Rows.Add(row);
                    }

                    // add the table to the control
                    control = radioListTable;

                    #endregion
                    break;
                default:
                    control = new Label { ID = ID + "_answerControlNone" };
                    break;
            }

            Controls.Add(control);
            base.CreateChildControls();
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content. </param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            RenderContents(writer);
            writer.RenderEndTag();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SahlSurveyAnswer"/> class.
        /// </summary>
        /// <param name="question">The questionnaire question to populate answers for.</param>
        public SahlSurveyAnswer(QuestionnaireQuestion question)
        {
            if (question == null) throw new ArgumentNullException("question");
            _question = question;
        }
    }
}
