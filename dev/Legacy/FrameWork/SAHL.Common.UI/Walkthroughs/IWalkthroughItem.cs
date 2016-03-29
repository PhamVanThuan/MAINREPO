using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.UI.Walkthroughs
{
    public interface IWalkthroughItem
    {
        string Caption { get;set; }
        string Title { get;set; }
        string Url { get;set; }
        string Image { get;set; }
        string HotImage { get;set; }
        string DisabledImage { get;set; }
        bool Visible { get;set; }
        bool Enabled { get;set; }
    }
}
