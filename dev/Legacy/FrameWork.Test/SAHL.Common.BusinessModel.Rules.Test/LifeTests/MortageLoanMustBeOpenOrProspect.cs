using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace SAHL.Common.BusinessModel.Rules.Test.LifeTests
{
  //[TestFixture]
  public class MortageLoanMustBeOpenOrProspect : TestBase
  {
    IDomainMessageCollection Messages = null;
    IRuleRepository RuleRepo = null;
    IRuleItem ruleItem = null;
    string RuleName = "RuleName";
    int AccountKey = -1;
    
    [NUnit.Framework.SetUp()]
    public void Setup()
    {
      try
      {
        SetRepositoryStrategy(TypeFactoryStrategy.Mock);
        ClearMockCache();
        Messages = new DomainMessageCollection();
      }
      catch (Exception ex)
      {
        string s = ex.ToString();
      }
    }

    [TearDown]
    public void Teardown()
    {
    }

    public void SetupRuleBaseCommon()
    {
      CacheManager CM = CacheFactory.GetCacheManager("MOCK");
      RuleRepo = _mockery.StrictMock<IRuleRepository>();
      MockCache.Add(typeof(IRuleRepository).ToString(), RuleRepo);

      ruleItem = _mockery.StrictMock<IRuleItem>();
      //ruleSetRule = _mockery.StrictMock<IRuleSetRule>();
      SetupResult.For(RuleRepo.FindRuleItemByName(RuleName)).IgnoreArguments().Return(ruleItem);
      //IEventList<IRuleSetRule> RuleSetRules = _mockery.StrictMock<IEventList<IRuleSetRule>>();
      //SetupResult.For(ruleItem.RuleSetRules).Return(RuleSetRules);
      //SetupIEnumerator<IRuleSetRule>(RuleSetRules, ref ruleSetRule);
      //SetupResult.For(ruleSetRule.EnForceRule).Return(true);
    }

    [NUnit.Framework.Test]
    public void MortgagueLoanOpen()
    {
      SetupRuleBaseCommon();
      IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
      MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);
      IAccount account = _mockery.StrictMock<IAccount>();
      SetupResult.For(accRepo.GetAccountByKey(AccountKey)).IgnoreArguments().Return(account);
      IAccountStatus accStatus = _mockery.StrictMock<IAccountStatus>();
      SetupResult.For(accStatus.Key).Return(1);
      SetupResult.For(account.AccountStatus).Return(accStatus);
      _mockery.ReplayAll();
      _mockery.VerifyAll();
    }

    [NUnit.Framework.Test]
    public void MortgagueLoanProspect()
    {
      SetupRuleBaseCommon();
      IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
      MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);
      IAccount account = _mockery.StrictMock<IAccount>();
      SetupResult.For(accRepo.GetAccountByKey(AccountKey)).IgnoreArguments().Return(account);
      IAccountStatus accStatus = _mockery.StrictMock<IAccountStatus>();
      SetupResult.For(accStatus.Key).Return(3);
      SetupResult.For(account.AccountStatus).Return(accStatus);
      _mockery.ReplayAll();
      _mockery.VerifyAll();
    }

    [NUnit.Framework.Test]
    public void MortgagueLoanNotOpenOrProspectButClosed()
    {
      SetupRuleBaseCommon();
      IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
      MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);
      IAccount account = _mockery.StrictMock<IAccount>();
      SetupResult.For(accRepo.GetAccountByKey(AccountKey)).IgnoreArguments().Return(account);
      IAccountStatus accStatus = _mockery.StrictMock<IAccountStatus>();
      // Closed
      SetupResult.For(accStatus.Key).Return(2);
      SetupResult.For(account.AccountStatus).Return(accStatus);
      _mockery.ReplayAll();
      _mockery.VerifyAll();
      
    }

    [NUnit.Framework.Test]
    public void MortgagueLoanClosed()
    {
      _mockery.ReplayAll();
      _mockery.VerifyAll();
    }

    [NUnit.Framework.Test]
    public void MortgagueLoanNotProspect()
    {
      _mockery.ReplayAll();
      _mockery.VerifyAll();
    }
  }
}
