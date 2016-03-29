using SAHL.Core.SystemMessages;

namespace SAHL.Services.Capitec.Managers.DecisionTreeResult.Models
{
    public class CreditBureauAssessmentResult
    {
        public bool ITCPassed { get; set; }

        public ISystemMessageCollection ITCMessages { get; set; }
    }
}