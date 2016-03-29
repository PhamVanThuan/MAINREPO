using System;
namespace SAHL.X2Engine2.Tests.X2.Models
{
    public sealed class X2StateActivity
    {
        public string State { get; private set; }
        public string Activity { get; private set; }

        public X2StateActivity(System.String State, System.String Activity)
        {
            this.Activity = Activity;
            this.State = State;
        }
    }
}
