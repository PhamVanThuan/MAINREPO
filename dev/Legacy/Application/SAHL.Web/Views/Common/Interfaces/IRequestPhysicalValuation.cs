using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IRequestPhysicalValuation : IViewBase
    {
        void BindReasons(Dictionary<int, string> reasonDefinitions);
        void BindValuations(DataTable valuationsData);
        void BindPropertyAccessDetails(IPropertyAccessDetails propertyAccessDetails);
        void DisableSubmitButton();

        event EventHandler SubmitClicked;
        event EventHandler CancelClicked;
        
        string Contact1Name { get; }
        string Contact1Phone { get; }
        string Contact1WorkPhone { get; }
        string Contact1MobilePhone { get; }
        string Contact2Name { get; }
        string Contact2Phone { get; }
        DateTime? AssessmentDate { get; }
        int SelectedValuationReasonDefinitionKey { get; }
        string SelectedValuationReasonDescription { get; }
        string SpecialInstructions { get; }
    }
}
