namespace SAHL.X2.Framework.Common
{
    public static class ErrorCodes
    {
        public static int Unknown = -1;
        /// <summary>
        /// The session was valid at one time but expired after being idle for more than the maximum idle time, usually 20 minutes.
        /// </summary>
        public static int ExpiredSession = 1;

        /// <summary>
        /// The Session is invalid.
        /// </summary>
        public static int InvalidSession = 2;

        /// <summary>
        /// The requested WorkFlow is not found, check process name, process version and workflow name.
        /// </summary>
        public static int WorkFlowNotFound = 3;

        /// <summary>
        /// The requested Activity is not found.
        /// </summary>
        public static int ActivityNotFound = 4;

        /// <summary>
        /// An exception has been caught in the X2 engine.
        /// </summary>
        public static int UnExpectedEngineError = 5;

        /// <summary>
        /// The user does not have access to the activity.
        /// </summary>
        public static int ActivityAccessDenied = 6;

        /// <summary>
        /// An instance of the .net WorkFlow could not be retrieved from the cache.
        /// </summary>
        public static int dotNetWorkFlowIsNull = 7;

        /// <summary>
        /// An instance of the .net WorkFlow data could not be retrieved.
        /// </summary>
        public static int dotNetWorkFlowDataIsNull = 8;

        /// <summary>
        /// A lock can not be acquired as the instance is alreadly locked
        /// </summary>
        public static int InstanceAlreadyLocked = 9;

        /// <summary>
        /// The activity completion cannot continue as the instance does not have the requisite lock
        /// </summary>
        public static int InstanceNotLockedForActivity = 10;

        /// <summary>
        /// The Workflows CreateActivity code behind failed
        /// </summary>
        public static int CreationActivityFailed = 11;

        /// <summary>
        /// The Workflows StartActivity code behind failed
        /// </summary>
        public static int StartActivityFailed = 12;

        /// <summary>
        /// The Workflows CompleteActivity code behind failed
        /// </summary>
        public static int CompleteActivityFailed = 13;

        public static int LoginFailed = 14;
        public static int LogoutFailed = 15;
        public static int UnableToRetreiveProcess = 16;
        public static int SplitInstanceFailed = 17;
        public static int EnterStateFailed = 18;
        public static int ExitStateFailed = 19;
        public static int ProcessWorkListsFailed = 20;
    }
}