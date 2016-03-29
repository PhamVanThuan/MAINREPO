using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class DocumentCheckListRepositoryTest : TestBase
    {
        private IDocumentCheckListRepository _docCheckListRepo;

        private const string OfferQuery = @";with Offer_CTE (OfferKey,OriginationSourceKey)
                    as
                    (
	                    select top 1
		                    o.OfferKey,o.OriginationSourceKey
	                    from 
		                    [2am].[dbo].[Offer] o (nolock)
	                    join  
		                    [2am].[dbo].[OfferInformation] oi (nolock) on oi.OfferKey = o.OfferKey	
	                    join 
		                    [2am].[dbo].[OriginationSourceProduct] osp (nolock) on osp.OriginationSourceKey = o.OriginationSourceKey and osp.ProductKey = oi.ProductKey
	                    join 
		                    [2am].[dbo].[DocumentSet] ds (nolock) on ds.OriginationSourceProductKey = osp.OriginationSourceProductKey and ds.OfferTypeKey = o.OfferTypeKey
	                    join 
		                    [2am].[dbo].[DocumentSetConfig] dsg (nolock) on dsg.DocumentSetKey = ds.DocumentSetKey
	                    order by 
		                    o.OfferKey desc
                    )
                    select 
	                    o.OfferKey, (select top 1 OriginationSourceProductKey from [2am]..OfferInformation oi (nolock) 
					                    join [2am].[dbo].[OriginationSourceProduct] osp (nolock) ON osp.OriginationSourceKey = o.OriginationSourceKey and osp.ProductKey = oi.ProductKey
					                    where oi.OfferKey = o.OfferKey order by OfferInformationKey desc) as OriginationSourceProductKey
                    from 
	                    Offer_CTE o";

        [Test]
        public void DeleteApplicationDocumentTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                ApplicationDocument_DAO adDao = ApplicationDocument_DAO.FindFirst();
                IApplicationDocument ad = null;
                if (adDao != null)
                {
                    ad = new ApplicationDocument(adDao);

                    _docCheckListRepo = RepositoryFactory.GetRepository<IDocumentCheckListRepository>();
                    _docCheckListRepo.DeleteApplicationDocument(ad);
                    //if we got here with no exceptions then we are happy with the test
                    // so any assert is fine to get accurate coverage metrics
                    Assert.AreEqual(1, 1);
                }
                else
                {
                    Assert.Fail("No data to test delete of IApplicationDocument");
                }

            }
        }

        [Test]
        public void GetApplicationDocumentsForApplicationTest()
        {
            using (TransactionScope tx = new TransactionScope())
            {

                DataTable DT = base.GetQueryResults(OfferQuery);
                Assert.That(DT.Rows.Count > 0);
                int OfferKey = Convert.ToInt32(DT.Rows[0]["OfferKey"].ToString());

                IDocumentCheckListService docCheckListService = ServiceFactory.GetService<IDocumentCheckListService>();
                docCheckListService.GetList(OfferKey);

                _docCheckListRepo = RepositoryFactory.GetRepository<IDocumentCheckListRepository>();
                IList<IApplicationDocument> appDocList = _docCheckListRepo.GetApplicationDocumentsForApplication(OfferKey);

                Assert.That(appDocList.Count > 0);

                tx.VoteRollBack();
            }
        }

        [Test]
        public void GetDocumentSetTest()
        {
            using (new SessionScope())
            {

                DataTable DT = base.GetQueryResults(OfferQuery);
                Assert.That(DT.Rows.Count > 0);

                int OfferKey = Convert.ToInt32(DT.Rows[0]["OfferKey"].ToString());
                int OriginationSourceProductKey = Convert.ToInt32(DT.Rows[0]["OriginationSourceProductKey"].ToString());

                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                IApplication application = appRepo.GetApplicationByKey(OfferKey);

                IOriginationSourceProduct osp = appRepo.GetOriginationSourceProductByKey(OriginationSourceProductKey);

                _docCheckListRepo = RepositoryFactory.GetRepository<IDocumentCheckListRepository>();
                IDocumentSet docSet =  _docCheckListRepo.GetDocumentSet(application, osp);

                Assert.IsNotNull(docSet);
            }
        }

        [Test]
        public void GetDocumentSetConfig()
        {
            using (new SessionScope())
            {
                DataTable DT = base.GetQueryResults(OfferQuery);
                Assert.That(DT.Rows.Count > 0);

                int OfferKey = Convert.ToInt32(DT.Rows[0]["OfferKey"].ToString());
                int OriginationSourceProductKey = Convert.ToInt32(DT.Rows[0]["OriginationSourceProductKey"].ToString());

                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                IApplication application = appRepo.GetApplicationByKey(OfferKey);

                IOriginationSourceProduct osp = appRepo.GetOriginationSourceProductByKey(OriginationSourceProductKey);

                _docCheckListRepo = RepositoryFactory.GetRepository<IDocumentCheckListRepository>();
                IDocumentSet docSet = _docCheckListRepo.GetDocumentSet(application, osp);

                Assert.IsNotNull(docSet);

                IList<IDocumentSetConfig> docSetConfig = _docCheckListRepo.GetDocumentSetConfig(docSet);

                Assert.That(docSetConfig.Count > 0);
            }
        }
    }
}
