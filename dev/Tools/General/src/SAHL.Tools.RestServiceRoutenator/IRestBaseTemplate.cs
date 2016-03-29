namespace SAHL.Tools.RestServiceRoutenator
{
    public interface IRestBaseTemplate
    {
        IScanResult Result { get; }
        
        string Process(IScanResult result);
    }
}