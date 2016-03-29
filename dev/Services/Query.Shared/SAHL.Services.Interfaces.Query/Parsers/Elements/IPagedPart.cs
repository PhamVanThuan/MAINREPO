namespace SAHL.Services.Interfaces.Query.Parsers.Elements
{
    public interface IPagedPart
    {
        int PageSize { get; set; } 
        int CurrentPage { get; set; } 
    }
}