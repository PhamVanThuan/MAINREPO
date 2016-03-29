using Machine.Specifications;
using Microsoft.Exchange.WebServices.Data;
using System;

namespace SAHL.Core.Exchange.Specs.ExchangeProviderSpecs
{
    public class when_creating_a_mailbox_folder : WithExchangeProviderFakes
    {
        private static string folderName;
        private static FolderId folderId;

        private Establish context = () =>
            {
                string base64Guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                folderName = string.Format("HaloUserFolder{0}", base64Guid);
                exchangeProvider.OpenMailboxConnection();
            };

        private Because of = () =>
            {
                exchangeProvider.CreateFolderOffInbox(folderName);
                folderId = exchangeProvider.ExchangeService
                    .FindFolders(WellKnownFolderName.Inbox, new SearchFilter.ContainsSubstring(FolderSchema.DisplayName, folderName), new FolderView(1)).Folders[0].Id;
            };

        private It should_create_the_folder = () =>
            {
                folderId.ShouldNotBeNull();
            };

        private Cleanup remove_folder = () =>
            {
                exchangeProvider.DeleteFolderOffInbox(folderName);
            };
    }
}