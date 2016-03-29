namespace SAHL.Config.Services
{
    public interface IServiceCORSSettings
    {
        string AllowedOrigins { get; }

        string AllowedMethods { get; }

        string AllowedHeaders { get; }

        string ExposedHeaders { get; }
    }
}