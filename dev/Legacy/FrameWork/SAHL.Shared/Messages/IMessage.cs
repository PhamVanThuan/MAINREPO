using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Shared.Messages
{
    public interface IMessage
    {
        int Id { get; set; }

        DateTime MessageDate { get; }

        string MachineName { get; }

        string Application { get; }

        string User { get; }

        string Source { get; }

        IDictionary<string, string> Parameters { get; }
    }
}
