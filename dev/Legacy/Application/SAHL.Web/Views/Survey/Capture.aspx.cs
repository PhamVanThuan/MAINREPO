using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Survey.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Text;

namespace SAHL.Web.Views.Survey
{
    public partial class Capture : SAHLCommonBaseView, ICapture
    {
        private bool _doSurveyButtonVisible, _saveSurveyButtonVisible, _cancelSurveyButtonVisible, _legalEntityEnabled;
        private int _selectedLegalEntityKey;
        private int _selectedClientQuestionnaireKey;
        private string _selectedClientQuestionnaireGUID;
        private int _selectedQuestionnaireKey;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage)
                return;

            btnDoSurvey.Visible = _doSurveyButtonVisible;
            //send client email works under the same conditions as the do survey....
            btnSendSurvey.Visible = _doSurveyButtonVisible;

            btnSaveSurvey.Visible = _saveSurveyButtonVisible;
            btnCancelSurvey.Visible = _cancelSurveyButtonVisible;

            ddlLegalEntity.Enabled = _legalEntityEnabled;
        }

        /// <summary>
        /// When this button is clicked - we must load up the selected survey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDoSurvey_Click(object sender, EventArgs e)
        {
            // get the selected legalentity
            if (String.Compare(ddlLegalEntity.SelectedValue, "-select-", true) == 0)
                _selectedLegalEntityKey = 0;
            else
                _selectedLegalEntityKey = Convert.ToInt32(ddlLegalEntity.SelectedItem.Value);

            // get the selected survey
            _selectedQuestionnaireKey = 0;
            _selectedClientQuestionnaireKey = 0;

            List<object> keyValues = grdSurvey.GetSelectedFieldValues("QuestionnaireKey", "ClientQuestionnaireKey");
            if (keyValues.Count > 0)
            {
                object[] selectedKeys = keyValues[0] as object[];

                _selectedQuestionnaireKey = selectedKeys == null ? 0 : Convert.ToInt32(selectedKeys[0]);
                _selectedClientQuestionnaireKey = selectedKeys == null ? 0 : (selectedKeys[1] != null ? Convert.ToInt32(selectedKeys[1]) : 0);
            }

            _selectedClientQuestionnaireGUID = String.IsNullOrEmpty(grdSurvey.SelectedKeyValue) ? "" : grdSurvey.SelectedKeyValue;

            DoSurveyButtonClicked(sender, e);
        }

        /// <summary>
        /// When this button is clicked - we must send the selected client an email request
        /// so that they can capture the survey at their leisure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSendSurvey_Click(object sender, EventArgs e)
        {
            // get the selected legalentity
            if (String.Compare(ddlLegalEntity.SelectedValue, "-select-", true) == 0)
                _selectedLegalEntityKey = 0;
            else
                _selectedLegalEntityKey = Convert.ToInt32(ddlLegalEntity.SelectedItem.Value);

            // get the selected survey
            _selectedQuestionnaireKey = 0;
            _selectedClientQuestionnaireKey = 0;

            List<object> keyValues = grdSurvey.GetSelectedFieldValues("QuestionnaireKey", "ClientQuestionnaireKey");
            if (keyValues.Count > 0)
            {
                object[] selectedKeys = keyValues[0] as object[];

                _selectedQuestionnaireKey = selectedKeys == null ? 0 : Convert.ToInt32(selectedKeys[0]);
                _selectedClientQuestionnaireKey = selectedKeys == null ? 0 : (selectedKeys[1] != null ? Convert.ToInt32(selectedKeys[1]) : 0);
            }

            _selectedClientQuestionnaireGUID = String.IsNullOrEmpty(grdSurvey.SelectedKeyValue) ? "" : grdSurvey.SelectedKeyValue;

            SendSurveyButtonClicked(sender, e);
        }

        protected void btnSaveSurvey_Click(object sender, EventArgs e)
        {
            IList<SurveyQuestionAnswer> surveyQuestionAnswers = new List<SurveyQuestionAnswer>();

            string[] allControlKeys = Page.Request.Form.AllKeys;
            foreach (var controlKey in allControlKeys)
            {
                if (controlKey.Contains("SAHLSurveyAnswer"))
                {
                    string[] controlValues = Page.Request.Form.GetValues(controlKey);
                    string controlValue = controlValues[0];

                    // split the QuestionaireQuestionKey, AnswerTypKey & AnswerKey out of the controlName
                    IList<string> nameParts = controlKey.Split('_').ToList<string>();
                    int questionaireQuestionKey = Convert.ToInt32(nameParts[nameParts.Count - 3]);
                    int answerTypeKey = Convert.ToInt32(nameParts[nameParts.Count - 2]);
                    int answerKey = Convert.ToInt32(nameParts[nameParts.Count - 1]);
                    string answerValue = "";

                    if (answerKey == 0)
                    {
                        // use the value attribute which contains the value
                        answerKey = Convert.ToInt32(controlValue);
                        answerValue = "";
                    }
                    else // textboxes, ranking list, MultiSelect
                    {
                        if (answerTypeKey != (int)SAHL.Common.Globals.AnswerTypes.MultiSelect)
                            answerValue = controlValue;
                    }

                    SurveyQuestionAnswer questionAnswer = new SurveyQuestionAnswer();
                    questionAnswer.QuestionnaireQuestionKey = questionaireQuestionKey;
                    questionAnswer.AnswerTypeKey = answerTypeKey;
                    questionAnswer.AnswerKey = answerKey;
                    questionAnswer.AnswerValue = answerValue;

                    surveyQuestionAnswers.Add(questionAnswer);
                }
            }

            KeyChangedEventArgs args = new KeyChangedEventArgs(surveyQuestionAnswers);

            SaveSurveyButtonClicked(sender, args);
        }

        protected void btnCancelSurvey_Click(object sender, EventArgs e)
        {
            CancelSurveyButtonClicked(sender, e);
        }

        #region IHistory Members

        public event EventHandler DoSurveyButtonClicked;

        public event EventHandler SendSurveyButtonClicked;

        public event KeyChangedEventHandler SaveSurveyButtonClicked;

        public event EventHandler CancelSurveyButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public bool DoSurveyButtonVisible
        {
            set
            {
                _doSurveyButtonVisible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SaveSurveyButtonVisible
        {
            set
            {
                _saveSurveyButtonVisible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CancelSurveyButtonVisible
        {
            set
            {
                _cancelSurveyButtonVisible = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool LegalEntityEnabled
        {
            set
            {
                _legalEntityEnabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedLegalEntityKey
        {
            set
            {
                _selectedLegalEntityKey = value;
            }
            get
            {
                return _selectedLegalEntityKey;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //public IList<SurveyQuestionAnswers> SelectedSurveyQuestionAnswers
        //{
        //    set
        //    {
        //        _selectedSurveyQuestionAnswers = value;
        //    }
        //    get
        //    {
        //        return _selectedSurveyQuestionAnswers;
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        public int SelectedQuestionnaireKey
        {
            set
            {
                _selectedQuestionnaireKey = value;
            }
            get
            {
                return _selectedQuestionnaireKey;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedClientQuestionnaireKey
        {
            set
            {
                _selectedClientQuestionnaireKey = value;
            }
            get
            {
                return _selectedClientQuestionnaireKey;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedClientQuestionnaireGUID
        {
            set
            {
                _selectedClientQuestionnaireGUID = value;
            }
            get
            {
                return _selectedClientQuestionnaireGUID;
            }
        }

        public void BindLegalEntityDropdownList(IReadOnlyEventList<ILegalEntity> legalEntityList)
        {
            var legalentityDict = new Dictionary<int, string>();

            foreach (ILegalEntity le in legalEntityList)
            {
                string display = le.GetLegalName(LegalNameFormat.Full) + " (" + (String.IsNullOrEmpty(le.EmailAddress) ? "no email" : le.EmailAddress) + ")";
                legalentityDict.Add(le.Key, display);
            }

            ddlLegalEntity.DataSource = legalentityDict;
            ddlLegalEntity.DataTextField = "Value";
            ddlLegalEntity.DataValueField = "Key";
            ddlLegalEntity.DataBind();
        }

        public void BindSurveyGrid(List<SurveyBindableObject> surveys)
        {
            //setup grid
            grdSurvey.Columns.Clear();

            grdSurvey.SettingsPager.PageSize = 5;
            grdSurvey.SettingsText.EmptyDataRow = "No Client Surveys";
            grdSurvey.SettingsText.Title = "Available Client Surveys";

            grdSurvey.Settings.ShowGroupPanel = false;
            grdSurvey.Settings.ShowTitlePanel = true;
            grdSurvey.PostBackType = GridPostBackType.NoneWithClientSelect;
            grdSurvey.AutoGenerateColumns = false;
            grdSurvey.KeyFieldName = "GUID";


            //setup columns
            grdSurvey.Columns.Clear();

            // key columns
            grdSurvey.AddGridColumn("ClientQuestionnaireKey", "ClientQuestionnaireKey", 5, GridFormatType.GridNumber, "", HorizontalAlign.Left, false, true);
            grdSurvey.AddGridColumn("QuestionnaireKey", "QuestionnaireKey", 5, GridFormatType.GridNumber, "", HorizontalAlign.Left, false, true);
            grdSurvey.AddGridColumn("LegalEntityKey", "LegalEntityKey", 5, GridFormatType.GridNumber, "", HorizontalAlign.Left, false, true);

            // visible columns
            grdSurvey.AddGridColumn("QuestionnaireDescription", "Questionnaire", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            grdSurvey.AddGridColumn("BusinessEventDescription", "Client Event", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            //grdSurvey.AddGridColumn("ADUser", "Completed By", 20, GridFormatType.GridString, "", HorizontalAlign.Left, false, true);
            grdSurvey.AddGridColumn("GenericKeyDescription", "Reference", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            grdSurvey.AddGridColumn("DatePresented", "Contact Date", 10, GridFormatType.GridDateTime, "dd/MM/yyyy HH:mm:ss", HorizontalAlign.Left, true, true);

            //grdSurvey.AddGridDateColumn("DateReceived", "Completed", 10, HorizontalAlign.Left, true, true);
            grdSurvey.AddGridColumn("ClientSurveyStatus", "Status", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);

            // invisible columns
            grdSurvey.AddGridColumn("LegalEntityName", "LegalEntity", 20, GridFormatType.GridString, "", HorizontalAlign.Left, false, true);
            grdSurvey.AddGridColumn("GenericKey", "GenericKey", 10, GridFormatType.GridString, "", HorizontalAlign.Left, false, true);
            grdSurvey.AddGridColumn("GenericKeyTypeKey", "GenericKeyTypeKey", 10, GridFormatType.GridString, "", HorizontalAlign.Left, false, true);


            grdSurvey.DataSource = surveys;
            grdSurvey.DataBind();
        }

        public void BindSurveyDetail(IClientQuestionnaire clientQuestionnaire, bool displaySurveyHeader, bool readOnly)
        {
            surveyControl.ClientQuestionnaire = clientQuestionnaire;
            surveyControl.DisplaySurveyHeader = displaySurveyHeader;
            surveyControl.ReadOnly = readOnly;
            if (displaySurveyHeader)
                surveyControl.ControlHeight = 265;
            else
                surveyControl.ControlHeight = 285;
        }
        #endregion
    }
}