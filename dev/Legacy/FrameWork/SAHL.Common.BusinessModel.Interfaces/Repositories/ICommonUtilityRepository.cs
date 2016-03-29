using System.Data;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ICommonUtilityRepository
    {
        TInterface GetObjectByKey<TInterface, TDAO>(int Key) where TDAO : class;
    }
}
