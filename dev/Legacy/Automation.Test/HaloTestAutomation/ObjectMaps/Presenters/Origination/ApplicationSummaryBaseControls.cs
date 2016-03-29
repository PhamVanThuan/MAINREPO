using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApplicationSummaryBaseControls : BasePageControls
    {
        public Span spanApplicationNumber
        {
            get
            {
                return base.Document.Span(new Regex("^[\x20-\x7E]*_lblApplicationNumber$"));
            }
        }
    }
}