using SAHL.Core.Services;

namespace SAHL.Core.Web.Tests
{
    public interface IFakeTestCommand : IServiceCommand
    {
        string ResultMessage { get; set; }
    }
}