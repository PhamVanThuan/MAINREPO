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
using System.Web.UI.MobileControls;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IApplicationWarnings : IViewBase
    {

        #region Properties

        bool ShowDetailTypes { get;set;}

        #endregion

        #region Methods

        void PopulateLegalEntityErrors(List<string> leErrors);

        void PopulateApplicationOfferRules(List<string> aoErrors);

        void PopulateCreditMatrixRules(List<string> cmErrors);

        void PopulateDetailTypes(List<string> detailTypes);

        #endregion
    }
}
