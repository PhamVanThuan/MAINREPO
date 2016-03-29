namespace SAHL.Common.Security
{
    public class SAHLPrincipalProvider : ISAHLPrincipalProvider
    {
        public SAHLPrincipal GetCurrent()
        {
            return SAHLPrincipal.GetCurrent();
        }
    }
}