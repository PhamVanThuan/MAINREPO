using System.Security.Permissions;
using SAHL.Services.Interfaces.Query.Parsers.Elements;

namespace SAHL.Services.Query.Parsers.Elemets
{
    public class LimitPart : ILimitPart
    {
        public int Count { get; set; }
    }

}