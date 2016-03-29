namespace Automation.DataModels
{
    public class Branch
    {
        //Branch
        public string ACBBranchCode { get; set; }

        public int ACBBankCode { get; set; }

        public Bank Bank { get; set; }

        public string ACBBranchDescription { get; set; }

        public string ActiveIndicator { get; set; }
    }
}