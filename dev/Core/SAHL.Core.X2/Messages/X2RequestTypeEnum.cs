namespace SAHL.Core.X2.Messages
{
    public enum X2RequestType
    {
        UserCreate = 0,
        CreateComplete = 1,
        UserStart = 2,
        UserComplete = 4,
        UserCancel = 8,
        Timer = 16,
        External = 32,
        SystemRequestGroup = 64,
        WorkflowActivity = 128,
        AutoForward = 256,
        SecurityRecalc = 512,
        BundledRequest = 1024,
        UserCreateWithComplete = 2048,
        WrappedRequest = 4096
    }
}