using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ManualValuationsMainDwellingDetailsControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_BackButton")]
        protected Button BackButton { get; set; }

        [FindBy(Id = "ctl00_Main_lblMainBuildingReplacementValue")]
        protected Span MainBuildingReplacementValue { get; set; }

        [FindBy(Id = "ctl00_Main_lblTotalConventionalValue")]
        protected Span TotalConventionalValue { get; set; }

        [FindBy(Id = "ctl00_Main_lblTotalThatchValue")]
        protected Span TotalThatchValue { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button Next { get; set; }
    }
}