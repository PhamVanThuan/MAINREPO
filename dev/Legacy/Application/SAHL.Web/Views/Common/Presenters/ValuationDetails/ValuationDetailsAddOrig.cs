using System;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{
    /// <summary>
    /// Valuation Details Add
    /// </summary>
    public class ValuationDetailsAddOrig : ValuationDetailsAdd
    {

        private Dictionary<string, string> inputFields = new Dictionary<string, string>();
        /// <summary>
        /// Valuation details add
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ValuationDetailsAddOrig(Interfaces.IValuationManualDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Submit button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _view_OnSubmitButtonClicked(object sender, KeyChangedEventArgs e)
        {
            IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
            IValuationDiscriminatedSAHLManual val = null;
            // SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            if (GlobalCacheData.ContainsKey(ViewConstants.ValuationManual))
                val = GlobalCacheData[ViewConstants.ValuationManual] as IValuationDiscriminatedSAHLManual;

            val = _view.GetUpdatedValuation(val) as IValuationDiscriminatedSAHLManual;

            if (val != null)
            {
                val.IsActive = true;
                val.Property = _properties[0];

                //TODO Remove this or move it to the correct Valuations screen - check this with paul
                //inputFields.Clear();
                //inputFields.Add("PropertyKey", val.Property.Key.ToString());
                //inputFields.Add("RequestingAdUser", spc.GetADUser(View.CurrentPrincipal).ADUserName);

                TransactionScope txn = new TransactionScope();

                try
                {
                    // perform validation - we need to validate before trying to save otherwise the 
                    // the object gets assigned a key and we'll fall over later on
                    if (val.ValidateEntity())
                    {
                        // Update HOC with the manual valuation 
                        ManualValuationHOCUpdate(val);

                        propRepo.SaveValuation(val);

                        IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                        // Update PropertyValuation on OfferInformationVariableLoan with new Valuation Amount    
                        IApplication application = applicationRepo.GetApplicationByKey(_genericKey);
                        IApplicationInformation appInfo = application.GetLatestApplicationInformation();
                        IApplicationInformationVariableLoan appVarLoan = applicationRepo.GetApplicationInformationVariableLoan(appInfo.Key);
                        appVarLoan.PropertyValuation = val.ValuationAmount;
                        //When updating the Val Amount, the application must be recalculated for rules etc.
                        IApplicationMortgageLoan appML = application as IApplicationMortgageLoan;
                        if (appML != null)
                            appML.CalculateApplicationDetail(false, false);

                        applicationRepo.SaveApplication(application);


                        txn.VoteCommit();

                        GlobalCacheData.Remove(ViewConstants.ValuationManual);

                        // pass some values back to x2
                        inputFields.Clear();
                        inputFields.Add("PropertyKey", val.Property.Key.ToString());
                        inputFields.Add("ValuationKey", val.Key.ToString());

                        X2Service.CompleteActivity(_view.CurrentPrincipal, inputFields, false);
                        X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                    }
                }

                catch (Exception)
                {
                    txn.VoteRollBack();
                    //X2Service.CancelActivity(_view.CurrentPrincipal);

                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    txn.Dispose();
                }
            }
        }

        protected override void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ValuationManual);
            X2Service.CancelActivity(_view.CurrentPrincipal);
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

    }

}
