using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class UserActivity : AbstractActivity
    {
        private List<AbstractRole> securityAccess;

        public UserActivity(string name, Single locationX, Single locationY, CodeSection onStartActivity, CodeSection onCompleteActivity, CodeSection onGetActivityMessage, CodeSection onGetStageTransition,Guid x2ID)
            : base(name, locationX, locationY, onStartActivity, onCompleteActivity, onGetStageTransition, x2ID)
        {
            this.GetActivityMesssageCode = onGetActivityMessage;
            base.AddCodeSection(this.GetActivityMesssageCode);
            this.securityAccess = new List<AbstractRole>();
        }

        public CustomForm CustomForm { get; set; }

        public AbstractActivity LinkedActivity { get; set; }

        public bool UseLinkedActivity { get; set; }

        public CodeSection GetActivityMesssageCode { get; protected set; }

        public ReadOnlyCollection<AbstractRole> SecurityAccess
        {
            get
            {
                return new ReadOnlyCollection<AbstractRole>(this.securityAccess);
            }
        }

        public void AddRoleToSecurityAccess(AbstractRole roleToAdd)
        {
            if (!this.securityAccess.Contains(roleToAdd))
            {
                this.securityAccess.Add(roleToAdd);
            }
        }
    }
}