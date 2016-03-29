using SAHL.V3.Framework;
using SAHL.V3.Framework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Summary description for DecisionTree
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class DecisionTree : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public DecisionTreeResult DetermineNCRGuidelineMinMonthlyFixedExpenses(decimal grossMonthlyIncome)
        {
            var v3ServiceManager = V3ServiceManager.Instance;
            IDecisionTreeRepository decisionTreeRepo = v3ServiceManager.Get<IDecisionTreeRepository>();
            var minMonthlyExpenses = decisionTreeRepo.DetermineNCRGuidelineMinMonthlyFixedExpenses(grossMonthlyIncome);

            return new DecisionTreeResult
            {
                Result = minMonthlyExpenses
            };
        }
    }


    public class DecisionTreeResult
    {
        public decimal Result { get; set; }
    }

}
