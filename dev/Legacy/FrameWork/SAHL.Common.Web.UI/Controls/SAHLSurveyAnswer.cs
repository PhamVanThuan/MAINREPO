using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Web.UI.HtmlControls;

namespace SAHL.Common.Web.UI.Controls
{
	public class SAHLSurveyAnswer : CompositeControl
	{
		#region Private Variables

		private IEventList<IQuestionnaireQuestionAnswer> _questionnaireQuestionAnswers;
        private IQuestionnaireQuestion _questionnaireQuestion;      
		private IList<IClientAnswer> _clientAnswers;
		private bool _readOnly; 

		SAHLRadioButtonList answerControl_RadioButtonList;
		SAHLTextBox answerControl_TextBox; 
		SAHLDateBox answerControl_DateBox; 
		SAHLLabel answerControl_Label;
		Table answerControl_Ranking;
        Table answerControl_MultiSelect; 

        private int _questionnaireQuestionKey;
        private int _answerTypeKey;

		#endregion
		
        #region Constructor


		/// <summary>
		/// if there are client answers then the SAHLSurveyAnswer control will be populated with them
		/// </summary>
		public SAHLSurveyAnswer(IQuestionnaireQuestion questionnaireQuestion, IList<IClientAnswer> clientAnswers, bool readOnly)
		{
            _questionnaireQuestion = questionnaireQuestion;

            this.ID = "SAHLSurveyAnswer" + "_" + _questionnaireQuestion.Sequence;
            _questionnaireQuestionAnswers = questionnaireQuestion.QuestionAnswers;         
			_clientAnswers = clientAnswers;
			_readOnly = readOnly;

            _questionnaireQuestionKey = _questionnaireQuestion.Key;
            if (_questionnaireQuestion.QuestionAnswers != null && _questionnaireQuestion.QuestionAnswers.Count > 0)
                _answerTypeKey = _questionnaireQuestionAnswers[0].Answer.AnswerType.Key;
            else
                _answerTypeKey = (int)SAHL.Common.Globals.AnswerTypes.None;
		}
		#endregion

        #region Properties

        public int QuestionnaireQuestionKey 
        {
            get { return _questionnaireQuestionKey; }
        }
        public int AnswerTypeKey
        {
            get { return _answerTypeKey; }
        }

        #endregion

        #region Events
        protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
		}

		protected override void CreateChildControls()
		{
			IDictionary<int, string> clientAnswerKeysAndValues = new Dictionary<int, string>();

			// get a dictionary of answerkeys and/or values that the client answered (if there are any)
			clientAnswerKeysAndValues = GetAnswerKeysAndValues(_answerTypeKey, _clientAnswers);

            string controlNameAnswerKey = _questionnaireQuestion.QuestionAnswers[0].Answer.Key.ToString();
            string controlNameAnswerTypeKey = _questionnaireQuestion.QuestionAnswers[0].Answer.AnswerType.Key.ToString(); 

			// go build the control
			switch (_answerTypeKey)
			{
				case (int)SAHL.Common.Globals.AnswerTypes.Alphanumeric:
					#region SAHLTextBox control
					answerControl_TextBox = new SAHLTextBox();
					answerControl_TextBox.ReadOnly = _readOnly;
                    answerControl_TextBox.ID = this.ID + "_answerControlText" + '_' + _questionnaireQuestionKey + '_' + controlNameAnswerTypeKey + '_' + controlNameAnswerKey;                  
					// populate answer if there is one
					foreach (KeyValuePair<int, string> clientAnswers in clientAnswerKeysAndValues)
					{                   
						answerControl_TextBox.Text = clientAnswers.Value;
						break; // there should only be one answer value for this answer type
					}
					this.Controls.Add(answerControl_TextBox);
					#endregion
					break;
				case (int)SAHL.Common.Globals.AnswerTypes.Boolean:
				case (int)SAHL.Common.Globals.AnswerTypes.Rating:
				case (int)SAHL.Common.Globals.AnswerTypes.SingleSelect:
					#region RadioButtonList control
					answerControl_RadioButtonList = new SAHLRadioButtonList();
					answerControl_RadioButtonList.Enabled = !_readOnly;
                    answerControl_RadioButtonList.ID = this.ID + "_answerControlRbnList" + '_' + _questionnaireQuestionKey + '_' + controlNameAnswerTypeKey + "_0";
					if (_answerTypeKey == (int)SAHL.Common.Globals.AnswerTypes.SingleSelect)
						answerControl_RadioButtonList.RepeatDirection = RepeatDirection.Vertical;
					else
						answerControl_RadioButtonList.RepeatDirection = RepeatDirection.Horizontal;

					foreach (IQuestionnaireQuestionAnswer questionnaireQuestionAnswer in _questionnaireQuestionAnswers)
					{
						ListItem li = new ListItem(questionnaireQuestionAnswer.Answer.Description, questionnaireQuestionAnswer.Answer.Key.ToString());

						// if the is an answer check for a match and select the radiobutton
						foreach (KeyValuePair<int, string> answers in clientAnswerKeysAndValues)
						{                         
							if (questionnaireQuestionAnswer.Answer.Key == answers.Key)
							{
								li.Selected = true;
								break; // there should only be one answerkey for this answer type
							}                         
						}

						answerControl_RadioButtonList.Items.Add(li);
					}

					this.Controls.Add(answerControl_RadioButtonList);
					#endregion
					break;
				case (int)SAHL.Common.Globals.AnswerTypes.Comment:
					#region SAHLTextBox control 
					answerControl_TextBox = new SAHLTextBox();
					answerControl_TextBox.ReadOnly = _readOnly;
                    answerControl_TextBox.ID = this.ID + "_answerControlComment" + '_' + _questionnaireQuestionKey + '_' + controlNameAnswerTypeKey + '_' + controlNameAnswerKey;      
					answerControl_TextBox.TextMode = TextBoxMode.MultiLine;
					answerControl_TextBox.Height = new Unit(70, UnitType.Pixel);
					answerControl_TextBox.Width = new Unit(500, UnitType.Pixel);
					
					// populate answer if there is one
					foreach (KeyValuePair<int, string> answers in clientAnswerKeysAndValues)
					{                      
						answerControl_TextBox.Text = answers.Value;
						break; // there should only be one answer value for this answer type
					}

					this.Controls.Add(answerControl_TextBox);
					#endregion
					break;
				case (int)SAHL.Common.Globals.AnswerTypes.Date:
					#region SAHLDateBox control
					answerControl_DateBox = new SAHLDateBox();
					answerControl_DateBox.ReadOnly = _readOnly;
                    answerControl_DateBox.ID = this.ID + "_answerControlDate" + '_' + _questionnaireQuestionKey + '_' + controlNameAnswerTypeKey + '_' + controlNameAnswerKey;      

					this.Controls.Add(answerControl_DateBox);
					#endregion
					break;
				case (int)SAHL.Common.Globals.AnswerTypes.Numeric:
					#region SAHLTextBox control
					answerControl_TextBox = new SAHLTextBox();
					answerControl_TextBox.ReadOnly = _readOnly;
					answerControl_TextBox.DisplayInputType = InputType.Number;
                    answerControl_TextBox.ID = this.ID + "_answerControlNumeric" + '_' + _questionnaireQuestionKey + '_' + controlNameAnswerTypeKey + '_' + controlNameAnswerKey;      
					
					// populate answer if there is one
					foreach (KeyValuePair<int, string> answers in clientAnswerKeysAndValues)
					{                        
						answerControl_TextBox.Text = answers.Value;
						break; // there should only be one answer value for this answer type
					}

					this.Controls.Add(answerControl_TextBox);
					#endregion
					break;
				case (int)SAHL.Common.Globals.AnswerTypes.MultiSelect:
					#region SAHLCheckBoxList control
                    // create the multiselect control - one checkbox per row
                    answerControl_MultiSelect = new Table();

					foreach (IQuestionnaireQuestionAnswer questionnaireQuestionAnswer in _questionnaireQuestionAnswers)
					{
                        // create the verticle rows
                        TableRow multiselectTableRow = new TableRow();
                        multiselectTableRow.VerticalAlign = VerticalAlign.Middle;

                        string controlID = this.ID + "answerControl_MultiSelect" + '_' + questionnaireQuestionAnswer.QuestionnaireQuestion.Key + '_' + questionnaireQuestionAnswer.Answer.AnswerType.Key + "_" + questionnaireQuestionAnswer.Answer.Key;
                        
                        // create a html checkbox item
                        HtmlInputCheckBox htmlCheckBox = new HtmlInputCheckBox();
                        htmlCheckBox.Disabled = _readOnly;
                        htmlCheckBox.Name = controlID;
                        htmlCheckBox.ID = controlID;
                        
                        // add checkbox to cell
                        TableCell tc1 = new TableCell();
                        tc1.Style.Add("vertical-align", "middle");
                        tc1.Width = new Unit(10, UnitType.Pixel);
                        tc1.Controls.Add(htmlCheckBox);
                        
                        // add label to cell
                        TableCell tc2 = new TableCell();
                        tc2.Style.Add("vertical-align", "middle");
                        tc2.Text = questionnaireQuestionAnswer.Answer.Description;

                        // add cells to row
                        multiselectTableRow.Cells.Add(tc1);
                        multiselectTableRow.Cells.Add(tc2);
                      			
						// if there are answers check for a match and select the checkbox
						foreach (KeyValuePair<int, string> answers in clientAnswerKeysAndValues)
						{
							if (questionnaireQuestionAnswer.Answer.Key == answers.Key)
							{
                                htmlCheckBox.Checked = true;
							}
						}

                        // add row to table
                        answerControl_MultiSelect.Rows.Add(multiselectTableRow);
					}
					this.Controls.Add(answerControl_MultiSelect);
					#endregion
					break;
				case (int)SAHL.Common.Globals.AnswerTypes.Ranking:
					#region RadioButtonList Ranking control
					// create the ranking control - one radio button list per question
					answerControl_Ranking = new Table();

                    List<int> selectedKeys = new List<int>();
                    int answerIndex = 0;
                    for (int rankingRow = 0; rankingRow <= _questionnaireQuestionAnswers.Count; rankingRow++)
                    {
                        answerIndex = rankingRow - 1;

                        // create the verticle rows
                        TableRow rankingTableRow = new TableRow();
                        rankingTableRow.VerticalAlign = VerticalAlign.Middle;
                        
                        // create a radiobutton list per row
                        SAHLRadioButtonList rbnList = new SAHLRadioButtonList();

                        if (rankingRow >= 1)
                        {
                            rbnList.ID = this.ID + "_answerControlRankingList" + '_' + _questionnaireQuestionKey + '_' + controlNameAnswerTypeKey + "_" + _questionnaireQuestionAnswers[answerIndex].Answer.Key;
                            rbnList.RepeatDirection = RepeatDirection.Horizontal;
                            
                            // get the rank for _questionnaireQuestionAnswers[rankingRow]
                            int rank = 0;
                            foreach (KeyValuePair<int, string> answers in clientAnswerKeysAndValues)
                            {
                                if (answers.Key == _questionnaireQuestionAnswers[answerIndex].Answer.Key)
                                {
                                    rank = Convert.ToInt32(answers.Value);
                                    break;
                                }
                            }

                            // create each radiobutton and set selected if required
                            for (int rankingCol = _questionnaireQuestionAnswers.Count; rankingCol > 0; rankingCol--)
                            {
                                int colIndex = rankingCol - 1;
                                ListItem li = new ListItem("",rankingCol.ToString()); 

                                if (rank == rankingCol)
                                    li.Selected = true;

                                rbnList.Items.Add(li);
                            }

                        }

                        int colHeader = _questionnaireQuestionAnswers.Count;
                        for (int rankingCol = 0; rankingCol <= _questionnaireQuestionAnswers.Count; rankingCol++)
                        {
                            #region add columns to the table rows

                            TableCell rankingCell = new TableCell();
                            rankingCell.ColumnSpan = 1;

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
                                if (rankingCol == 0)  // 1st column - add row description
                                {
                                    rankingCell.Text = _questionnaireQuestionAnswers[answerIndex].Answer.Description;
                                }
                                else if (rankingCol == 1) // col > 0  - add radiobuttons
                                {
                                    rankingCell.ColumnSpan = _questionnaireQuestionAnswers.Count;

                                    rankingCell.Controls.Add(rbnList);
                                    rankingTableRow.Cells.Add(rankingCell);
                                    break;
                                }
                            }

                            rankingTableRow.Cells.Add(rankingCell);
                            #endregion
                        }

                        // add each ranking control row to the table
                        answerControl_Ranking.Rows.Add(rankingTableRow);
                    }

                    // add the table to the control
                    this.Controls.Add(answerControl_Ranking);

					#endregion
					break;
				default:
					answerControl_Label = new SAHLLabel();
					answerControl_Label.ID = this.ID + "_answerControlNone";
                    this.Controls.Add(answerControl_Label);
					break;
			}

			base.CreateChildControls();
		}

		private IDictionary<int,string> GetAnswerKeysAndValues(int answerTypeKey, IList<IClientAnswer> clientAnswers)
		{
			IDictionary<int, string> answerKeyAndValue = new Dictionary<int,string>();

			if (clientAnswers != null && clientAnswers.Count > 0)
			{
				foreach (IClientAnswer clientAnswer in clientAnswers)
				{
					int key = clientAnswer.Answer.Key;
					string val = clientAnswer.ClientAnswerValue!=null ? clientAnswer.ClientAnswerValue.Value : String.Empty;
					answerKeyAndValue.Add(key,val);
		 
				}
			}

			return answerKeyAndValue;
		}

		public override void RenderControl(HtmlTextWriter writer)
		{
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			switch (_answerTypeKey)
			{
				case (int)SAHL.Common.Globals.AnswerTypes.Alphanumeric:
				case (int)SAHL.Common.Globals.AnswerTypes.Comment:
				case (int)SAHL.Common.Globals.AnswerTypes.Numeric:
					answerControl_TextBox.RenderControl(writer);
					break;
				case (int)SAHL.Common.Globals.AnswerTypes.Boolean:
				case (int)SAHL.Common.Globals.AnswerTypes.Rating:
				case (int)SAHL.Common.Globals.AnswerTypes.SingleSelect:
					answerControl_RadioButtonList.RenderControl(writer);
					break;
				case (int)SAHL.Common.Globals.AnswerTypes.Date:
					answerControl_DateBox.RenderControl(writer);
					break;
				case (int)SAHL.Common.Globals.AnswerTypes.MultiSelect:
					answerControl_MultiSelect.RenderControl(writer);
					break;
				case (int)SAHL.Common.Globals.AnswerTypes.Ranking:
					answerControl_Ranking.RenderControl(writer);
					break;
				default:
					break;
			}
			
			
			writer.RenderEndTag();    // td
			writer.RenderEndTag();    // tr
			writer.RenderEndTag();    // table
		}

		#endregion
	}
}
