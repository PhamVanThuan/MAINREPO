using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IPersonalLoanMaintainLifePolicy : IViewBase
    {
        event EventHandler OnCancelButtonClicked;

        event EventHandler OnSubmitButtonClicked;

        event KeyChangedEventHandler OnPolicyStatusSelectedIndexChanged;

        void ResetCloseDate();

        void ResetCeded();

        void BindInsurers(IDictionary<string, string> insurers);

        void BindStatus(IDictionary<string, string> lifeStatuses);

        void BindMaintainLifePolicyForReadOnly(IExternalLifePolicy externalLifePolicy);

        void BindMaintainLifePolicyForReadWrite(IExternalLifePolicy externalLifePolicy);

        void SetupSummaryView();

        string Insurer { get; }

        string PolicyNumber { get; }

        DateTime? CommencementDate { get; }

        string Status { get; }

        DateTime? CloseDate { get; }

        double SumInsured { get; }

        bool PolicyCeded { get; }
    }
}