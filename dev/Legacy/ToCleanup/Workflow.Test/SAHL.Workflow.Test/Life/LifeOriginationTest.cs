using System;
using System.Collections.Generic;
using System.Text;
//using NUnit.Core;
using SAHL.Test;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using Rhino.Mocks;
using SAHL.X2.Framework.DataAccess;
//using SAHL.Common.DataAccess;
using NUnit.Framework;
using SAHL.Common.BusinessModel;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using System.Collections;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Workflow.Test.Life
{
  public class DummyX2DataTran : IActiveDataTransaction
  {
    #region IActiveDataTransaction Members

    public void BeginTransaction()
    {
    }

    public void CommitTransaction()
    {
    }

    public ITransactionContext Context
    {
        get { return null;}// new TransactionContext() as ITransactionContext; }
    }

    public void RollBackTransaction()
    {
    }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
    }

    #endregion
  }

  [TestFixture]
  public class LifeOriginationTest : TestBase
  {
    #region Setup/TearDown
    int OfferKey = -1;
    Int64 InstanceID = -1;
    IActiveDataTransaction Tran = null;
    SAHL.Life.Helper.Life life = null;
    int PolicyNumber = 9;

    [SetUp]
    public void FixtureSetup()
    {
      Tran = new DummyX2DataTran();
      life = new SAHL.Life.Helper.Life();
    }

    [TearDown]
    public void FixtureTearDown()
    {
    }

    #endregion

    

    [NUnit.Framework.Test]
    public void ExtCreateActivity_WithAssuredLives()
    {
      SetRepositoryStrategy(TypeFactoryStrategy.Mock);
      ClearMockCache();
      IAccount A = null;
      IApplication O = null;
      SetupLifeMocks(ref O, ref A);

      IReadOnlyEventList<ILegalEntityNaturalPerson> lst = _mockery.CreateMock<IReadOnlyEventList<ILegalEntityNaturalPerson>>();
      SetupResult.For(lst.Count).Return(1);
      SetupResult.For(A.GetNaturalPersonLegalEntitiesByRoleType(null, new int[3] { 1, 2, 3 })).IgnoreArguments().Return(lst);
      

      SetupResult.For(A.GetLegalName(LegalNameFormat.Full)).IgnoreArguments().Return("blab;a");
      _mockery.ReplayAll();

      string Name = "";
      string Subject = "";
      int Priority = -1;
      life.OnCompleteActivity_Create_Instance_Ext(InstanceID, OfferKey, Tran, ref Name, ref Subject, ref Priority);

      
      _mockery.VerifyAll();
    }
    [NUnit.Framework.Test]
    public void ExtCreateActivity_WithoutAssuredLives()
    {
      SetRepositoryStrategy(TypeFactoryStrategy.Mock);
      ClearMockCache();
      IAccount A = null;
      IApplication O = null;
      SetupLifeMocks(ref O, ref A);

      // Get a collection of assured lives and return a count of 1
      IReadOnlyEventList<ILegalEntityNaturalPerson> lst = _mockery.CreateMock<IReadOnlyEventList<ILegalEntityNaturalPerson>>();
      SetupResult.For(lst.Count).Return(0);
      SetupResult.For(A.GetNaturalPersonLegalEntitiesByRoleType(null, new int[3] { 1, 2, 3 })).IgnoreArguments().Return(lst);

      // get mortgageloan accounts from the offer.Account object (we will be iterating to get full names
      IEventList<IAccount> MortgageLoanAccounts = _mockery.CreateMock<IEventList<IAccount>>();
      SetupResult.For(A.RelatedParentAccounts).Return(MortgageLoanAccounts);
      // check the accounts inside the foreach are of type Mortgageloan
      IAccount mla = _mockery.CreateMock<IAccount>();

      IEnumerator<IAccount> enumerator = SetupIEnumerator<IAccount>(MortgageLoanAccounts, ref mla);
      
      // check to see that we have the correct account type
      SetupResult.For(mla.AccountType).Return(AccountTypes.MortgageLoan);

      // now we have a mortgageloan we want to get the Fullname to set the subject in workflow
      SetupResult.For(mla.GetLegalName(LegalNameFormat.Full)).Return("Bobthebuilder");
      _mockery.ReplayAll();

      string Name = "";
      string Subject = "";
      int Priority = -1;
      life.OnCompleteActivity_Create_Instance_Ext(InstanceID, OfferKey, Tran, ref Name, ref Subject, ref Priority);


      _mockery.VerifyAll();
    }
    [NUnit.Framework.Test]
    public void OnEnter_Policy_NTUd()
    {
      SetRepositoryStrategy(TypeFactoryStrategy.Mock);
      ClearMockCache();
      IApplication O = null;
      IAccount A = null;

      // get an account for the given offer key
      SetupLifeMocks(ref O, ref A);
      IAccountRepository AR = _mockery.CreateMock<IAccountRepository>();
      MockCache.Add(typeof(IAccountRepository).ToString(), AR);
      // create an account status of key = 2 // closed
      IAccountStatus status = _mockery.CreateMock<IAccountStatus>();
      SetupResult.For(status.Key).Return(2);
      
      // set the account status to be 2
      SetupResult.For(A.AccountStatus).Return(status);
      status.Key = 3;
      LastCall.IgnoreArguments();
      // set the date on the account to be now
      A.ChangeDate = DateTime.Now;
      LastCall.IgnoreArguments();
      // set the UserID to be System
      A.UserID = "SystemUser";
      LastCall.IgnoreArguments();

      // get the financial services for our offer
      IEventList<IFinancialService> FinancialServices = _mockery.CreateMock<IEventList<IFinancialService>>();
      SetupResult.For(A.FinancialServices).Return(FinancialServices);
      // setup a financial service that will represent the one we are NTUing
      IFinancialService svc = _mockery.CreateMock<IFinancialService>();

      SetupIEnumerator<IFinancialService>(FinancialServices, ref svc);
      SetupResult.For(svc.Key).IgnoreArguments().Return(-1);

      SetupResult.For(svc.Account).Return(A);
      // set our financial service to be closed
      status.Key = 2;
      LastCall.IgnoreArguments();

      IApplicationRepository OR = MockCache[typeof(IApplicationRepository).ToString()] as IApplicationRepository;
      OR.SaveApplication(null, O);
      LastCall.IgnoreArguments();
      AR.SaveAccount(null, A);
      LastCall.IgnoreArguments();

      _mockery.ReplayAll();

      life.OnEnter_Policy_NTUd(InstanceID, OfferKey, Tran);
      _mockery.VerifyAll();
    }
    [NUnit.Framework.Test]
    public void GetActivityTime_Wait_for_Callback()
    {
      SetRepositoryStrategy(TypeFactoryStrategy.Mock);
      ClearMockCache();

      IApplication O = null;
      IAccount A = null;
      SetupLifeMocks(ref O, ref A);

      // create callback object
      ICallback callback = _mockery.CreateMock<ICallback>();

      // get callback for the offer from the offer repo
      IApplicationRepository OR = (IApplicationRepository)CacheFactory.GetCacheManager("MOCK")[typeof(IApplicationRepository).ToString()];
      Expect.Call(OR.GetCallBackbyApplicationKey(null, OfferKey)).IgnoreArguments().Return(callback);

      // get the callbacktime from the callback
      SetupResult.For(callback.CallbackDate).Return(DateTime.Now);

      _mockery.ReplayAll();

      DateTime dt = life.GetActivityTime_Wait_for_Callback(InstanceID, OfferKey, Tran);
      _mockery.VerifyAll();
    }

    [NUnit.Framework.Test]
    public void OnCompleteActivity_Reactivate_NTUd_Policy_WithNOTNULLDetail()
    {
      SetRepositoryStrategy(TypeFactoryStrategy.Mock);
      ClearMockCache();
      IApplicationRepository OR = _mockery.CreateMock<IApplicationRepository>();
      base.MockCache.Add(typeof(IApplicationRepository).ToString(), OR);
      IAccountRepository AR = _mockery.CreateMock<IAccountRepository>();
      base.MockCache.Add(typeof(IAccountRepository).ToString(), AR);

      IApplication offer = _mockery.CreateMock<IApplication>();
      IApplicationLife LifeOffer = _mockery.CreateMock<IApplicationLife>();
      IAccount LifeAccount = _mockery.CreateMock<IAccount>();
      IAccount MLA = _mockery.CreateMock<IAccount>();
      ILifePolicy LifePolicy = _mockery.CreateMock<ILifePolicy>();
      ILifePolicyStatus LifePolicyStatus = _mockery.CreateMock<ILifePolicyStatus>();
      IOriginationSourceProduct osp = _mockery.CreateMock<IOriginationSourceProduct>();
      IFinancialService LifeFinancialService = _mockery.CreateMock<IFinancialService>();
      IDetail detail = _mockery.CreateMock<IDetail>();

      // *** Setup the various offers, accounts linked to those offers, keys etc ***
      // setup the parent MLA
      SetupResult.For(MLA.Key).Return(123);

      // setup the Life account
      SetupResult.For(LifeAccount.Key).Return(1000);
      //Expect.Call(LifeAccount.Key).Return(1000).Repeat.Any();
      IEventList<IAccount> ParentAccounts = new EventList<IAccount>();
      SetupResult.For(LifeAccount.RelatedParentAccounts).Return(ParentAccounts);
      SetupResult.For(LifeAccount.GetRelatedAccountByType(AccountTypes.MortgageLoan, null)).IgnoreArguments().Return(MLA);

      // setup the life offer
      SetupResult.For(LifeOffer.Application).Return(offer);
      SetupResult.For(offer.Account).Return(LifeAccount);

      // setup the main offer to return a life offer
      SetupResult.For(offer.ApplicationLife).Return(LifeOffer);

      // when we call get offer on the offerRepo we return the offer we have created here.
      SetupResult.For(OR.GetApplicationByKey(null, OfferKey)).IgnoreArguments().Return(offer);

      SetupResult.For(LifeAccount.OriginationSourceProduct).Return(osp);
      SetupResult.For(osp.Key).Return(4);

      // update the account
      LifeAccount.FixedPayment = 0; // dont ignore args
      LifeAccount.ChangeDate = DateTime.Now;
      LastCall.IgnoreArguments();
      LifeAccount.UserID = "X2";
      LastCall.IgnoreArguments();

      // update the financial service
      IFinancialService svc = _mockery.CreateMock<IFinancialService>();
      SetupResult.For(MLA.GetFinancialServiceByType(FinancialServiceTypes.LifePolicy)).Return(svc);
      svc.Payment = 0;
      LastCall.IgnoreArguments();
      //LifeFinancialService.Payment = 0; // dont ignore args

      // need another test here to check the null return state
      Expect.Call(AR.GetDetailByAccountKeyAndDetailType(null, -1, 450)).IgnoreArguments().Return(detail);
      detail.LinkID = 1000;// LifeAccount.Key;
      LastCall.IgnoreArguments();
      detail.Description = string.Format("Policy: {0}", 1000);
      LastCall.IgnoreArguments();

      AR.SaveAccount(null, LifeAccount);
      LastCall.IgnoreArguments();
      // need to save the changes to detail here.
      AR.SaveDetail(null, detail);
      LastCall.IgnoreArguments();

      _mockery.ReplayAll();
      life.OnCompleteActivity_Reactivate_NTUd_Policy(InstanceID, OfferKey, Tran, PolicyNumber);
      _mockery.VerifyAll();
    }

    [NUnit.Framework.Test]
    public void OnEnter_Ready_to_Callback()
    {
        SetRepositoryStrategy(TypeFactoryStrategy.Mock);
        ClearMockCache();
        IApplicationRepository OR = _mockery.CreateMock<IApplicationRepository>();
        base.MockCache.Add(typeof(IApplicationRepository).ToString(), OR);
        IX2Repository x2 = _mockery.CreateMock<IX2Repository>();
        MockCache.Add(typeof(IX2Repository).ToString(), x2);
        IMessageService messageSvc = _mockery.CreateMock<IMessageService>();
        MockCache.Add(typeof(IMessageService).ToString(), messageSvc);

        IAccount MLA = _mockery.CreateMock<IAccount>();
        IAccount lifeAccount = _mockery.CreateMock<IAccount>();
        IApplication offer = _mockery.CreateMock<IApplication>();
        IApplicationLife LifeOffer = _mockery.CreateMock<IApplicationLife>();
        IInstance instance = _mockery.CreateMock<IInstance>();
        ICallback cb = _mockery.CreateMock<ICallback>();
        IFinancialService lifeSvc = _mockery.CreateMock<IFinancialService>();
        IBroker broker = _mockery.CreateMock<IBroker>();
        IMailMessage message = _mockery.CreateMock<IMailMessage>();
        IEventList<IAccount> MortgageLoanAccounts = _mockery.CreateMock<IEventList<IAccount>>();
      IReason reason = _mockery.CreateMock<IReason>();

        SetupResult.For(x2.GetInstanceByKey(null, InstanceID)).IgnoreArguments().Return(instance);  
        SetupResult.For(OR.GetApplicationByKey(null, OfferKey)).IgnoreArguments().Return(offer);

        SetupResult.For(offer.ApplicationLife).Return(LifeOffer);
        SetupResult.For(offer.Account).Return(lifeAccount);
        SetupResult.For(OR.GetCallBackbyApplicationKey(null, OfferKey)).IgnoreArguments().Return(cb);

        SetupResult.For(lifeAccount.RelatedParentAccounts).IgnoreArguments().Return(MortgageLoanAccounts);
        SetupResult.For(lifeAccount.GetRelatedAccountByType(AccountTypes.MortgageLoan, null)).IgnoreArguments().Return(MLA);
        SetupResult.For(MLA.Key).Return(123);
        SetupResult.For(MLA.GetFinancialServiceByType(FinancialServiceTypes.LifePolicy)).Return(lifeSvc);

        SetupResult.For(OR.GetCallBackbyApplicationKey(null, OfferKey)).IgnoreArguments().Return(cb);
        cb.CompletedDate = DateTime.Now;
        LastCall.IgnoreArguments();
        SetupResult.For(cb.EntryUser).Return("BobTheDestroyerOfWorldsButOnlyOnTuesday");
        LastCall.IgnoreArguments();
        cb.CompletedUser = "BobTheDestroyerOfWorldsButOnlyOnTuesday";
        LastCall.IgnoreArguments();

        SetupResult.For(LifeOffer.Consultant).Return("RatbertConsultant");
        SetupResult.For(lifeSvc.Key).Return(1234);
        SetupResult.For(LifeOffer.BrokerKey).Return(broker);
        SetupResult.For(broker.EmailAddress).Return("Suckers@WillBeBrokeSoon.co.za");
        SetupResult.For(instance.Subject).Return("Call this muppet back");
        SetupResult.For(cb.Reason).Return(reason);
        SetupResult.For(reason.Comment).Return("A A A A# D Bb C");
        SetupResult.For(cb.CallbackDate).IgnoreArguments().Return(DateTime.Now);

        //message.From = "";
        //LastCall.IgnoreArguments();
        //message.To = "";
        //LastCall.IgnoreArguments();
        //message.Subject = "";
        //LastCall.IgnoreArguments();
        //message.Body = "";
        //LastCall.IgnoreArguments();

        //Expect.Call(OR.GetApplicationByKey(null, OfferKey)).IgnoreArguments().Return(offer);
        //Expect.Call(x2.GetInstanceByKey(null, InstanceID)).IgnoreArguments().Return(instance);

        //Expect.Call(messageSvc.SendMessage).IgnoreArguments();

        OR.SaveCallback(null, cb);
        LastCall.IgnoreArguments();
        _mockery.ReplayAll();
        life.OnEnter_Ready_to_Callback(InstanceID, OfferKey, Tran);
        _mockery.VerifyAll();
    }

  [NUnit.Framework.Ignore("To see if Ed Notices")]
    [NUnit.Framework.Test]
    public void OnCompleteActivity_Continue_Sale()
    {
    }

    [NUnit.Framework.Test]
    public void OnGetRole_CurrentConsultant()
    {
      SetRepositoryStrategy(TypeFactoryStrategy.Mock);
      ClearMockCache();
      IApplicationRepository OR = _mockery.CreateMock<IApplicationRepository>();
      base.MockCache.Add(typeof(IApplicationRepository).ToString(), OR);
      IApplication offer = _mockery.CreateMock<IApplication>();
      IApplicationLife offerLife = _mockery.CreateMock<IApplicationLife>();

      SetupResult.For(OR.GetApplicationByKey(null, OfferKey)).IgnoreArguments().Return(offer);
      SetupResult.For(offer.ApplicationLife).Return(offerLife);
      SetupResult.For(offerLife.Consultant).Return("SAHL\\Test");

      _mockery.ReplayAll();
      life.OnGetRole_CurrentConsultant(OfferKey, Tran);
      _mockery.VerifyAll();
    }

    protected void SetupLifeMocks(ref IApplication O, ref IAccount A)
    {
      CacheManager CM = CacheFactory.GetCacheManager("MOCK");
      // setup the cbosevice

      // setup an offer repository
      IApplicationRepository OR = _mockery.CreateMock<IApplicationRepository>();
      CM.Add(typeof(IApplicationRepository).ToString(), OR);

      // setup the data returned by a call to GetApplicationByKey
      O = _mockery.CreateMock<IApplication>();
      // setup fake account. WHen the account is referenced from teh offer we return the fake
      // accoutn and fake properties
      A = _mockery.CreateMock<SAHL.Common.BusinessModel.Interfaces.IAccount>();
      SetupResult.For(A.Key).Return(1000);
      SetupResult.For(O.Account).Return(A);

      // when we call get offer on the offerRepo we return the offer we have created here.
      Expect.Call(OR.GetApplicationByKey(null, OfferKey)).IgnoreArguments().Return(O);
    }
  }
}
