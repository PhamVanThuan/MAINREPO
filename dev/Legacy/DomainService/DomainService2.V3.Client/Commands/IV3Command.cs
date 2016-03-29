using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.V3.Client.Commands
{
    public interface IV3Command
    {
        string ADUserName { get; }
        string ToJSON();
    }
}
