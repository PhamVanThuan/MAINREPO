using SAHL.Core.Services;

namespace SAHL.Config.Core.Specs
{
    public class FakeCommand : ServiceCommand, IRequiresFake1
    {
        public int Key { get; private set; }
    }
}