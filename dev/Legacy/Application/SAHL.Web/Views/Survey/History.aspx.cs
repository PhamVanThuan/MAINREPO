using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Survey.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.Web.UI.Events;
using System.Drawing;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Survey
{
    public partial class History : SAHLCommonBaseView, IHistory
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        #region IHistory Members
        
        public event KeyChangedEventHandler ClientQuestionnaireSelected;

        public void BindSurveyGrid(IEventList<IClientQuestionnaire> lstClientQuestionnaires)
        {
            grdClientQuestionnaire.Settings.ShowGroupPanel = false;
            grdClientQuestionnaire.Settings.ShowTitlePanel = true;
            grdClientQuestionnaire.PostBackType = GridPostBackType.SingleClick;
            grdClientQuestionnaire.AutoGenerateColumns = false;
            grdClientQuestionnaire.KeyFieldName = "GUID";

            grdClientQuestionnaire.SettingsPager.PageSize = 5;

            grdClientQuestionnaire.SettingsText.EmptyDataRow = "No Client Surveys";
            grdClientQuestionnaire.SettingsText.Title = "Client Survey History";

            //setup columns
            grdClientQuestionnaire.Columns.Clear();

            // key columns
            grdClientQuestionnaire.AddGridColumn("ClientQuestionnaireKey", "ClientQuestionnaireKey", 5, GridFormatType.GridNumber, "", HorizontalAlign.Left, false, true);
            grdClientQuestionnaire.AddGridColumn("QuestionnaireKey", "QuestionnaireKey", 5, GridFormatType.GridNumber, "", HorizontalAlign.Left, false, true);
            grdClientQuestionnaire.AddGridColumn("LegalEntityKey", "LegalEntityKey", 5, GridFormatType.GridNumber, "", HorizontalAlign.Left, false, true);

            // visible columns
            grdClientQuestionnaire.AddGridColumn("QuestionnaireDescription", "Questionnaire", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            grdClientQuestionnaire.AddGridColumn("BusinessEventDescription", "Client Event", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);         
            grdClientQuestionnaire.AddGridColumn("GenericKeyDescription", "Reference", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            grdClientQuestionnaire.AddGridDateColumn("DatePresented", "Contact Date", 15, HorizontalAlign.Left, true, true); 
            grdClientQuestionnaire.AddGridDateColumn("DateReceived", "Completed", 15, HorizontalAlign.Left, true, true);
            grdClientQuestionnaire.AddGridColumn("ClientSurveyStatus", "Status", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);

            // invisible columns
            grdClientQuestionnaire.AddGridColumn("ADUserKey", "Completed By", 20, GridFormatType.GridString, "", HorizontalAlign.Left, false, true);  
            grdClientQuestionnaire.AddGridColumn("LegalEntityName", "LegalEntity", 20, GridFormatType.GridString, "", HorizontalAlign.Left, false, true); 
            grdClientQuestionnaire.AddGridColumn("GenericKey", "GenericKey", 10, GridFormatType.GridString, "", HorizontalAlign.Left, false, true);
            grdClientQuestionnaire.AddGridColumn("GenericKeyTypeKey", "GenericKeyTypeKey", 10, GridFormatType.GridString, "", HorizontalAlign.Left, false, true);


            // loop through the ClientQuestionnaires and create a list of bindable objects
            List<SurveyBindableObject> lstSurveyBindableObject = new List<SurveyBindableObject>();

            foreach (IClientQuestionnaire clientQuestionnaire in lstClientQuestionnaires)
            {

                // set the implied client survey status
                string clientSurveyStatus = "";
                if (clientQuestionnaire.DateReceived.HasValue == false)
                    clientSurveyStatus = SAHL.Common.Globals.ClientSurveyStatus.Unanswered.ToString();
                else
                {
                    if (clientQuestionnaire.ClientAnswers != null && clientQuestionnaire.ClientAnswers.Count > 0)
                        clientSurveyStatus = SAHL.Common.Globals.ClientSurveyStatus.Answered.ToString();
                    else
                        clientSurveyStatus = SAHL.Common.Globals.ClientSurveyStatus.Rejected.ToString();
                }


                SurveyBindableObject bo = new SurveyBindableObject(clientQuestionnaire.BusinessEventQuestionnaire.Questionnaire.Key,
                    clientQuestionnaire.BusinessEventQuestionnaire.Questionnaire.Description,
                    clientQuestionnaire.BusinessEventQuestionnaire.BusinessEvent != null ? clientQuestionnaire.BusinessEventQuestionnaire.BusinessEvent.Key : 0,
                    clientQuestionnaire.BusinessEventQuestionnaire.BusinessEvent != null ? clientQuestionnaire.BusinessEventQuestionnaire.BusinessEvent.Description : "None",
                    clientQuestionnaire.Key,
                    clientQuestionnaire.BusinessEventQuestionnaire.Key,
                    clientQuestionnaire.DatePresented.ToString(SAHL.Common.Constants.DateTimeFormatWithSeconds),
                    clientQuestionnaire.ADUser != null ? clientQuestionnaire.ADUser.Key : 0,
                    clientQuestionnaire.GenericKey,
                    clientQuestionnaire.GenericKeyType.Key,
                    clientQuestionnaire.GenericKeyType.Description + " - " + clientQuestionnaire.GenericKey,
                    clientQuestionnaire.DateReceived.HasValue ? clientQuestionnaire.DateReceived.Value.ToString(SAHL.Common.Constants.DateTimeFormatWithSeconds) : "",
                    clientQuestionnaire.LegalEntity != null ? clientQuestionnaire.LegalEntity.Key : 0,
                    clientQuestionnaire.LegalEntity != null ? clientQuestionnaire.LegalEntity.GetLegalName(LegalNameFormat.Full) : "",
                    clientSurveyStatus,
                    clientQuestionnaire.GUID);

                lstSurveyBindableObject.Add(bo);
            }

            grdClientQuestionnaire.DataSource = lstSurveyBindableObject;
            grdClientQuestionnaire.DataBind();
        }

        public void BindSurveyDetail(IClientQuestionnaire clientQuestionnaire, bool displaySurveyHeader, bool readOnly, int height)
        {
            surveyControl.ClientQuestionnaire = clientQuestionnaire;
            surveyControl.DisplaySurveyHeader = displaySurveyHeader;
            surveyControl.ReadOnly = readOnly;
            surveyControl.ControlHeight = height;
        }
        #endregion

        protected void grdClientQuestionnaire_SelectionChanged(object sender, EventArgs e)
        {
            if (grdClientQuestionnaire.SelectedKeyValue != null)
            {
                KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(grdClientQuestionnaire.SelectedKeyValue);

                ClientQuestionnaireSelected(sender, keyChangedEventArgs);
            }
        }
    }
}