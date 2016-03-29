using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.SqlServer.Server;

namespace SAHL.Core.Exchange.Provider
{
    public class ExchangeProvider : IExchangeProvider
    {
        public ExchangeService ExchangeService { get; set; }

        public IExchangeProviderCredentials ExchangeProviderCredentials { get; protected set; }

        public ExchangeProvider(ExchangeService exchangeService, IExchangeProviderCredentials exchangeProviderCredentials)
        {
            ExchangeService = exchangeService;
            this.ExchangeProviderCredentials = exchangeProviderCredentials;
        }
        
        public bool OpenMailboxConnection()
        {
            try
            {
                ExchangeService.Credentials = new WebCredentials(ExchangeProviderCredentials.UserName, ExchangeProviderCredentials.Password);
                ExchangeService.AutodiscoverUrl(ExchangeProviderCredentials.MailBox);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public List<IMailMessage> FetchPageOfMessagesFromInbox(int pageSize, int pageNumber)
        {
            ItemView itemView = new ItemView(pageSize);
            itemView.Offset = pageNumber > 1 ? (pageNumber - 1) * pageSize : itemView.Offset;
            FindItemsResults<Item> messages = ExchangeService.FindItems(WellKnownFolderName.Inbox, itemView);
            return MapMessages(messages);
        }

        public List<IMailMessage> FetchAllMessagesFromInbox()
        {
            int offset = 0;
            int pageSize = 50;
            bool hasMoreMessages = true;
            ItemView view = new ItemView(pageSize, offset, OffsetBasePoint.Beginning);

            view.PropertySet = PropertySet.IdOnly;
            FindItemsResults<Item> findResults;
            List<IMailMessage> emails = new List<IMailMessage>();

            while (hasMoreMessages)
            {
                findResults = ExchangeService.FindItems(WellKnownFolderName.Inbox, view);
                emails.AddRange(MapMessages(findResults));
                hasMoreMessages = findResults.MoreAvailable;
                if (hasMoreMessages)
                {
                    view.Offset += pageSize;
                }
            }
            return emails;
        }

        public bool MoveMailMessageToFolder(IMailMessage mailMessage, string folderName)
        {

            try
            {
                FolderId folderId = FindFolderOffInBox(folderName);

                if (folderId != null)
                {
                    var itemsToMove = PrepareItemsToMove(mailMessage);
                    return MoveMailMessageToFolder(itemsToMove, folderId);
                }

                return false;
            }
            catch
            {
                throw;
            }

        }

        public bool SendMessage(IMailMessage mailMessage)
        {
            try
            {
                EmailMessage email = new EmailMessage(ExchangeService);
                email.ToRecipients.Add(mailMessage.To);
                email.Subject = mailMessage.Subject;
                email.Body = new MessageBody(mailMessage.Body);
                if (mailMessage.Attachments.Count > 0)
                {
                    foreach (var attachment in mailMessage.Attachments)
                    {
                        email.Attachments.AddFileAttachment(string.Format("{0}.{1}", attachment.AttachmentName, attachment.AttachmentType), Convert.FromBase64String(attachment.ContentAsBase64));
                    }
                }
                email.Send();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public FolderId CreateFolderOffInbox(string folderName)
        {
            try
            {
                Folder newFolder = new Folder(ExchangeService);
                newFolder.DisplayName = folderName;
                newFolder.Save(WellKnownFolderName.Inbox);
                return newFolder.Id;
            }
            catch
            {
                throw;
            }
        }
        public void DeleteFolderOffInbox(string folderName)
        {
            try
            {
                FolderId folderId = FindFolderOffInBox(folderName);
                Folder folder = Folder.Bind(ExchangeService, folderId);
                folder.Delete(DeleteMode.HardDelete);
            }
            catch
            {
                throw;
            }
        }
        private bool MoveMailMessageToFolder(List<ItemId> itemsToMove, FolderId folderId)
        {
            var itemsMoved = ExchangeService.MoveItems(itemsToMove, folderId);
            if (itemsMoved.Count > 0)
            {
                return true;
            }
            return false;
        }

        private static List<ItemId> PrepareItemsToMove(IMailMessage mailMessage)
        {
            ItemId messageId = new ItemId(mailMessage.UniqueExchangeId);
            List<ItemId> itemsToMove = new List<ItemId>();
            itemsToMove.Add(messageId);
            return itemsToMove;
        }

        private FolderId FindFolderOffInBox(string folderName)
        {
            
            FolderView folderView = new FolderView(1);
            var folderResults = ExchangeService.FindFolders(WellKnownFolderName.Inbox, new SearchFilter.ContainsSubstring(FolderSchema.DisplayName, folderName), folderView);

            if (folderResults.TotalCount > 0)
            {
                return folderResults.Folders[0].Id;    
            }
            else
            {
                return CreateFolderOffInbox(folderName);
            }
            
        }

        private List<IMailMessage> MapMessages(FindItemsResults<Item> itemsResults)
        {
            List<IMailMessage> items = new List<IMailMessage>();

            foreach (Item item in itemsResults)
            {

                EmailMessage emailMessage = item as EmailMessage;
                if (emailMessage != null)
                {
                    items.Add(CreateMailMessage(emailMessage));
                }
            }
            
            return items;

        }

        private MailMessage CreateMailMessage(EmailMessage emailMessage)
        {
            emailMessage.Load();
            MailMessage mailMessage = new MailMessage();
            mailMessage.UniqueExchangeId = emailMessage.Id.UniqueId;
            mailMessage.Body = emailMessage.Body.Text;
            mailMessage.Subject = emailMessage.Subject;
            mailMessage.DateRecieved = emailMessage.DateTimeReceived;
            mailMessage.From = emailMessage.From.Address ?? emailMessage.From.Name;

            if (emailMessage.HasAttachments)
            {
                AddAllAttachments(emailMessage, mailMessage);
            }

            return mailMessage;

        }

        private void AddAllAttachments(EmailMessage emailMessage, MailMessage mailMessage)
        {
            foreach (Attachment attachment in emailMessage.Attachments)
            {
                var fileAttachment = attachment as FileAttachment;
                if (fileAttachment != null)
                {
                    AddSingleAttachment(fileAttachment, mailMessage);
                }
            }
        }

        private void AddSingleAttachment(FileAttachment fileAttachment, MailMessage mailMessage)
        {
            fileAttachment.Load();
            IMailAttachment mailAttachment = new MailAttachment();
            mailAttachment.ContentAsBase64 = Convert.ToBase64String(fileAttachment.Content);
            mailAttachment.AttachmentName = fileAttachment.Name;
            mailAttachment.AttachmentType = FindAttachementType(fileAttachment.Name);
            mailMessage.Attachments.Add(mailAttachment);
        }

        private string FindAttachementType(string fullFileName)
        {
            return Path.GetExtension(fullFileName).ToLower();   
        }

    }
}