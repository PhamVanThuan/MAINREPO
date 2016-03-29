using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Survey.Interfaces
{
    public interface ICapture : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler DoSurveyButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler SendSurveyButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler SaveSurveyButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler CancelSurveyButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        bool DoSurveyButtonVisible { set; }

        /// <summary>
        /// 
        /// </summary>
        bool SaveSurveyButtonVisible { set; }



		/// <summary>
		/// 
		/// </summary>
        bool CancelSurveyButtonVisible { set; }

        /// <summary>
        /// 
        /// </summary>
        bool LegalEntityEnabled { set; }

        /// <summary>
        /// 
        /// </summary>
        int SelectedQuestionnaireKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int SelectedClientQuestionnaireKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string SelectedClientQuestionnaireGUID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int SelectedLegalEntityKey { get;  set; }

        /// <summary>
        /// Bind the legal entity drop down list
        /// </summary>
        /// <param name="legalEntityList"></param>
        void BindLegalEntityDropdownList(IReadOnlyEventList<ILegalEntity> legalEntityList);


        /// <summary>
        /// Bind the survey Grid
        /// </summary>
        /// <param name="surveys"></param>
        void BindSurveyGrid(List<SurveyBindableObject> surveys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientQuestionnaire"></param>
        /// <param name="displaySurveyHeader"></param>
        /// <param name="readOnly"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        void BindSurveyDetail(IClientQuestionnaire clientQuestionnaire, bool displaySurveyHeader, bool readOnly);

    }
}
