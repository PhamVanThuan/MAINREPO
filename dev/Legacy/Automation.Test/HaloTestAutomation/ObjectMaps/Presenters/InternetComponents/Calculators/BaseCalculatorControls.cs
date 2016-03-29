using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.InternetComponents
{
    public abstract class BaseCalculatorControls : BasePageControls
    {
        [FindBy(Class = "calculate-button")]
        public Button Calculate { get; set; }

        [FindBy(Id = "dnn_ctr838_XSSmartModule_ctl00_lblSummaryReferenceNumber")]
        public Span ReferenceNumber { get; set; }
    }
}