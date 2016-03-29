using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.Pages;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class DebtCounsellingAssignSupervisor : DebtCounsellingAssignSupervisorControls
    {
        private readonly IAssignmentService assignmentService;

        public DebtCounsellingAssignSupervisor()
        {
            assignmentService = ServiceLocator.Instance.GetService<IAssignmentService>();
        }

        public void AssignToUser(string userName, ButtonTypeEnum buttonLabel)
        {
            base.UserDropDown.Option(assignmentService.GetAdUsersForWorkflowRoleType(userName).Rows(0).Column("Description").Value).Select();
            ClickButton(buttonLabel);
        }

        public bool UserListContainsExpression(string expression)
        {
            Dictionary<string, string> users = new Dictionary<string, string>();
            foreach (Option option in base.UserDropDown.Options)
                if (option.Text.Contains(expression))
                    return true;
            return false;
        }

        public void ClickButton(ButtonTypeEnum buttonLabel)
        {
            switch (buttonLabel)
            {
                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;

                case ButtonTypeEnum.Submit:
                    base.SubmitButton.Click();
                    break;

                default:
                    break;
            }
        }
    }
}