using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Disputes
{
    [RuleDBTag("CheckDisputes",
    "CheckDisputes",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Disputes.CheckDisputes")]
    [RuleInfo]
    public class CheckDisputes : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The CheckDisputes rule expects a Domain object to be passed.");

            IAccount _account = Parameters[0] as IAccount;

            if (_account == null)
                throw new ArgumentException("The CheckDisputes rule expects the following objects to be passed: IAccount.");

            #endregion

            IStageDefinitionRepository sdRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            List<int> sdsdgList = new List<int>();
            sdsdgList.Add((int)StageDefinitionStageDefinitionGroups.DisputesIn);
            IList<IStageTransition> inStageTransList = sdRepo.GetStageTransitionList(_account.Key,(int)GenericKeyTypes.Losscontrol,sdsdgList);

            sdsdgList = new List<int>();
            sdsdgList.Add((int)StageDefinitionStageDefinitionGroups.AwaitingSpouseConfirmationOut);
            sdsdgList.Add((int)StageDefinitionStageDefinitionGroups.DebtCounsellingQuickCashIn);
            sdsdgList.Add((int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlOut);
            IList<IStageTransition> outStageTransList = sdRepo.GetStageTransitionList(_account.Key, (int)GenericKeyTypes.Losscontrol, sdsdgList);

            if (inStageTransList.Count > outStageTransList.Count)
            {
                string errorMessage = "The account has a dispute";
                AddMessage(errorMessage, errorMessage, Messages);
            }
            return 0;
        }
    }
}
