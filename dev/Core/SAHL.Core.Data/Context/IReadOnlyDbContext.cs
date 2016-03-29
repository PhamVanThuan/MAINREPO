using System;

namespace SAHL.Core.Data.Context
{
    public interface IReadOnlyDbContext : IReadOnlySqlRepository, IDisposable
    {
        void Complete();
    }
}