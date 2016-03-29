using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AdminUserStatusMaintenanceControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblRoleType")]
        protected Span RoleTypeLabel { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button SubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_ddlRoleTypes")]
        protected SelectList RoleTypesDropDownList { get; set; }

        [FindBy(Id = "ctl00_Main_gvADUserStatusUpdate_DXMainTable")]
        protected Table UserStatusMaintenanceTable { get; set; }

        [FindBy(Id = "ctl00_Main_gvADUserStatusUpdate_DXEditor3_I")]
        protected TextField UserStatusDropDownList { get; set; }

        [FindBy(Id = "ctl00_Main_gvADUserStatusUpdate_DXEditor4_I")]
        protected TextField RoundRobinDropDownList { get; set; }

        [FindBy(Id = "ctl00_Main_gvADUserStatusUpdate_DXEditor3_DDD_L_LBI0T0")]
        protected TableCell ActiveUserStatusCell { get; set; }

        [FindBy(Id = "ctl00_Main_gvADUserStatusUpdate_DXEditor3_DDD_L_LBI1T0")]
        protected TableCell InactiveUserStatusCell { get; set; }

        [FindBy(Id = "ctl00_Main_gvADUserStatusUpdate_DXEditor4_DDD_L_LBI0T0")]
        protected TableCell ActiveRoundRobinCell { get; set; }

        [FindBy(Id = "ctl00_Main_gvADUserStatusUpdate_DXEditor4_DDD_L_LBI1T0")]
        protected TableCell InactiveRoundRobinCell { get; set; }

        [FindBy(Class = "dxWeb_pNext_SoftOrange")]
        protected Image NextButtonImage { get; set; }

        [FindBy(Class = "dxWeb_pPrev_SoftOrange")]
        protected Image PreviousButtonImage { get; set; }
    }
}