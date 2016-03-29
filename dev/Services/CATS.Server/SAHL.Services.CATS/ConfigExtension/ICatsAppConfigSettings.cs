namespace SAHL.Services.CATS.ConfigExtension
{
    public interface ICatsAppConfigSettings
    {
        string CATSInputFileLocation { get; }
        string CATSOutputFileLocation { get; }
        string CATSFailureFileLocation { get; }
        string CATSSuccessFileLocation { get; }
        string CATSArchiveFileLocation { get; }
    }
}
