namespace DomainService2.SharedServices.Common
{
    public class PerformEWorkActionCommand : StandardDomainServiceCommand
    {
        public PerformEWorkActionCommand(string eFolderID, string actionToPerform, int genericKey, string assignedUser, string currentStage)
        {
            this.EFolderID = eFolderID;
            this.ActionToPerform = actionToPerform;
            this.GenericKey = genericKey;
            this.AssignedUser = assignedUser;
            this.CurrentStage = currentStage;
        }

        public string EFolderID
        {
            get;
            protected set;
        }

        public string ActionToPerform
        {
            get;
            protected set;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public string AssignedUser
        {
            get;
            protected set;
        }

        public string CurrentStage
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}