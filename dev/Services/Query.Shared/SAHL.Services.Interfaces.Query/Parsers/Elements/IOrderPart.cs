namespace SAHL.Services.Interfaces.Query.Parsers.Elements
{
    public interface IOrderPart
    {
        int Sequence { get; set; }
        string Field { get; set; } 
    }
}