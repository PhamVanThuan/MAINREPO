using Microsoft.AspNet.SignalR;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.IoC;
using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument;
using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign
{
    public class LockScheduleService : IStartable
    {
        private const string usernameFormat = "SAHL\\{0}";

        private int lockCheckDelay = 5000;
        private int lockTimeoutInMinutes = 25;
        private int removelockTimeoutInMinutes = 35;


        private ICurrentlyOpenDocumentManager dataManager;
        private IHubContext scheduleHub;
        private IEnumerable<OpenDocumentsView> openDocuments;
        private HashSet<OpenDocumentsView> documentsToNotify = new HashSet<OpenDocumentsView>();

        public LockScheduleService(ICurrentlyOpenDocumentManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void Start()
        {
            scheduleHub = GlobalHost.ConnectionManager.GetHubContext<LockScheduleHub>();
            Init();
        }

        private void Init()
        {
            var task = Task.Factory.StartNew(async () =>
            {
                openDocuments = this.dataManager.GetAllOpenDocuments();
                while (true)
                {
                    await Task.Delay(lockCheckDelay);
                    IEnumerable<OpenDocumentsView> currentlyOpenedTrees = this.dataManager.GetAllOpenDocuments();

                    IEnumerable<OpenDocumentsView> closed = GetDocumentsThatHaveClosed(currentlyOpenedTrees);
                    IEnumerable<OpenDocumentsView> opened = GetDocumentsThatHaveOpened(currentlyOpenedTrees);

                    if (closed.Count() > 0)
                        scheduleHub.Clients.All.onRemovedLocks(closed);
                    await Task.Delay(1000);
                    if (opened.Count() > 0)
                        scheduleHub.Clients.All.onAddedLocks(opened);

                    this.openDocuments = currentlyOpenedTrees;
                }
            }, TaskCreationOptions.LongRunning);
        }

        private IEnumerable<OpenDocumentsView> GetDocumentsThatHaveClosed(IEnumerable<OpenDocumentsView> currentlyOpenedDocuments)
        {
            return GetTimedoutDocuments(currentlyOpenedDocuments).Concat(openDocuments.Where(x => !currentlyOpenedDocuments.Any(y => y.Id == x.Id)));
        }

        private IEnumerable<OpenDocumentsView> GetDocumentsThatHaveOpened(IEnumerable<OpenDocumentsView> currentlyOpenedDocuments)
        {
            return currentlyOpenedDocuments.Where(x => !openDocuments.Any(y => y.Id == x.Id));
        }

        private IEnumerable<OpenDocumentsView> GetTimedoutDocuments(IEnumerable<OpenDocumentsView> currentlyOpenedDocuments)
        {
            DateTime now = DateTime.Now;
            IEnumerable<OpenDocumentsView> documentsToBeNotified = currentlyOpenedDocuments.Where(x => (now - x.OpenDate).TotalMinutes >= lockTimeoutInMinutes && !documentsToNotify.Any(y => y.Id == x.Id)).ToArray();
            IEnumerable<OpenDocumentsView> documentToClose = currentlyOpenedDocuments.Where(x => (now - x.OpenDate).TotalMinutes >= removelockTimeoutInMinutes && documentsToNotify.Any(y => y.Id == x.Id)).ToArray();
            IEnumerable<OpenDocumentsView> documentsReopened = currentlyOpenedDocuments.Where(x => (now - x.OpenDate).TotalMinutes < lockTimeoutInMinutes && documentsToNotify.Any(y => y.Id == x.Id)).Concat(documentsToNotify.Where(x => !currentlyOpenedDocuments.Any(y => y.Id == x.Id))).ToArray();

            foreach (OpenDocumentsView docs in documentsReopened)
            {
                documentsToNotify.RemoveWhere(x => x.Id == docs.Id);
            }

            foreach (OpenDocumentsView openDocument in documentToClose)
            {
                dataManager.CloseDocument(openDocument.DocumentVersionId);
                documentsToNotify.RemoveWhere(x => x.Id == openDocument.Id);
            }

            foreach (OpenDocumentsView openDocument in documentsToBeNotified)
            {
                string userId = String.Format(usernameFormat, openDocument.Username);
                scheduleHub.Clients.User(userId).onPendingLockRelease(openDocument);
                documentsToNotify.Add(openDocument);
            }

            return documentToClose;
        }
    }
}
