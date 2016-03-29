using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IPersonalLoanApplicationDeclaration : IViewBase
    {
        void ConfigurePanel(string legalEntityName);

        void BindDeclaration(IList<IApplicationDeclarationQuestionAnswerConfiguration> appDecQandAConfig, IEventList<IExternalRoleDeclaration> appDecs);

        IEventList<IExternalRoleDeclaration> GetExternalRoleDeclarations { get; }

        bool ShowCancelButton { set; }

        bool ShowBackButton { set; }

        bool ShowUpdateButton { set; }

        bool UpdateMode { set; }

        string UpdateButtonText { get; set; }

        event EventHandler onCancelButtonClicked;

        event EventHandler onUpdateButtonClicked;

        event EventHandler onBackButtonClicked;

        void SetViewForNullDeclarations();
    }
}