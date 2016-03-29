using System;

namespace SAHL.Core.Services
{
    public interface IQueryParameterManager
    {
        T GetParameter<T>() where T : IQueryParameter;

        void SetParameter<T>(Action<T> parameterToSet) where T : IQueryParameter;
    }
}