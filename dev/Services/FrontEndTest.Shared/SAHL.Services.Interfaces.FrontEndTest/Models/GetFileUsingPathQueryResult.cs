namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetFileUsingPathQueryResult
    {
        public string fullpath { get; set; }

        public GetFileUsingPathQueryResult(string fullpath)
        {
            this.fullpath = fullpath;
        }
    }
}