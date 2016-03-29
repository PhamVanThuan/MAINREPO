using BuildingBlocks.Timers;

using System.Linq;

namespace BuildingBlocks.Presenters.Admin
{
    public sealed class EmployerUpdate : EmployerBase
    {
        public override void Populate(Automation.DataModels.Employer employer)
        {
            base.EmployerName.TypeText(employer.Name);
            this.SelectEmployerIfExist(employer.Name);
            base.Populate(employer);
        }

        public void ClickUpdate()
        {
            base.UpdateButton.Click();
        }

        #region Helpers

        private void SelectEmployerIfExist(string name)
        {
            GeneralTimer.Wait(2000);
            var autoCompleteDev = (from div in base.SAHLAutoComplete_DefaultItem_Collection()
                                   where div.Text.Equals(name)
                                   select div).FirstOrDefault();
            if (autoCompleteDev != null)
            {
                autoCompleteDev.MouseDown();
                GeneralTimer.Wait(2000);
            }
        }

        #endregion Helpers
    }
}