namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface ISimpleNode
    {
        System.Collections.Generic.List<string> Enumerations { get; }

        string Name { get; set; }

        string Type { get; set; }
    }
}