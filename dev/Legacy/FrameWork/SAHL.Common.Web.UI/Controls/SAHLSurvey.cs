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
using System.Drawing;

namespace SAHL.Common.Web.UI.Controls
{
    public class SAHLSurvey :Panel
    {
        #region private variables
        private bool _readOnly, _displaySurveyHeader;
        private int _controlHeight;
        private ISurveyRepository _surveyRepo;
        private IClientQuestionnaire _clientQuestionnaire;
        private Table _questionnaireTable, _questionnaireHeadingTable;
        private SAHLPanel _surveyPanel;
        #endregion

        #region Properties

        /// <summary>
        /// This is the main property that allows us to build the whole control
        /// </summary>
        public IClientQuestionnaire ClientQuestionnaire
        {
            set { _clientQuestionnaire = value;}
            get { return _clientQuestionnaire; }
        }

        /// <summary>
        /// To set whether answers can be changed or not
        /// </summary>
        public bool ReadOnly
        {
            set { _readOnly = value; }
            get { return _readOnly; }
        }

        /// <summary>
        /// Display Survey Header containing Survey Description and LegalEntity
        /// </summary>
        public bool DisplaySurveyHeader
        {
            set { _displaySurveyHeader = value; }
            get { return _displaySurveyHeader; }
        }

        /// <summary>
        /// Height of the control in pixels
        /// </summary>
        public int ControlHeight
        {
            set { _controlHeight = value; }
            get { return _controlHeight; }
        }
 
        #endregion

        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // setup panel
            _surveyPanel = new SAHLPanel();
            _surveyPanel.ID = "pnlSurvey";
            _surveyPanel.Attributes.Add("runat", "Server");
            _surveyPanel.CssClass = "surveyPanel";

            // setup tables
            _questionnaireTable = new Table();
            _questionnaireTable.ID = "tblSurvey";
            _questionnaireTable.Attributes.Add("runat", "Server");
            _questionnaireTable.CssClass = "surveyTable";
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
            if (_clientQuestionnaire != null)
                CreateControlHierarchy();

            //ClearChildViewState();
        }

        protected virtual void CreateControlHierarchy()
        {
            TableRow tableRow;
            TableCell tableCell;

            if (_surveyRepo == null)
                _surveyRepo = RepositoryFactory.GetRepository<ISurveyRepository>();

            if (_displaySurveyHeader)
            {
                #region add questionnaire header table
                _questionnaireHeadingTable = new Table();
                _questionnaireHeadingTable.ID = "tblSurveyHeading";
                _questionnaireHeadingTable.CssClass = "surveyHeadingTable";

                tableRow = new TableRow();
                tableRow.CssClass = "surveyHeadingRow";

                // column 1
                tableCell = new TableCell();
                tableCell.CssClass = "surveyHeadingColLeft";        
                tableCell.Text = "Questionnaire : " + ClientQuestionnaire.BusinessEventQuestionnaire.Questionnaire.Description;
                tableRow.Cells.Add(tableCell);

                // column 2
                tableCell = new TableCell();
                tableCell.CssClass = "surveyHeadingColRight";
                tableCell.Text = ClientQuestionnaire.LegalEntity !=null ? "Legal Entity : " + ClientQuestionnaire.LegalEntity.GetLegalName(LegalNameFormat.Full) : "";
                tableRow.Cells.Add(tableCell);

                // add the header row to the header table
                _questionnaireHeadingTable.Rows.Add(tableRow);

                // add the header table to the control
                this.Controls.Add(_questionnaireHeadingTable);

                #endregion
            }

            // AddBlankTableRow(_questionnaireTable);

            // add questionnaire detail row(s)

            int controlCount = 0;
            foreach (IQuestionnaireQuestion questionnaireQuestion in _clientQuestionnaire.BusinessEventQuestionnaire.Questionnaire.Questions)
            {
                controlCount++;

                // get the Client Answer collection
                IList<IClientAnswer> clientAnswers = _surveyRepo.GetClientAnswersForQuestion(_clientQuestionnaire, questionnaireQuestion);

                int questionSequence = questionnaireQuestion.Sequence;
                string questionText = questionnaireQuestion.Question.Description;

                #region add the question row

                tableRow = new TableRow();
                tableRow.CssClass = "surveyQuestionRow";

                tableCell = new TableCell();
                tableCell.CssClass = "surveyQuestionCell";
                tableCell.Width = new Unit(20, UnitType.Pixel);
                tableCell.Text = Convert.ToString(questionSequence) + ".";
                tableRow.Cells.Add(tableCell);

                tableCell = new TableCell();
                tableCell.CssClass = "surveyQuestionCell";
                tableCell.Text = questionText;
                tableRow.Cells.Add(tableCell);

                _questionnaireTable.Rows.Add(tableRow);

                #endregion

                #region add answer control row

                SAHLSurveyAnswer answerControl = new SAHLSurveyAnswer(questionnaireQuestion, clientAnswers, _readOnly);

                tableRow = new TableRow();
                tableRow.CssClass = "surveyAnswerRow";

                // blank cell
                tableCell = new TableCell();
                tableCell.CssClass = "surveyBlankCell";
                tableCell.Width = new Unit(20, UnitType.Pixel);
                tableCell.Text = "";
                tableRow.Cells.Add(tableCell);

                // control cell
                tableCell = new TableCell();
                tableCell.CssClass = "surveyAnswerCell";
                tableCell.Controls.Add(answerControl);
                tableRow.Cells.Add(tableCell);

                _questionnaireTable.Rows.Add(tableRow);

                #endregion

            }


            // add the table to the panel
            if (_controlHeight > 0)
                _surveyPanel.Height = _controlHeight;

            _surveyPanel.Controls.Add(_questionnaireTable);

            // add the panel to the control
            this.Controls.Add(_surveyPanel);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (_clientQuestionnaire != null && _questionnaireTable.Rows.Count > 0)
            {
                AddAttributesToRender(writer);

                if (_displaySurveyHeader)
                {
                    _questionnaireHeadingTable.RenderControl(writer);
                }

                writer.AddStyleAttribute("Width", "100%");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);          
               
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                _surveyPanel.RenderControl(writer);

                writer.RenderEndTag();    // td
                writer.RenderEndTag();    // tr

                writer.RenderEndTag();    // table
            }
        }

        /// <summary>
        /// helper method to add a blank spacer row in tbale
        /// </summary>
        /// <param name="table"></param>
        private void AddBlankTableRow(Table table)
        {
            TableRow rowBlank = new TableRow();
            rowBlank.CssClass = "surveyBlankRow";
            TableCell cellBlank = new TableCell();
            cellBlank.CssClass = "surveyBlankCell";
            rowBlank.Cells.Add(cellBlank);
            table.Rows.Add(rowBlank);
        }

        #endregion
    }


}
