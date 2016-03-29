using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetFileUsingPathQuery : ServiceQuery<string>, IFrontEndTestQuery, IServiceQuery
    {
        public string fullpath { get; set; }

        public GetFileUsingPathQuery(string fullpath)
        {
            this.fullpath = fullpath;
        }
    }
}
