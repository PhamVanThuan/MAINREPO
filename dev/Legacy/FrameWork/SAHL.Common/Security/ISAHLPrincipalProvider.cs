namespace SAHL.Common.Security
{
    public interface ISAHLPrincipalProvider
    {
        SAHLPrincipal GetCurrent();
    }
}