using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Query
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DoesNotRequireAnIdProperty : Attribute
    {
    }
}
