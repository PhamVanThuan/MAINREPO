using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.DebtCounselling
{
    public class GetSeventeenPointOneDateDaysCommandHandler : IHandlesDomainServiceCommand<GetSeventeenPointOneDateDaysCommand>
    {
        private IStageDefinitionRepository stageDefinitionRepository;
        private ICommonRepository commonRepository;

        public GetSeventeenPointOneDateDaysCommandHandler(IStageDefinitionRepository stageDefinitionRepository, ICommonRepository commonRepository)
        {
            this.stageDefinitionRepository = stageDefinitionRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetSeventeenPointOneDateDaysCommand command)
        {
            // get the 17.1 Stage Definition
            List<int> sdsdgList = new List<int>();
            sdsdgList.Add((int)StageDefinitionStageDefinitionGroups.Received17pt1);
            IList<IStageTransition> stList = stageDefinitionRepository.GetStageTransitionList(command.DebtCounsellingKey, (int)GenericKeyTypes.DebtCounselling2AM, sdsdgList);

            if (stList.Count > 0)
            {
                // get latest Received17pt1 StageTransition by key
                IStageTransition latestStageTransition = stList.OrderByDescending(x => x.Key).FirstOrDefault();
                if (latestStageTransition.EndTransitionDate.HasValue)
                    command.SeventeenPointOneDatePlusDaysResult = commonRepository.GetnWorkingDaysFromDate(command.Days, latestStageTransition.EndTransitionDate.Value);
                else
                    throw new Exception("There is no 17.1 Received date for this Debt Counselling case.");
            }
            else
                throw new Exception("There is no 17.1 Received transition for this Debt Counselling case.");
        }
    }
}