using System;

namespace Automation.DataModels
{
    public class eWorkCase : Automation.DataModels.DebtCounselling
    {
        #region Constructors

        public eWorkCase()
        {
        }

        public eWorkCase(eWorkCase eworkCase, Automation.DataModels.DebtCounselling debtCounselling)
        {
            var thisInstance = this;
            Helpers.SetProperties<eWorkCase, Automation.DataModels.DebtCounselling>(ref thisInstance, debtCounselling);

            //Has to be set last!!!
            Helpers.SetProperties<eWorkCase, eWorkCase>(ref thisInstance, eworkCase);
        }

        #endregion Constructors

        public string BackToStage { get; set; }

        public string AssignedUser { get; set; }

        public string eMapName { get; set; }

        public bool IsSubsidised { get; set; }

        public DateTime eEventTime { get; set; }

        public int ProductKey { get; set; }
    }
}