namespace SAHL.WCFServices.ComcorpConnector.Managers.MacAuthentication
{
    public interface IMacAuthenticationManager
    {
        bool AuthenticateMessage<T>(T requestMessage, string requestMac);
    }
}