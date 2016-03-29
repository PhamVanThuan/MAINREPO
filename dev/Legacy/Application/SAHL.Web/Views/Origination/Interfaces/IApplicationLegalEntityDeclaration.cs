using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Origination.Interfaces
{
    public interface IApplicationLegalEntityDeclaration :IViewBase
    {
        void ConfigurePanel(string legalEntityName);

        void BindDeclaration(IList<IApplicationDeclarationQuestionAnswerConfiguration> appDecQandAConfig, IEventList<IApplicationDeclaration> appDecs);

        IEventList<IApplicationDeclaration> GetOfferDeclarations {get;}

        bool ShowCancelButton { set;}
        bool ShowBackButton { set;}
        bool ShowUpdateButton { set;}
        bool UpdateMode { set;}

        string UpdateButtonText { get; set;}

        event EventHandler onCancelButtonClicked;
        event EventHandler onUpdateButtonClicked;
        event EventHandler onBackButtonClicked;

        void SetViewForNullDeclarations();
    }
}
