namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IXsdAbstraction
    {
        System.Collections.Generic.List<System.Xml.XmlNode> ComplexNodes { get; }

        System.Collections.Generic.List<string> ComplexTypes { get; }

        System.Collections.Generic.List<string> Elements { get; }

        System.Collections.Generic.List<ISimpleNode> SimpleNodes { get; }

        System.Collections.Generic.List<string> SimpleTypes { get; }
    }
}