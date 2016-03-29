using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.PersonalLoans
{
    public abstract class PersonalLoanCampaignExtractControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnImport")]
        protected Button ImportButton { get; set; }

        [FindBy(Name = "ctl00$Main$FileName")]
        protected FileUpload BrowseButton { get; set; }

        [FindBy(Id = "ctl00_Main_FileNameRow")]
        protected TableRow FileNameLabel { get; set; }

        [FindBy(Id = "ctl00_Main_FileName")]
        protected TextField FileToUpload { get; set; }

    }
}