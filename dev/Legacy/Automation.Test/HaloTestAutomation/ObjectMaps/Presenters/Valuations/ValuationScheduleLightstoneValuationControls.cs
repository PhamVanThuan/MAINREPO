using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ValuationScheduleLightstoneValuationControls : BasePageControls
    {
        [FindBy(Name = "ctl00$Main$txtContact1Name")]
        public TextField Contact1 { get; set; }

        [FindBy(Name = "ctl00$Main$txtContact1Phone")]
        public TextField Phone01 { get; set; }

        [FindBy(Name = "ctl00$Main$txtContact2Name")]
        public TextField Contact2 { get; set; }

        [FindBy(Name = "ctl00$Main$txtContact2Phone")]
        public TextField Phone02 { get; set; }

        [FindBy(Name = "ctl00$Main$txtContact1WorkPhone")]
        public TextField WorkPhone1 { get; set; }

        [FindBy(Name = "ctl00$Main$txtContact1MobilePhone")]
        public TextField MobilePhone1 { get; set; }

        [FindBy(Name = "ctl00$Main$dtAssessmentByDateValue")]
        public TextField AssessmentDate { get; set; }

        [FindBy(Name = "ctl00$Main$ddlValuationReasons")]
        public SelectList AssessmentReasons { get; set; }

        [FindBy(Id = "ctl00_Main_btnInstruct")]
        public Button InstructValuer { get; set; }
    }
}