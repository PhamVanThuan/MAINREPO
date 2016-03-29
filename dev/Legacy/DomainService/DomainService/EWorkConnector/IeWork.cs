namespace EWorkConnector
{
    public interface IeWork
    {
        string CancelAction(string SessionID, string FolderID, string ActionName);

        string CreateFolder(string SessionID, string Map, System.Collections.Generic.Dictionary<string, string> Vars, string ChangedField);

        string InvokeAndSubmitAction(string SessionID, string FolderID, string ActionName, System.Collections.Generic.Dictionary<string, string> Vars, string ChangedField);

        string LogIn(string Username);

        string LogOut(string SessionID);
    }
}