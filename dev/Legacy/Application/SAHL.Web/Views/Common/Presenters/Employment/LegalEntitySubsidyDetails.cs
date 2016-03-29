using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Common.Presenters.Employment
{
    /// <summary>
    /// Displays subsidy details for a legal entity, and allows them to be added to an employment record.
    /// </summary>
    public class LegalEntitySubsidyDetails : LegalEntitySubsidyBase
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntitySubsidyDetails(ISubsidyDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.GridVisible = false;
            _view.ReadOnly = true;
            _view.ShowButtons = false;

            IEmploymentSubsidised employment = EmploymentRepository.GetEmploymentByKey(CachedEmployment.Key) as IEmploymentSubsidised;
            if (employment.Subsidy != null)
                _view.SetSubsidy(employment.Subsidy.Key);

        }

    }
}
