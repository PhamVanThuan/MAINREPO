using SAHL.Core.Data;

namespace SAHL.Core.DomainProcess
{
    public class DomainProcessStartResult : IDomainProcessStartResult
    {
        public DomainProcessStartResult(bool result, IDataModel data)
        {
            this.Result = result;
            this.Data = data;
        }

        public bool Result { get; private set; }

        public IDataModel Data { get; private set; }
    }
}