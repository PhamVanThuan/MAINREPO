using System;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class ClapperBoard : AbstractState
    {
        public ClapperBoard(string keyVariable, string subject, GlobalRole limitAccessToRole, Single locationX, Single locationY)
            : base(locationX, locationY)
        {
            this.LimitAccessToRole = limitAccessToRole;
            this.KeyVariable = keyVariable;
            this.Subject = subject;
        }

        public string Subject { get; protected set; }

        public string KeyVariable { get; protected set; }

        public void UpdateKeyVariable(string keyVariable)
        {
            if (string.IsNullOrEmpty(keyVariable))
            {
                throw new ArgumentException("KeyVariable may not be empty.", "keyVariable");
            }

            this.KeyVariable = keyVariable;
        }

        public void UpdateSubject(string subject)
        {
            this.Subject = subject;
        }

        public GlobalRole LimitAccessToRole { get; protected set; }

        public void UpdateLimitAccessToRole(GlobalRole limitAccessToRole)
        {
            this.LimitAccessToRole = limitAccessToRole;
        }
    }
}