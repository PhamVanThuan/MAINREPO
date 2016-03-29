using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Web.Views.FurtherLending.Interfaces
{
    public interface ICalculatorPreCheck : IViewBase
    {
        event EventHandler OnNextButtonClicked;

        event EventHandler OnCancelButtonClicked;

        void BindDisplay(IAccount Account, IList<IFinancialAdjustment> FinancialAdjustments);

        bool NextButtonVisible { set; }
    }
}
