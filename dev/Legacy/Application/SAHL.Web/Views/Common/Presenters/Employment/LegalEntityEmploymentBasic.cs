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
using SAHL.Web.Controls;
using SAHL.Common.Globals;
using SAHL.Web.Controls.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.Employment
{
    /// <summary>
    /// Base class for the legal entity employment views (Display, Add, Update)
    /// </summary>
    public class LegalEntityEmploymentBasic : LegalEntityEmploymentBase<IEmploymentView>
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityEmploymentBasic(IEmploymentView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (_view.IsMenuPostBack)
                ClearCachedData();

            if (!_view.ShouldRunPage) return;

            _view.EmploymentSelected += new EventHandler(_view_EmploymentSelected);
            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);

            BindLegalEntityEmployment();

        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

        protected void _view_EmploymentSelected(object sender, EventArgs e)
        {
            // clear the cached data
            ClearCachedData();
            _view.EmployerDetails.ClearEmployer();

            IEmployment emp = View.SelectedEmployment;
            if (emp.Employer != null)
                CachedEmployerKey = emp.Employer.Key;
            PopulateEmploymentDetails(emp, _view);
            CachedEmployment = emp;

            BindLegalEntityEmployment();
        }


        private void BindLegalEntityEmployment()
        {
            ILegalEntity le = GetLegalEntity(View.CurrentPrincipal);
            if (CachedEmployment != null)
                _view.SelectedEmployment = CachedEmployment;

            _view.BindEmploymentDetails(le.Employment, true);

        }

        /// <summary>
        /// Central method that can be used to work out where to navigate to next
        /// </summary>
        /// <param name="currentViewName"></param>
        /// <param name="employment"></param>
        /// <returns></returns>
        /// <remarks>Marked as static as does not use "this".</remarks>
        protected static string GetNavigateTarget(string currentViewName, IEmployment employment)
        {
            if (employment == null)
                return null;

            string extendedView = "EmploymentExtended";
            string subsidyView = "SubsidyDetails";

            if (currentViewName == "LegalEntityEmploymentUpdate")
            {
                //if (employment.RemunerationType != null
                //&& employment.RemunerationType.Key != (int)RemunerationTypes.Unknown
                //&& employment.RemunerationType.Key != (int)RemunerationTypes.Salaried
                //&& employment.RemunerationType.Key != (int)RemunerationTypes.HourlyRate
                //&& employment.RemunerationType.Key != (int)RemunerationTypes.BasicAndCommission)
                //{
                //    if (employment.ConfirmedEmploymentFlag.HasValue
                //        && Convert.ToInt32(employment.ConfirmedEmploymentFlag.Value) == (int)SAHL.Common.Globals.ConfirmedEmployment.Yes)
                //    {
                //        if (employment.ConfirmedBasicIncome.HasValue && employment.ConfirmedBasicIncome.Value > 0D)
                //            return extendedView;
                //    }
                //}
                //else
                //    return extendedView;

                if (employment.RemunerationType.Key == (int)RemunerationTypes.Salaried ||
                    employment.RemunerationType.Key == (int)RemunerationTypes.BasicAndCommission)
                {
                        return extendedView;
                }
                else if (employment.ConfirmedIncomeFlag.HasValue && 
                        Convert.ToInt32(employment.ConfirmedIncomeFlag.Value) == (int)SAHL.Common.Globals.ConfirmedIncome.Yes)
                {
                    return extendedView;
                }
                else if (employment.ConfirmedEmploymentFlag.HasValue && 
                        Convert.ToInt32(employment.ConfirmedEmploymentFlag.Value) == (int)SAHL.Common.Globals.ConfirmedEmployment.Yes)
                {
                    return extendedView;
                }
                else
                    return null;
            }

                // determine if we need to gather extended employment details
                if (employment.RequiresExtended &&
                    (currentViewName != extendedView) &&
                    (currentViewName != subsidyView))
                    return extendedView;

            // determine if we need to gather subsidy details
            if ((employment is IEmploymentSubsidised) && currentViewName != subsidyView)
                return subsidyView;

            // no further information required - return null so basic employment details can be saved
            return null;

        }


        protected static void ResetConfirmedEmploymentValues(IEmployment emp)
        {
            emp.ConfirmedBasicIncome = new double?();
            if (emp.ExtendedEmployment != null)
            {
                emp.ExtendedEmployment.ConfirmedCommission = new double?();
                emp.ExtendedEmployment.ConfirmedOvertime = new double?();
                emp.ExtendedEmployment.ConfirmedShift = new double?();
                emp.ExtendedEmployment.ConfirmedPerformance = new double?();
                emp.ExtendedEmployment.ConfirmedAllowances = new double?();
                emp.ExtendedEmployment.ConfirmedPAYE = new double?();
                emp.ExtendedEmployment.ConfirmedUIF = new double?();
                emp.ExtendedEmployment.ConfirmedPensionProvident = new double?();
                emp.ExtendedEmployment.ConfirmedMedicalAid = new double?();
            }
        }

    }
}
