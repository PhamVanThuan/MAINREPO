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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.Employment
{
    public class LegalEntitySubsidyDetailsUpdate : LegalEntitySubsidyBase
    {
        private IEmploymentSubsidised _employment;
        private int subsidyStatusKey;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntitySubsidyDetailsUpdate(ISubsidyDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            View.SaveButtonClicked += new EventHandler(View_SaveButtonClicked);

            base.BindSubsidyAccounts();
            _employment = EmploymentRepository.GetEmploymentByKey(CachedEmployment.Key) as IEmploymentSubsidised;
            
            _view.EmploymentStatusKey = CachedEmployment.EmploymentStatus.Key;
            
            if (_employment.Subsidy != null)
            {
                subsidyStatusKey = _employment.Subsidy.GeneralStatus.Key;
                View.SetSubsidy(_employment.Subsidy.Key);
            }

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.GridVisible = false;
            // if the employment is being set to previous, then the view must be made read-only
            _view.ReadOnly = (CachedEmployment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous);
            _view.ShowButtons = true;
            
        }

        void View_SaveButtonClicked(object sender, EventArgs e)
        {
            // use a new employment object for saving so we don't corrupt the global variable if the save 
            // doesn't happen
            IEmploymentSubsidised employment = EmploymentRepository.GetEmploymentByKey(CachedEmployment.Key) as IEmploymentSubsidised;
            CopyCachedValues(View.CurrentPrincipal, employment);

            try
            {
                // the subsidy is only updated if the employment record is current
                if (employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
                {
                    if (employment.Subsidy == null)
                        employment.Subsidy = EmploymentRepository.GetEmptySubsidy();

                    ISubsidy subsidy = View.GetCapturedSubsidy(employment.Subsidy);

                    // If the subsidy status is INACTIVE, fire a warning before making it ACTIVE.
                    if (subsidyStatusKey == (int)SAHL.Common.Globals.GeneralStatuses.Inactive)
                    {
                        string warningMessage = "By saving this employment record the connected subsidy will be set to ACTIVE.";
                        _view.Messages.Add(new Warning(warningMessage, warningMessage));
                    }

                    subsidy.GeneralStatus = LookupRepository.GeneralStatuses[GeneralStatuses.Active];
                    subsidy.LegalEntity = GetLegalEntity(View.CurrentPrincipal);
                    subsidy.Employment = employment;

                    // assume that OfferKey was selected
                    if (subsidy.Application != null
                        && subsidy.Application.Account != null
                        && subsidy.Application.Account.AccountStatus.Key == (int)AccountStatuses.Application)
                    {
                        subsidy.Account = subsidy.Application.Account;
                    }

                    subsidy.ValidateEntity();

                }
                if (_view.IsValid)
                {
                    SaveEmployment(View, employment, "LegalEntityEmploymentDisplay");
                }

            }
            catch (Exception)
            {
                if (_view.IsValid)
                    throw;
            }
        }
    }
}
