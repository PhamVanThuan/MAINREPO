using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class EmployerDetailsControls : BasePageControls
    {
        [FindBy(IdRegex = "ctl00_Main_employerDetails_txtEmployerName")]
        protected TextField EmployerName { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_ddlBusinessType")]
        protected SelectList BusinessType { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_ddlEmploymentSector")]
        protected SelectList EmploymentSector { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_txtContactPerson")]
        protected TextField ContactPerson { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_phnContactPersonPhone__CODE")]
        protected TextField ContactPersonPhoneCode { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_phnContactPersonPhone__NUMB")]
        protected TextField ContactPersonPhoneNumber { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_txtContactEmail")]
        protected TextField ContactEmail { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_txtAccountant")]
        protected TextField Accountant { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_txtAccountantContact")]
        protected TextField AccountantContact { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_phnAccountantNumber__CODE")]
        protected TextField AccountantNumber__CODE { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_phnAccountantNumber__NUMB")]
        protected TextField AccountantNumber__NUMB { get; set; }

        [FindBy(IdRegex = "ctl00_Main_employerDetails_txtAccountantEmail")]
        protected TextField AccountantEmail { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button UpdateButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button AddButton { get; set; }
    }
}