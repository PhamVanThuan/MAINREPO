using SAHL.Core.IoC;
using System;

namespace SAHL.Config.Services.Windows.Web
{
    public interface IWebApiService : IStartableService, IStoppableService, IDisposable
    {
    }
}