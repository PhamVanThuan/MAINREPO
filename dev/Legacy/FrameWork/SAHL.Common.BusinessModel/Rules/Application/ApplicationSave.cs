using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Application.ApplicationSave
{
    [RuleDBTag("ApplicationOpenSave",
    "ApplicationSave rules: the application must be open in order to save it.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.ApplicationSave.ApplicationOpenSave")]
    public class ApplicationOpenSave : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplication app = Parameters[0] as IApplication;

            if (app == null)
                throw new ArgumentException("Parameters[0] is not of type IApplication.");
            // Check if it is really really open
            if (app.Key > 0 && app.IsOpen)
                return 1;

            // if we are reinstating an ntu'd application then its ok
            if (app.Key > 0 
                && app.ApplicationStatusPrevious != null && app.ApplicationStatusPrevious.Key == (int)OfferStatuses.NTU
                && app.ApplicationStatus != null && app.ApplicationStatus.Key == (int)OfferStatuses.Open)
                return 1;

            // if it's an existing application with a status not equal to open, then the rule fails
            if (app.Key > 0 && app.ApplicationStatusPrevious != null && app.ApplicationStatusPrevious.Key != (int)OfferStatuses.Open)
            {
                string err = String.Format("Mortgage loan application: {0} is not open and can not be saved.", app.Key);
                AddMessage(err, err, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ApplicationInformationAcceptedSave",
    "ApplicationSave rules: if the client has accepted the application, no changes are allowed.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.ApplicationSave.ApplicationInformationAcceptedSave")]
    public class ApplicationInformationAcceptedSave : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters[0] is IApplicationUnknown)
                return 1;

            IApplicationInformation appInfo = Parameters[0] as IApplicationInformation;

            if (appInfo == null)
                throw new ArgumentException("Parameters[0] is not of type IApplicationInformation.");

            // if it's an existing object with a status of accepted, then the rule fails
            if (appInfo.Key > 0 && appInfo.ApplicationInformationTypePrevious != null && appInfo.ApplicationInformationTypePrevious.Key == (int)OfferInformationTypes.AcceptedOffer)
            {
                string err = String.Format("Application: {0} has been accepted by the client and can not be saved.", appInfo.Application.Key);
                AddMessage(err, err, Messages);
                return 0;
            }

            return 1;
        }
    }




    [RuleDBTag("EdgeMaximumLoanAgreementAmount",
  "EdgeMaximumLoanAgreementAmount rules: if the client has accepted the application, no changes are allowed.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Application.ApplicationSave.EdgeMaximumLoanAgreementAmount")]
    public class EdgeMaximumLoanAgreementAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters[0] is IApplicationUnknown)
                return 1;

            IApplication app = Parameters[0] as IApplication;

            if (app == null)
                throw new ArgumentException("Parameters[0] is not of type Application.");

            IApplicationProductEdge prod = app.CurrentProduct as IApplicationProductEdge;
            if (prod != null)
            {
                // Lookup Edge max LAA from conrtol table
                IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                IControl ctrl = ctrlRepo.GetControlByDescription("EdgeMaxLAA");

                if (ctrl == null || !ctrl.ControlNumeric.HasValue)
                    throw new ArgumentNullException("There is no value for 'Edge Max LAA' in the control table");

                double maxEdgeLAA = ctrl.ControlNumeric.Value;
                IApplicationMortgageLoan appML = app as IApplicationMortgageLoan;
                
                // if it's an existing object with a status of accepted, then the rule fails
                if (appML.LoanAgreementAmount > maxEdgeLAA)
                {
                    string prodDesc = appML.GetLatestApplicationInformation().Product.Description;
                    string err = String.Format("The maximum Loan Agreement amount for {0} is R{1}.", prodDesc, maxEdgeLAA);
                    AddMessage(err, err, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }







}
