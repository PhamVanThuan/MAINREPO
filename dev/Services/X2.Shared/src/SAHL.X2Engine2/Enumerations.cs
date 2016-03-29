namespace SAHL.X2Engine2
{
    public class Enumerations
    {
        public enum RequestStatus
        {
            Received = 1,
            Routed,
            TimedOut,
            TimedOutAndNotServicable,
            NoRouteAvailable
        }

        public enum ActivityTypes
        {
            User = 1,
            Decision = 2,
            External = 3,
            Timed = 4,
            CallWorkflow = 9,
            ReturnWorkflow = 10
        }
    }
}