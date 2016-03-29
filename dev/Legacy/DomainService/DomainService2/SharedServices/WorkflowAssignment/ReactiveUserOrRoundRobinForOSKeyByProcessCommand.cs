namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class ReactiveUserOrRoundRobinForOSKeyByProcessCommand : StandardDomainServiceCommand
    {
        public ReactiveUserOrRoundRobinForOSKeyByProcessCommand(string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string state, SAHL.Common.Globals.Process process, int roundRobinPointerKey)
        {
			this.DynamicRole = dynamicRole;
			this.GenericKey = genericKey;
			this.OrganisationStructureKey = organisationStructureKey;
			this.InstanceID = instanceID;
			this.State = state;
			this.Process = process;
			this.RoundRobinPointerKey = roundRobinPointerKey;
            this.Result = string.Empty;
        }

        public string Result
        {
            get;
            set;
        }

		public string DynamicRole
		{
			get;
			protected set;
		}

		public int GenericKey
		{
			get;
			protected set;
		}

		public int OrganisationStructureKey
		{
			get;
			protected set;
		}

		public long InstanceID
		{
			get;
			protected set;
		}

		public string State
		{
			get;
			protected set;
		}

		public SAHL.Common.Globals.Process Process
		{
			get;
			protected set;
		}

		public int RoundRobinPointerKey
		{
			get;
			protected set;
		}
    }
}