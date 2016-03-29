using System.Collections.Generic;
using Microsoft.Exchange.WebServices.Data;

namespace SAHL.Core.Exchange
{
    public interface IExchangeProvider
    {
        IExchangeProviderCredentials ExchangeProviderCredentials { get; }
        ExchangeService ExchangeService { get; set; }
        List<IMailMessage> FetchPageOfMessagesFromInbox(int pageSize, int pageNumber);
        List<IMailMessage> FetchAllMessagesFromInbox();
        bool OpenMailboxConnection();
        bool MoveMailMessageToFolder(IMailMessage mailMessage, string folderName);
        bool SendMessage(IMailMessage mailMessage);
        FolderId CreateFolderOffInbox(string folderName);
        void DeleteFolderOffInbox(string folderName);
    }
}