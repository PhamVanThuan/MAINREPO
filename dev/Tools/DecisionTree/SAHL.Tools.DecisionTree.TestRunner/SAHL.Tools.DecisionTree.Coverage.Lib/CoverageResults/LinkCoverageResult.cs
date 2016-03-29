namespace SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResults
{
    public class LinkCoverageResult
    {
        public string LinkType { get; set; }

        public string ToNodeName { get; set; }

        public bool Covered { get; set; }

        public LinkCoverageResult(string linkType, string toNodeName, bool covered)
        {
            this.LinkType = linkType;
            this.ToNodeName = toNodeName;
            this.Covered = covered;
        }
    }
}