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
    public class when_closing_document_for_deletion : WithFakes
    {
        static ICurrentlyOpenDocumentManager manager;
        static ICurrentlyOpenDocumentDataManager dataManager;

        static Guid documentVersionId;

        Establish context = () =>
        {
            dataManager = An<ICurrentlyOpenDocumentDataManager>();
            manager = new CurrentlyOpenDocumentManager(dataManager);
        };

        Because of = () =>
        {
            manager.CloseDocument(documentVersionId);
        };

        It should_close_document = () =>
        {
            dataManager.WasToldTo(x => x.CloseDocument(documentVersionId));
        };
    }
}
