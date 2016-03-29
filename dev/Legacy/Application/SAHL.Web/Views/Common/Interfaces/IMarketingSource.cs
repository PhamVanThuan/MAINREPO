using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IMarketingSource : IViewBase
    {
        void BindMarketingSourcesGrid(object marketRates);
    }
}
