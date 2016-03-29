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
    [Subject("SAHL.Services.DecisionTreeDesign.Services.CurrentlyOpenTreeDataManager.CurrentlyOpenDocumentManager.CloseDocument")]
    public class when_closing_a_document_when_opened_by_different_user : WithFakes
    {
        static ICurrentlyOpenDocumentDataManager currentlyOpenDocumentDataManager;
        static ICurrentlyOpenDocumentManager currentlyOpenDocumentManager;

        static Guid documentVersionId;
        static string username;

        Establish context = () =>
        {
            username = "testUsername";
            currentlyOpenDocumentDataManager = An<ICurrentlyOpenDocumentDataManager>();
            currentlyOpenDocumentDataManager.WhenToldTo(x => x.IsDocumentOpen(documentVersionId)).Return(true);
            currentlyOpenDocumentManager = new CurrentlyOpenDocumentManager(currentlyOpenDocumentDataManager);
        };

        Because of = () =>
        {
            currentlyOpenDocumentManager.CloseDocument(documentVersionId, username);
        };

        It should_check_if_document_is_opened = () =>
        {
            currentlyOpenDocumentDataManager.WasToldTo(x => x.IsDocumentOpen(documentVersionId));
        };

        It should_check_if_document_is_opened_by_user = () =>
        {
            currentlyOpenDocumentDataManager.WasToldTo(x => x.IsDocumentOpenByUser(documentVersionId, username));
        };

        It should_not_close_document_for_user = () =>
        {
            currentlyOpenDocumentDataManager.WasNotToldTo(x => x.CloseDocument(documentVersionId, username));
        };
    }
}
