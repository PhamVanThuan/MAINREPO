using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class Correspondence_LoanStatementControls : CorrespondenceProcessingControls
    {
        [FindBy(Id = "ctl00_Main_FromDate")]
        protected TextField FromDate { get; set; }

        [FindBy(Id = "ctl00_Main_ToDate")]
        protected TextField ToDate { get; set; }

        [FindBy(Id = "ctl00_Main_TransactionType")]
        protected SelectList TransactionTypes { get; set; }

        [FindBy(Id = "ctl00_Main_Format")]
        protected SelectList Formats { get; set; }

        [FindBy(Id = "ctl00_Main_Language")]
        protected SelectList Language { get; set; }
    }
}