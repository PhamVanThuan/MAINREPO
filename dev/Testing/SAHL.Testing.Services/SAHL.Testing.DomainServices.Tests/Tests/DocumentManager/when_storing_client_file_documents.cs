using NUnit.Framework;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DocumentManager
{
    [TestFixture]
    public class when_storing_client_file_documents : ServiceTestBase<IDocumentManagerServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetUnusedIDNumberQuery();
            PerformQuery(query);
            var key1_idNumber = query.Result.Results.First().IDNumber;
            var category1 = "Identity Documents";
            var category2 = "Marriage certificate";
            var key5_firstName = "Test";
            var key6_surname = "User";
            var key8_username = "SAHL\\JessicaV";
            var extension = FileExtension.Tiff;
            var image = Convert.ToBase64String(File.ReadAllBytes("Tests\\DocumentManager\\SampleTiff\\ComcorpSampleDoc.tiff"));
            var document1 = new ClientFileDocumentModel(image, category1, key1_idNumber, key5_firstName, key6_surname, key8_username, DateTime.Now, extension);
            var document2 = new ClientFileDocumentModel(image, category2, key1_idNumber, key5_firstName, key6_surname, key8_username, DateTime.Now, extension);
            var documentModels = new List<ClientFileDocumentModel> { document1, document2 };
            var command = new StoreClientFileDocumentsCommand(documentModels);
            var originalFileName1 = string.Format("{0} - {1}", key1_idNumber, category1);
            var originalFileName2 = string.Format("{0} - {1}", key1_idNumber, category2);
            Execute(command).WithoutErrors();
            IEnumerable<DataDataModel> clientFileDocuments = TestApiClient.Get<DataDataModel>(new { key1 = key1_idNumber });
            Assert.That(clientFileDocuments.Where(x => x.Key1 == key1_idNumber && x.OriginalFilename.Contains(originalFileName1)
                && x.Key4 == category1 && x.Key5 == key5_firstName && x.Key6 == key6_surname).First() != null);
            Assert.That(clientFileDocuments.Where(x => x.Key1 == key1_idNumber && x.OriginalFilename.Contains(originalFileName2)
                && x.Key4 == category2 && x.Key5 == key5_firstName && x.Key6 == key6_surname).First() != null);
        }
    }
}