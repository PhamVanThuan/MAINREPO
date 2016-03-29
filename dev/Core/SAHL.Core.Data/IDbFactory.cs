namespace SAHL.Core.Data
{
    public interface IDbFactory
    {
        IDb NewDb();
    }
}