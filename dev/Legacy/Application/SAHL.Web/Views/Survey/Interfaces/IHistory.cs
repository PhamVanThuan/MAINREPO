using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Survey.Interfaces
{
    public interface IHistory : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler ClientQuestionnaireSelected;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstClientQuestionnaires"></param>
        void BindSurveyGrid(IEventList<IClientQuestionnaire> lstClientQuestionnaires);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientQuestionnaire"></param>
        /// <param name="displaySurveyHeader"></param>
        /// <param name="readOnly"></param>
        /// <param name="height"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        void BindSurveyDetail(IClientQuestionnaire clientQuestionnaire, bool displaySurveyHeader, bool readOnly, int height);

    }
}
