using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class NoteRepositoryTest : TestBase
    {
        [SetUp]
        public void Setup()
        {
            // set the strategy to default so we actually go to the database
            SetRepositoryStrategy(TypeFactoryStrategy.Default);
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            MockCache.Flush();
        }

        [Test]
        public void CreateEmptyNote()
        {
            using (new SessionScope())
            {
                INoteRepository repo = RepositoryFactory.GetRepository<INoteRepository>();
                INote note = repo.CreateEmptyNote();
                Assert.IsNotNull(note);
            }
        }

        [Test]
        public void CreateEmptyNoteDetail()
        {
            using (new SessionScope())
            {
                INoteRepository repo = RepositoryFactory.GetRepository<INoteRepository>();
                INoteDetail noteDetail = repo.CreateEmptyNoteDetail();
                Assert.IsNotNull(noteDetail);
            }
        }

        [Test]
        public void GetNoteByKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from [2am].dbo.Note (nolock)";
                DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count != 1)
                    Assert.Ignore("No data found for test.");

                int noteKey = Convert.ToInt32(DT.Rows[0]["NoteKey"]);
                INoteRepository repo = RepositoryFactory.GetRepository<INoteRepository>();
                INote note = repo.GetNoteByKey(noteKey);

                Assert.That(note.Key == noteKey);
            }
        }

        [Test]
        public void GetNoteDetailByKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from [2am].dbo.NoteDetail (nolock)";
                DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count != 1)
                    Assert.Ignore("No data found for test.");

                int noteDetailKey = Convert.ToInt32(DT.Rows[0]["NoteDetailKey"]);

                INoteRepository repo = RepositoryFactory.GetRepository<INoteRepository>();
                INoteDetail noteDetail = repo.GetNoteDetailByKey(noteDetailKey);

                Assert.That(noteDetail.Key == noteDetailKey);
            }
        }

        [Test]
        public void GetNoteDetailsByGenericKeyAndType()
        {
            using (new SessionScope())
            {
                string query = "select top 1 n.GenericKey, n.GenericKeyTypeKey, count(nd.NoteDetailKey) as 'DetailCount' from [2am]..Note n (nolock) join [2am].dbo.NoteDetail nd (nolock) on n.NoteKey = nd.NoteKey group by n.GenericKey, n.GenericKeyTypeKey";
                DataTable DT = base.GetQueryResults(query);
                if (DT.Rows.Count != 1)
                    Assert.Ignore("No data found for test.");

                int genericKey = Convert.ToInt32(DT.Rows[0]["GenericKey"]);
                int genericKeyTypeKey = Convert.ToInt32(DT.Rows[0]["GenericKeyTypeKey"]);
                int noteDetailCount = Convert.ToInt32(DT.Rows[0]["DetailCount"]);

                INoteRepository repo = RepositoryFactory.GetRepository<INoteRepository>();

                // get the note details
                List<INoteDetail> noteDetails = repo.GetNoteDetailsByGenericKeyAndType(genericKey, genericKeyTypeKey);
                Assert.IsTrue(noteDetails.Count == noteDetailCount);
            }
        }
    }
}