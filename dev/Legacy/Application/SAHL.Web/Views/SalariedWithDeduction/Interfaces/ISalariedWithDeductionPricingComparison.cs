using SAHL.Common.Web.UI;
using SAHL.Web.Views.SalariedWithDeduction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Web.Views.SalariedWithDeduction.Interfaces
{
    public interface ISalariedWithDeductionPricingComparison : IViewBase
    {
        event EventHandler OnCancelButtonClicked;

        void DisplayCurrentApplicationPricingDetails(ApplicationPricingDetailModel currentPricing);

        void DisplayNewApplicationPricingDetails(ApplicationPricingDetailModel newPricing);
    }
}