using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.Service.Test
{
    [TestFixture]
    public class DocumentCheckListServiceTest : TestBase
    {
        [Test]
        public void GetListTest_When_All_Application_Documents_Receieved()
        {

            var query = @"      select top 1 o.OfferKey, count(*) as [Required], count(DocumentReceivedDate) as [Receieved]
                                from OfferDocument od (nolock)
                                join Offer o (nolock) on o.OfferKey = od.OfferKey
                                where o.OfferStatusKey = 1 
                                group by o.OfferKey
                                having count(*) = count(DocumentReceivedDate)
                                order by o.OfferKey desc";

            using (new SessionScope(FlushAction.Never))
            {
                var data = base.GetQueryResults(query);

                int applicationWithNoOutstandingDocs = Convert.ToInt32(data.Rows[0][0]);

                int requiredDocs = Convert.ToInt32(data.Rows[0][1]);

                int received = Convert.ToInt32(data.Rows[0][2]);

                IDocumentCheckListService documentCheckListService = ServiceFactory.GetService<IDocumentCheckListService>();

                IDomainMessageCollection dmc = CurrentPrincipalCache.DomainMessages;

                var docs = documentCheckListService.GetList(applicationWithNoOutstandingDocs);

                Assert.Greater(docs.Count, 0);
            }

        }

        [Test]
        public void GetListTest_When_Application_Has_Documents_Outstanding()
        {
            var query = @"      select top 1 o.OfferKey, count(*) as [Required], count(DocumentReceivedDate) as [Receieved]
                                from OfferDocument od (nolock)
                                join Offer o (nolock) on o.OfferKey = od.OfferKey
                                where o.OfferStatusKey = 1 
                                group by o.OfferKey
                                having count(*) > count(DocumentReceivedDate)
                                order by o.OfferKey desc";

            

            using (new SessionScope(FlushAction.Never))
            {
                var data = base.GetQueryResults(query);

                int applicationWithOutstandingDocs = Convert.ToInt32(data.Rows[0][0]);

                int requiredDocs = Convert.ToInt32(data.Rows[0][1]);

                int received = Convert.ToInt32(data.Rows[0][2]);

                IDocumentCheckListService documentCheckListService = ServiceFactory.GetService<IDocumentCheckListService>();

                IDomainMessageCollection dmc = CurrentPrincipalCache.DomainMessages;

                var docs = documentCheckListService.GetList(applicationWithOutstandingDocs);

                Assert.Greater(docs.Count, 0);
            }

        }

    }
}
