using System;
using System.Web.UI.WebControls;
using SAHL.Internet.SAHL.Web.Services.Survey;
using System.Web.UI;
using System.Linq;

namespace SAHL.Internet.Components.Survey
{
    /// <summary>
    /// Represents a survey.
    /// </summary>
    public class SahlSurvey : Panel
    {
        /// <summary>
        /// This is the main property that allows us to build the whole control
        /// </summary>
        public ClientQuestionnaire Questionnaire
        {
            set;
            get;
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (Questionnaire == null || Questionnaire.QuestionnaireQuestions == null || Questionnaire.QuestionnaireQuestions.Length == 0) return;

            var table = new Table {ID = "tblSurvey", CssClass = "survey-table"};
            foreach (var question in Questionnaire.QuestionnaireQuestions)
            {
                var questionRow = new TableRow {CssClass = "survey-question-row"};
                questionRow.Cells.Add(new TableCell {CssClass = "survey-question-cell", Width = Unit.Pixel(20), Text = Convert.ToString(question.Sequence) + "."});
                questionRow.Cells.Add(new TableCell {CssClass = "survey-question-cell", Text = question.Description});

                var answerRow = new TableRow {CssClass = "survey-answer-row"};
                answerRow.Cells.Add(new TableCell {CssClass = "survey-blank-cell", Width = Unit.Pixel(20), Text = "&nbsp;"});
                var cell = new TableCell {CssClass = "survey-answer-cell"};
                cell.Controls.Add(new SahlSurveyAnswer(question));
                answerRow.Cells.Add(cell);

                table.Rows.Add(questionRow);
                table.Rows.Add(answerRow);
            }

            Controls.Add(table);
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content. </param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            var table = Controls.OfType<Table>().FirstOrDefault();
            if (Questionnaire == null || table == null || table.Rows.Count == 0) return;

            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            RenderContents(writer);
            writer.RenderEndTag();
        }
    }
}
