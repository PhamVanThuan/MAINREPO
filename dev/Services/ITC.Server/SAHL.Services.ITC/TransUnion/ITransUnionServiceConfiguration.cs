namespace SAHL.Services.ITC.TransUnion
{
    public interface ITransUnionServiceConfiguration
    {
        string ClientReference { get; }

        string SecurityCode { get; }

        string SubscriberCode { get; }

        string ContactName { get; }

        string ContactNumber { get; }

        string Destination { get; }
    }
}