using System.Collections.Generic;

namespace SAHL.Core.Communication
{
    public interface IUserCommunicationService
    {
        void SendUserEmail(string emailAddress, string templateName, Dictionary<string, string> templateParameters);
    }
}