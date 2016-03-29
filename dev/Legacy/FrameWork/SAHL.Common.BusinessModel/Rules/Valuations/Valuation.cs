using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using System.Linq;

namespace SAHL.Common.BusinessModel.Rules.Valuations
{
    [RuleDBTag("ValuationValuer",
"Valuation Valuer must be captured.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ValuationValuer")]
    [RuleInfo]
    public class ValuationValuer : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IValuation val = (IValuation)Parameters[0];

            if (val.Valuator == null)
            {
                AddMessage("Valuer is required.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("ValuationValuationDateThreshold",
   "Valuation Date must be less than or equal to today's date.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Valuations.ValuationValuationDateThreshold")]
    [RuleInfo]
    public class ValuationValuationDateThreshold : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IValuation val = (IValuation)Parameters[0];

            if (val.ValuationDate == System.DateTime.MinValue)
            {
                AddMessage("Valuation Date must be entered.", "", Messages);
            }

            if (Convert.ToDateTime(val.ValuationDate).Date > DateTime.Now.Date)
            {
                AddMessage("Valuation Date must be less than or equal to today's date.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("ValuationValuationAmountMinimum",
   "Valuation Amount less than Minimum.",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Valuations.ValuationValuationAmountMinimum")]
    [RuleParameterTag(new string[] { "@ValuationAmountMinimum,100000,7" })]
    [RuleInfo]
    public class ValuationValuationAmountMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            Double minimumValue = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            IValuation val = (IValuation)Parameters[0];

            double valuationAmount = 0;
            valuationAmount = val.ValuationAmount.HasValue ? val.ValuationAmount.Value : 1;

            if (valuationAmount <= minimumValue)
            {
                AddMessage("Valuation Amount must be greater than " + minimumValue.ToString(SAHL.Common.Constants.CurrencyFormat) + ".", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("ValuationHOCRoof",
"Valuation HOC Roof is required",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Valuations.ValuationHOCRoof")]
    [RuleInfo]
    public class ValuationHOCRoof : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IValuation val = (IValuation)Parameters[0];

            if (val.HOCRoof == null)
            {
                AddMessage("Valuation HOC Roof is required.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("ValuationHOCAmount",
   "Valuation HOCAmount must be captured.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Valuations.ValuationHOCAmount")]
    [RuleInfo]
    public class ValuationHOCAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IValuation val = (IValuation)Parameters[0];

            double valuationHOCValue = 0;
            valuationHOCValue = val.ValuationHOCValue.HasValue ? val.ValuationHOCValue.Value : 1;

            if (valuationHOCValue < 1)
            {
                AddMessage("Valuation HOC Amount must be captured.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("ValuationActiveStatus",
    "Only one Valuation per property can be active.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Valuations.ValuationActiveStatus")]
    [RuleInfo]
    public class ValuationActiveStatus : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IValuation val = (IValuation)Parameters[0];

            int numberActive = 0;

            if (val.Property != null)
            {
                for (int i = 0; i < val.Property.Valuations.Count; i++)
                {
                    if (val.Property.Valuations[i].IsActive == true)
                        numberActive += 1;
                }
            }

            if (numberActive > 1)
                AddMessage("Only one Valuation per Property may be set to Active", "", Messages);

            return 1;
        }
    }

    [RuleDBTag("ValuationTypeValidation",
    "Invalid Valuation Discrimination.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Valuations.ValuationTypeValidation")]
    [RuleInfo]
    public class ValuationTypeValidation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IValuation val = (IValuation)Parameters[0];

            IValuationDiscriminatedAdCheckDesktop valAdCheckDeskTop = val as IValuationDiscriminatedAdCheckDesktop;
            IValuationDiscriminatedAdCheckPhysical valADCheckPhysical = val as IValuationDiscriminatedAdCheckPhysical;
            IValuationDiscriminatedLightstoneAVM valLSAVM = val as IValuationDiscriminatedLightstoneAVM;
            IValuationDiscriminatedSAHLClientEstimate valClientEstimate = val as IValuationDiscriminatedSAHLClientEstimate;
            IValuationDiscriminatedSAHLManual valSAHLManual = val as IValuationDiscriminatedSAHLManual;
            IValuationDiscriminatedLightStonePhysical valLightstonePhysical = val as IValuationDiscriminatedLightStonePhysical;

            if (valAdCheckDeskTop == null && valADCheckPhysical == null && valLSAVM == null && valClientEstimate == null && valSAHLManual == null && valLightstonePhysical == null)
            {
                AddMessage("Invalid Valuation Discrimination", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("ValuationApplication",
    "Application must have a Valuation.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Valuations.ValuationApplication")]
    [RuleInfo]
    public class ValuationApplication : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplicationMortgageLoan App = (IApplicationMortgageLoan)Parameters[0];

            if (App == null)
                throw new ArgumentException("Parameter[0] is not of type IApplicationMortgageLoan.");

            if (App.Property == null)
            {
                AddMessage("Valuation could not be found. A property does not exist for the application", "", Messages);
            }

            if (App.Property.Valuations == null || App.Property.Valuations.Count == 0)
                AddMessage("The application must have a valuation.", "", Messages);

            return 1;
        }
    }

    [RuleDBTag("AutomatedValuationRapid",
   "The valuation for this application has not been completed",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Valuations.AutomatedValuationRapid", false)]
    [RuleInfo]
    public class AutomatedValuationRapid : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplicationMortgageLoan App = (IApplicationMortgageLoan)Parameters[0];

            if (App.ApplicationType.Key == (int)OfferTypes.ReAdvance)
            {
                IStageDefinitionRepository stageRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                IList<IStageTransition> stageTransitions = stageRepo.GetStageTransitionsByGenericKey(App.Key);

                bool valInstructed = false;
                bool valCompleted = false;

                for (int i = 0; i < stageTransitions.Count; i++)
                {
                    if (stageTransitions[i].StageDefinitionStageDefinitionGroup.StageDefinitionGroup.Key == (int)StageDefinitionGroups.PhysicalValuation)
                        valInstructed = true;
                    if (stageTransitions[i].StageDefinitionStageDefinitionGroup.StageDefinitionGroup.Key == (int)StageDefinitionGroups.CompleteValuation)
                        valCompleted = true;
                }

                if (valInstructed && !valCompleted)
                    AddMessage("The valuation assessment for this application has not yet been completed.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("AutomatedValuationFurtherLoanFurtherAdvance",
   "The valuation for this application has not been completed",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Valuations.AutomatedValuationFurtherLoanFurtherAdvance")]
    [RuleInfo]
    public class AutomatedValuationFurtherLoanFurtherAdvance : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplicationMortgageLoan App = (IApplicationMortgageLoan)Parameters[0];

            if (App.ApplicationType.Key == (int)OfferTypes.FurtherLoan || App.ApplicationType.Key == (int)OfferTypes.FurtherAdvance)
            {
                IStageDefinitionRepository stageRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                IList<IStageTransition> stageTransitions = stageRepo.GetStageTransitionsByGenericKey(App.Key);

                bool valInstructed = false;
                bool valCompleted = false;

                for (int i = 0; i < stageTransitions.Count; i++)
                {
                    if (stageTransitions[i].StageDefinitionStageDefinitionGroup.StageDefinitionGroup.Key == (int)StageDefinitionGroups.PhysicalValuation)
                        valInstructed = true;
                    if (stageTransitions[i].StageDefinitionStageDefinitionGroup.StageDefinitionGroup.Key == (int)StageDefinitionGroups.CompleteValuation)
                        valCompleted = true;
                }

                if (valInstructed && !valCompleted)
                    AddMessage("The valuation assessment for this application has not yet been completed.", "", Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("CheckActiveValuation12Months",
  "Checks whether there is an active valuation in the last 12 months.",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Valuations.CheckActiveValuation12Months")]
    [RuleInfo]
    public class CheckActiveValuation12Months : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
			if (Parameters[0] is IApplicationFurtherLending)
				return 1;
            IApplicationMortgageLoan applicationMortgageLoan = (IApplicationMortgageLoan)Parameters[0];

            IEventList<IValuation> valuations = applicationMortgageLoan.Property.Valuations;

            bool valid12Months = false;
            foreach (IValuation valuation in valuations)
            {
                if (valuation.IsActive && (valuation.ValuationDate >= DateTime.Now.AddMonths(-12)))
                {
                    valid12Months = true;
                }
            }
            if (!valid12Months)
                AddMessage("Active valuation not conducted within the last 12 months.", "", Messages);

            return 1;
        }
    }

    [RuleDBTag("CheckActiveValuation12MonthsWarning",
 "Checks whether there is an active valuation in the last 12 months.",
  "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Valuations.CheckActiveValuation12MonthsWarning")]
    [RuleInfo]
    public class CheckActiveValuation12MonthsWarning : CheckActiveValuation12Months
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            return base.ExecuteRule(Messages, Parameters);
        }
    }

    #region ValuationCheckApplicationManagementWorkflowCaseOpen

    [RuleDBTag("ValuationCheckApplicationManagementWorkflowCaseOpen",
   "ValuationCheckApplicationManagementWorkflowCaseOpen",
    "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Valuations.ValuationCheckApplicationManagementWorkflowCaseOpen")]
    [RuleInfo]
    public class ValuationCheckApplicationManagementWorkflowCaseOpen : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ValuationCheckApplicationManagementWorkflowCaseOpen rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The ValuationCheckApplicationManagementWorkflowCaseOpen rule expects the following objects to be passed: IApplication.");

            #endregion Check for allowed object type(s)

            IApplication application = Parameters[0] as IApplication;

            string HQL = @"select stOut from StageTransition_DAO stOut where stOut.GenericKey = ? and stOut.StageDefinitionStageDefinitionGroup in (2174,2149,2057,1507)";

            SimpleQuery<StageTransition_DAO> queryOut = new SimpleQuery<StageTransition_DAO>(HQL, application.Key);
            StageTransition_DAO[] resOut = queryOut.Execute();

            HQL = @"select stIn from StageTransition_DAO stIn where stIn.GenericKey = ? and stIn.StageDefinitionStageDefinitionGroup in (1889, 1891, 1895)";
            SimpleQuery<StageTransition_DAO> queryIn = new SimpleQuery<StageTransition_DAO>(HQL, application.Key);
            StageTransition_DAO[] resIn = queryIn.Execute();

            if (resOut.Length < resIn.Length)
            {
                string errorMessage = String.Format("Valuation Workflow Case still open for this application.");
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;
        }
    }

    #endregion ValuationCheckApplicationManagementWorkflowCaseOpen

    #region ValuationRecentExists

    [RuleDBTag("ValuationRecentExists",
    "Warns if a recent Valuation Exists that should be used instead of creating a new one",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Valuations.ValuationRecentExists")]
    [RuleInfo]
    public class ValuationRecentExists : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int months = 0;

            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ValuationRecentExists rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IApplicationMortgageLoan))
                throw new ArgumentException("The ValuationRecentExists rule expects the following objects to be passed: IApplicationMortgageLoan.");

            if (RuleItem.RuleParameters.Count < 1)
                throw new ArgumentException("The ValuationRecentExists rule expects Rule Parameters to be configured.");
            else
            {
                bool b = Int32.TryParse(RuleItem.RuleParameters[0].Value, out months);
                if (!b || months == 0)
                    throw new ArgumentException("The ValuationRecentExists rule expects Rule Parameters to be configured correctly.");
            }

            #endregion Check for allowed object type(s)

            IApplicationMortgageLoan appML = Parameters[0] as IApplicationMortgageLoan;

            //Only valid for new business
            if (appML != null && appML.Property != null && (appML.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan || appML.ApplicationType.Key == (int)OfferTypes.SwitchLoan || appML.ApplicationType.Key == (int)OfferTypes.RefinanceLoan))
            {
                foreach (IValuation val in appML.Property.Valuations)
                {
                    if (val.IsActive && val.ValuationDate > DateTime.Now.AddMonths(-months))
                    {
                        string errorMessage = String.Format("Existing active valuations younger than {0} months exists. A new valuation is not required for this property", months);
                        AddMessage(errorMessage, errorMessage, Messages);
                        return 1;
                    }
                }
            }
            return 0;
        }
    }

    #endregion ValuationRecentExists

    [RuleDBTag("ValuationRequestPending",
    "Checks if there is already a pending valuation for a Property",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Valuations.ValuationRequestPending")]
    [RuleInfo]
    public class ValuationRequestPending : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            var property = Parameters[0] as IProperty;

            var pendingValuation = property.Valuations.Where(x => x.ValuationStatus.Key == (int)ValuationStatuses.Pending)
                                                      .FirstOrDefault();
            if (pendingValuation != null)
            {
                string errorMessage = "Property has a pending valuation request.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;
        }
    }


}