using SAHL.Core;
using SAHL.Services.Interfaces.Query.Parsers.Elements;

namespace SAHL.Services.Query.Parsers.Elemets
{
    public class SkipPart : ISkipPart
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}