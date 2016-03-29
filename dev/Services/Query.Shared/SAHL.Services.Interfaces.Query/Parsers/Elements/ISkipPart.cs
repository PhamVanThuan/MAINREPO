namespace SAHL.Services.Interfaces.Query.Parsers.Elements
{
    public interface ISkipPart
    {
        int Skip { get; set; }
        int Take { get; set; }
    }
}