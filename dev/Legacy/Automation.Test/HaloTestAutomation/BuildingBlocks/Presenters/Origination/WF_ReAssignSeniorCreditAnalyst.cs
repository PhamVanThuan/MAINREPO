using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class WF_ReAssignSeniorCreditAnalyst : WF_ReAssignSeniorCreditAnalystControls
    {
        public enum btn
        {
            None = 0,
            Submit,
            Cancel
        }

        /// <summary>
        /// Selects a user to reassign the case to and submits the change
        /// </summary>
        /// <param name="reassignToUser">New User</param>
        /// <param name="button">Button to Click</param>
        public void SelectConsultantFromDropdownAndCommit(string reassignToUser, btn button)
        {
            base.ddlConsultant.Option(reassignToUser).Select();

            switch (button)
            {
                case btn.Submit:
                    base.btnSubmit.Click();
                    break;

                case btn.Cancel:
                    base.btnCancel.Click();
                    break;
            }
        }
    }
}