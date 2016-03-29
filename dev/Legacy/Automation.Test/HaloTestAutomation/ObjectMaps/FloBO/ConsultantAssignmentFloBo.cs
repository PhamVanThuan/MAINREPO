using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloboControls
{
    public abstract class LoanNodeConsultantAssignment : BaseNavigation
    {
        [FindBy(TitleRegex = @"^[\x20-\x7E]* \(Stage\: Consultant Assignment\)$")]
        protected Link LoanNode { get; set; }

        [FindBy(Title = "AssignConsultant")]
        protected Link AssignConsultant { get; set; }
    }
}