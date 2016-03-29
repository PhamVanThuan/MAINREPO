namespace DomainService2.Workflow.Life
{
    using System;
    using SAHL.Common;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;

    public class AwaitingConfirmationTimeoutCommandHandler : IHandlesDomainServiceCommand<AwaitingConfirmationTimeoutCommand>
    {
        private IOrganisationStructureRepository OrganisationStructureRepository;
        private ILifeRepository LifeRepository;
        private IReasonRepository ReasonRepository;
        private IStageDefinitionRepository StageDefinitionRepository;
        private ILookupRepository LookupRepository;

        public AwaitingConfirmationTimeoutCommandHandler(IOrganisationStructureRepository organisationStructureRepository, ILifeRepository lifeRepository, IReasonRepository reasonRepository, IStageDefinitionRepository stageDefinitionRepository, ILookupRepository lookupRepository)
        {
            this.OrganisationStructureRepository = organisationStructureRepository;
            this.LifeRepository = lifeRepository;
            this.ReasonRepository = reasonRepository;
            this.StageDefinitionRepository = stageDefinitionRepository;
            this.LookupRepository = lookupRepository;
        }

        public void Handle(IDomainMessageCollection messages, AwaitingConfirmationTimeoutCommand command)
        {
            IADUser adUser = OrganisationStructureRepository.GetAdUserForAdUserName("System");

            IReadOnlyEventList<IReasonDefinition> reasonDefinitions = ReasonRepository.GetReasonDefinitionsByReasonDescriptionKey(ReasonTypes.LifeCallback, (int)ReasonDescriptions.AwaitingSpouseConfirmation);
            if (reasonDefinitions.Count > 0)
            {
                IReasonDefinition reasonDefinition = reasonDefinitions[0];

                // write the stage transition  record
                IStageTransition stageTransition = StageDefinitionRepository.SaveStageTransition(command.ApplicationKey
                        , Convert.ToInt32(StageDefinitionGroups.LifeOrigination)
                        , Constants.StageDefinitionConstants.CallBackSet
                        , reasonDefinition.ReasonDescription.Description
                        , adUser);

                // save the Reason
                IReason reason = ReasonRepository.CreateEmptyReason();
                reason.GenericKey = command.ApplicationKey;
                reason.Comment = "Awaiting Spouse Confirmation";
                reason.ReasonDefinition = reasonDefinition;
                reason.StageTransition = stageTransition;
                ReasonRepository.SaveReason(reason);

                // save the callback
                ICallback callback = LifeRepository.CreateEmptyCallback();
                callback.GenericKey = command.ApplicationKey;
                callback.GenericKeyType = LookupRepository.GenericKeyType.ObjectDictionary[Convert.ToString((int)GenericKeyTypes.Offer)];
                callback.CallbackDate = DateTime.Now;
                callback.CallbackUser = adUser.ADUserName;
                callback.EntryDate = DateTime.Now;
                callback.EntryUser = adUser.ADUserName;
                callback.Reason = reason;
                LifeRepository.SaveCallback(callback);
            }
        }
    }
}