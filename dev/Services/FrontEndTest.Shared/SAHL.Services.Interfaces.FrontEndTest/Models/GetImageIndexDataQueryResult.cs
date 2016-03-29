namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetImageIndexDataQueryResult
    {
        public GetImageIndexDataQueryResult(int id, int stor, string guid)
        {
            this.Id = id;
            this.Stor = stor;
            this.Guid = guid;
        }

        public int Id { get; set; }

        public int Stor { get; set; }

        public string Guid { get; set; }
    }
}