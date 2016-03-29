using ObjectMaps.InternetComponents;
using WatiN.Core;

namespace BuildingBlocks.Presenters.InternetComponents
{
    public class ClientSurvey : ClientSurveyControls
    {
        public void DefaultPopulate(string comments)
        {
            foreach (RadioButton radioBtn in base.SurveyAnswerTable.RadioButtons)
                radioBtn.Checked = true;
            base.Comments.Value = comments;
        }

        public void Submit()
        {
            base.Submit.Click();
        }
    }
}