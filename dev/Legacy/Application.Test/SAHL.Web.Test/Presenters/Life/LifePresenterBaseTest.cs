using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Factories;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Web.Test.Presenters.Common;
using SAHL.Web.Test.Presenters.Banking;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Test.Presenters.Life
{

    public class LifePresenterBaseTest : TestViewBase
    {


        #region Protected Helpers

        protected void SetupLifeOfferCBONode()
        {
            Type T = typeof(BankingDetailsDisplayPresenter);
            string Query = base.GetSQLResource(T, "GetLifeOffer.sql");
            int BusinessKey = (int)base.DBHelper.ExecuteScalar(Query);

            ICBOService CBO = ServiceFactory.GetService<ICBOService>();
            IDomainMessageCollection Messages = new DomainMessageCollection();
            InstanceNode IN = new InstanceNode(0, null, "A Sample Life Offer Node", "A Sample Life Offer Node", BusinessKey,null);
            CBO.AddCBOMenuNode(TestPrincipal, null, IN, CBONodeSet.X2NODESET);
            CBO.SetCurrentCBONode(TestPrincipal, IN, CBONodeSet.X2NODESET);
        }

        protected void SetupLifeMocks()
        {
            CacheManager CM = CacheFactory.GetCacheManager("MOCK");
            // setup the cbosevice
            ICBOService CBO = base.GetMock<ICBOService>();
            InstanceNode IN = new InstanceNode(0, null, "A Sample Life Offer Node", "A Sample Life Offer Node", 1000,null);
            Expect.Call(CBO.GetCurrentCBONode(base.TestPrincipal, CBONodeSet.X2NODESET)).IgnoreArguments().Return(IN);
            
            // setup an offer repository
            IApplicationRepository OR = _mockery.CreateMock<IApplicationRepository>();
            CM.Add(typeof(IApplicationRepository).ToString(), OR);
            // setup the data returned by a call to GetOfferByKey
            IApplication O = _mockery.CreateMock<IApplication>();
            SAHL.Common.BusinessModel.Interfaces.IAccount A = _mockery.CreateMock<SAHL.Common.BusinessModel.Interfaces.IAccount>();
            SetupResult.For(O.Account).Return(A);
            SetupResult.For(A.Key).Return(1000);
            IReadOnlyEventList<ILegalEntityNaturalPerson> lst = new ReadOnlyEventList<ILegalEntityNaturalPerson>();
            SetupResult.For(A.GetNaturalPersonLegalEntitiesByRoleType(null, new int[3] { 1, 2, 3 })).IgnoreArguments().Return(lst);
            Expect.Call(OR.GetApplicationByKey(1)).IgnoreArguments().Return(O);
        }
        #endregion
    }
}
