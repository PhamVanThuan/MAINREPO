using System;
using System.Collections.Generic;
using System.Text;

namespace EWorkConnector
{
    interface IeWorkTransactionProtocolEngine
    {
        string SendRequest(string Request);
    }
}
