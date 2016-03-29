using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2CommandRequest : X2RequestBase
    {
        public string CommandName;
        public string CommandArgs;
        public string SessionID;

        public X2CommandRequest(string CommandName, string CommandArgs, string SessionID)
            : base(false, null)
        {
            this.CommandName = CommandName;
            this.CommandArgs = CommandArgs;
            this.SessionID = SessionID;
            this.m_RequestType = RequestType.CommandRequest;
        }
    }
}