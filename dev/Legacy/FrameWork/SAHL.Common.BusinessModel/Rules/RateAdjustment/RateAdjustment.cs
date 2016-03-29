using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.RateAdjustment
{
    [RuleDBTag("RateAdjustmentGenericRule",
    "Generic placeholder to execute the configured rateadjustmentelementtype's uiStatement to determine whether the adjustment gets applied.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.RateAdjustment.RateAdjustmentGenericRule")]
    public class RateAdjustmentGenericRule : BusinessRuleBase
    {
        public RateAdjustmentGenericRule(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length != 2)
                throw new ArgumentException("The RateAdjustmentGenericRule rule expects 2 objects to be passed.");

            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("The RateAdjustmentGenericRule rule expects the following objects to be passed: IApplication.");

            if (!(Parameters[1] is IRateAdjustmentElement))
                throw new ArgumentException("The RateAdjustmentGenericRule rule expects the following objects to be passed: IRateAdjustmentElement.");

            #endregion Check for allowed object type(s)

            IApplication app = Parameters[0] as IApplication;
            IRateAdjustmentElement rae = Parameters[1] as IRateAdjustmentElement;

            if (app == null)
                throw new ArgumentException("Parameters[0] is not of type IApplication.");

            if (rae == null)
                throw new ArgumentException("Parameters[1] is not of type IRateAdjustmentElement.");

            string sql = UIStatementRepository.GetStatement("COMMON", rae.RateAdjustmentElementType.StatementName);
            ParameterCollection prms = new ParameterCollection();
            switch (rae.GenericKeyType.Key)
            {
                case (int)Globals.GenericKeyTypes.Offer:

                    //Helper.AddIntParameter(prms, "@OfferKey", app.Key);
                    prms.Add(new SqlParameter("@OfferKey", app.Key));
                    break;
                case (int)Globals.GenericKeyTypes.Account:

                    //Helper.AddIntParameter(prms, "@AccountKey", app.Account.Key);
                    prms.Add(new SqlParameter("@AccountKey", app.Account.Key));
                    break;
                default:
                    return 0; // invalid GenericKeyType
            }

            //Helper.AddIntParameter(prms, "@RateAdjustmentElementKey", rae.Key);
            prms.Add(new SqlParameter("@RateAdjustmentElementKey", rae.Key));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), prms);

            if (ds.Tables.Count == 0)
                return 0; // no data

            int value = -1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Convert.IsDBNull(dr[0]))
                    break;
                value = Convert.ToInt32(dr[0]);
                break; // no data
            }
            return value;
        }
    }

    [RuleDBTag("RateAdjustmentCounterRateStillValid",
    "Determines whether a particular rate adjustment (Counter Rate) is still valid.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.RateAdjustment.RateAdjustmentCounterRateStillValid")]
    public class RateAdjustmentCounterRateStillValid : BusinessRuleBase
    {
        /// <summary>
        /// Determine Whether a Particular Rate Adjustment (Counter Rate) is still valid
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            if (parameters.Count() != 1)
            {
                throw new ArgumentException("The RateAdjustmentCounterRateStillValid rule expects at least one parameter to be passed.");
            }
            if (!(parameters[0] is IApplication))
            {
                throw new ArgumentException("The RateAdjustmentCounterRateStillValid rule expects the following objects to be passed: IApplication.");
            }

            var application = parameters[0] as IApplication;

            //Get the Application Rate Adjustment
            var applicationInformationFinancialAdjustment = GetApplicationInformationFinancialAdjustments(application, FinancialAdjustmentTypeSources.CounterRate);
            IApplicationInformationAppliedRateAdjustment appliedRateAdjustment = null;
            if (applicationInformationFinancialAdjustment != null)
            {
                appliedRateAdjustment = applicationInformationFinancialAdjustment.ApplicationInformationAppliedRateAdjustments != null
                                        ? applicationInformationFinancialAdjustment.ApplicationInformationAppliedRateAdjustments.FirstOrDefault()
                                        : null;
            }

            //Does the offer still satisfy the conditions for the Rate Adjustment to be applied
            if (appliedRateAdjustment != null)
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                int rulePassed = svc.ExecuteRule(spc.DomainMessages, appliedRateAdjustment.RateAdjustmentElement.RuleItem.Name, application, appliedRateAdjustment.RateAdjustmentElement);
                if (rulePassed > 0)
                {
                    return 0;
                }
                else
                {
                    //Failure
                    string ruleMessage = "This application has a Counter Rate pricing adjustment that it no longer qualifies for.";
                    AddMessage(ruleMessage, ruleMessage, messages);
                    return 1;
                }
            }

            //Rule Success
            return 0;
        }

        /// <summary>
        /// Get the Application Information FinancialAdjustments of a Particular Type
        /// </summary>
        /// <param name="application"></param>
        /// <param name="fats"></param>
        /// <returns>IApplicationInformationFinancialAdjustment</returns>
        private IApplicationInformationFinancialAdjustment GetApplicationInformationFinancialAdjustments(IApplication application, FinancialAdjustmentTypeSources fats)
        {
            var latestApplicationInformation = application.GetLatestApplicationInformation();
            if (latestApplicationInformation == null || latestApplicationInformation.ApplicationInformationFinancialAdjustments == null)
            {
                return null;
            }
            var applicationFinancialAdjustment = (from financialAdjustment in latestApplicationInformation.ApplicationInformationFinancialAdjustments
                                                  where financialAdjustment.FinancialAdjustmentTypeSource.Key == (int)fats
                                                  select financialAdjustment).FirstOrDefault();
            return applicationFinancialAdjustment;
        }
    }
}