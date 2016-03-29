using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DocumentManager.Utils.DataStore;
using SAHL.Services.Interfaces.DocumentManager.Models;

namespace SAHL.Services.DocumentManager.Specs.Managers.DataStore
{
    public class when_getting_a_client_files_document_name : WithFakes
    {
        private static DataStoreUtils utils;
        private static ClientFileDocumentModel document;
        private static DateTime date;
        private static string result;
        private static string expectedName;

        private Establish context = () =>
        {
            utils = new DataStoreUtils();
            date = new DateTime(2014, 08, 26, 10, 36, 40);
            document = new ClientFileDocumentModel("123456123", "Identity Documents", "1234567890132", "Bob", "Builder", "Zorro", DateTime.Now, FileExtension.Pdf);
            expectedName = "1234567890132 - Identity Documents - 2014-08-26 103640";
        };

        private Because of = () =>
        {
            result = utils.GetFileNameForClientFileDocument(document, date);
        };

        private It should_return_a_name_using_the_key_combination_and_current_date_time = () =>
        {
            result.ShouldEqual(expectedName);
        };
    }
}