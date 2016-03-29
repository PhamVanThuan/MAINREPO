namespace SAHL.Tools.Capitec.CSJsonifier
{
    public interface IBaseTemplate
    {
        IScanResult Result { get; }
        
        string Process(IScanResult result);
    }
}