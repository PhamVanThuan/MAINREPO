using System.Data;

namespace SAHL.Core.Data
{
    public interface ISqlRepository
    {
        void UseConnection(IDbConnection dbConnection);
    }
}