using System;

namespace SAHL.Core.Data.Context
{
    public interface IDbContext : IReadWriteSqlRepository, IDisposable
    {
        void Complete();
    }
}