using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;

namespace SAHL.Services.Query.Parsers.Elemets
{

    public class FindManyQuery : IFindQuery
    {
        public FindManyQuery()
        {
            Errors = new List<string>();
        }

        public bool IsValid
        {
            get { return Errors.Count == 0; }
        }

        public List<string> Errors { get; set; }
        public ILimitPart Limit { get; set; }
        public List<string> Fields { get; set; }
        public List<string> Includes { get; set; }
        public List<IWherePart> Where { get; set; }
        public List<IOrderPart> OrderBy { get; set; }
        public ISkipPart Skip { get; set; }
        public IPagedPart PagedPart { get; set; }
        public string FullFilterString { get; set; }
    }

}