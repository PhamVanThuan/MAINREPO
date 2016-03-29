using System;
using System.Collections.Generic;
using SAHL.X2Engine2.Tests.X2;
using SAHL.X2Engine2.Tests.X2.Models;
namespace SAHL.X2Engine2.Tests.X2.Services
{
    public interface IX2Maps: IDisposable
    {
        IEnumerable<X2ProcessWorkflow> GetMapNames();
        IEnumerable<X2StateActivity> GetMapInfo ();
    }
}