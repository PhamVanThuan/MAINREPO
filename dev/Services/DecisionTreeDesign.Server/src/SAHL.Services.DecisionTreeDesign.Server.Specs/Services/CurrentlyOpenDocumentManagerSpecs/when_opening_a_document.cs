using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Server.Specs.Services.CurrentlyOpenDocumentManagerSpecs
{
    [Subject("SAHL.Services.DecisionTreeDesign.Services.CurrentlyOpenTreeDataManager.CurrentlyOpenDocumentManager.OpenDocument")]
    public class when_opening_a_document : WithFakes
    {
        static ICurrentlyOpenDocumentDataManager currentlyOpenDocumentDataManager;
        static ICurrentlyOpenDocumentManager currentlyOpenDocumentManager;

        static Guid documentVersionId;
        static Guid documentTypeId;
        static string username;

        Establish context = () =>
        {
            username = "testUsername";
            currentlyOpenDocumentDataManager = An<ICurrentlyOpenDocumentDataManager>();
            currentlyOpenDocumentDataManager.WhenToldTo(x => x.IsDocumentOpen(documentVersionId)).Return(false);
            currentlyOpenDocumentManager = new CurrentlyOpenDocumentManager(currentlyOpenDocumentDataManager);
        };

        Because of = () =>
        {
            currentlyOpenDocumentManager.OpenDocument(documentVersionId, documentTypeId, username);
        };

        It should_check_if_document_is_opened = () =>
        {
            currentlyOpenDocumentDataManager.WasToldTo(x => x.IsDocumentOpen(documentVersionId));
        };

        It should_not_check_if_document_is_opened_by_user = () =>
        {
            currentlyOpenDocumentDataManager.WasNotToldTo(x => x.IsDocumentOpenByUser(documentVersionId,username));
        };

        It should_open_document = () =>
        {
            currentlyOpenDocumentDataManager.WasToldTo(x => x.OpenDocument(documentVersionId, documentTypeId, username));
        };

        It should_not_update_document_open_date = () =>
        {
            currentlyOpenDocumentDataManager.WasNotToldTo(x => x.UpdateDocumentOpenDate(documentVersionId, username));
        };
    }
}
