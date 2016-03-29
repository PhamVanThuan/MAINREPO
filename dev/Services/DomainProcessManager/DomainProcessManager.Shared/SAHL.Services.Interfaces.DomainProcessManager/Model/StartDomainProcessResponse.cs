using SAHL.Core.Data;
using SAHL.DomainProcessManager.Models;
using System;
using System.Runtime.Serialization;

namespace SAHL.Services.Interfaces.DomainProcessManager
{
    [Serializable]
    [KnownType(typeof(ApplicationCreationReturnDataModel))]
    public class StartDomainProcessResponse : IStartDomainProcessResponse
    {
        public StartDomainProcessResponse(bool result, IDataModel data)
        {
            this.Result = result;
            this.Data = data;
        }

        public bool Result { get; private set; }

        public IDataModel Data { get; private set; }
    }
}