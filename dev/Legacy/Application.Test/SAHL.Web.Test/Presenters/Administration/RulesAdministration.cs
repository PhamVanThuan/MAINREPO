using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks.Constraints;

namespace SAHL.Web.Test.Presenters.Administration
{
  [TestFixture]
  public class RuleDetailsView : TestViewBase
  {
    private IRuleDetails _view;
    private IEventRaiser _SearchButtonClickRaiser;
    private IRuleRepository _RulesRepo;

    [SetUp]
    public void Setup()
    {
      _view = _mockery.CreateMock<IRuleDetails>();
      base.SetupMockedView(_view);
      SetupPrincipalCache(base.TestPrincipal);
      _RulesRepo = _mockery.CreateMock<IRuleRepository>();
      MockCache.Add(typeof(IRuleRepository).ToString(), _RulesRepo);
    }

    [TearDown]
    public void TearDown()
    {
      _view = null;
    }

    [NUnit.Framework.Test]
    public void InitialiseTest()
    {
      IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

      Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();
      IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
      _view.OnSearchButtonClicked += null;
      EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
      dict.Add("OnSearchButtonClicked", EventRaiser);

      _mockery.ReplayAll(); 
    }

    [NUnit.Framework.Test]
    public void PreRenderTest()
    {
      //_mockery.ReplayAll();
      //_mockery.VerifyAll();
    }

    [NUnit.Framework.Test]
    public void OnSearchClickedNoErrorsWarningsOrInfo()
    {
      // hook up the button click event
      _view.OnSearchButtonClicked += null;
      LastCall.Constraints(Is.NotNull());
      _SearchButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser();
      SetupResult.For(RepositoryFactory.GetRepository<IRuleRepository>()).IgnoreArguments().Return(_RulesRepo);
      
      IDomainMessageCollection Messages = _mockery.CreateMock<IDomainMessageCollection>();
      IEventList<IRuleItem> Rules = _mockery.CreateMock<IEventList<IRuleItem>>();
      SetupResult.For(_RulesRepo.FindRulesByPartialName(null, null)).IgnoreArguments().Return(Rules);

      SetupResult.For(Messages.HasErrorMessages).Return(false);
      SetupResult.For(Messages.HasWarningMessages).Return(false);
      SetupResult.For(Messages.HasInfoMessages).Return(false);

      _view.BindRulesGrid(Rules);
      LastCall.IgnoreArguments();

      _mockery.ReplayAll();
      
      
      _mockery.VerifyAll();
    }
  }
}
