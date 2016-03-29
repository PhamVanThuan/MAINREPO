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
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Rules.Application.Credit;
using SAHL.Common.BusinessModel.Rules.ApplicationRole;
using SAHL.Common.BusinessModel.Rules.ApplicationDeclaration;
using Castle.ActiveRecord;
using SAHL.Common.Factories;
namespace SAHL.Common.BusinessModel.Rules.Test.ApplicationDeclaration
{
    [TestFixture]
    public class ApplicationDeclarationTest : RuleBase
    {
        IApplication _application;
        IApplicationRole _appRole;
        IApplicationRoleType _appRoleType;
        IGeneralStatus _generalStatus;
        IApplicationDeclaration _appDeclaration;
        IApplicationDeclarationQuestion _appDecQues;
        IApplicationDeclarationAnswer _appDecAns;
        IEventList<IApplicationDeclaration> _appDeclarationList;
        IReadOnlyEventList<IApplicationRole> _appRoleReadOnlyList;
        IList<IApplicationRole> _appRoleList;
        ILegalEntity _legalEntity;

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [Test]
        public void ApplicationDeclarationCurrentDebtRearrangementTestPass()
        {
            ApplicationDeclarationCurrentDebtRearrangement rule = new ApplicationDeclarationCurrentDebtRearrangement();
            SetUpPassHelper();
            ExecuteRule(rule, 0, _application);
        }

        [Test]
        public void ApplicationDeclarationCurrentDebtRearrangementTestFail()
        {
            ApplicationDeclarationCurrentDebtRearrangement rule = new ApplicationDeclarationCurrentDebtRearrangement();
            SetUpFailHelper();
            ExecuteRule(rule, 1, _application);
        }

        void SetUpPassHelper()
        {
            _application = _mockery.StrictMock<IApplication>();
            _appRole = _mockery.StrictMock<IApplicationRole>();
            _appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            _generalStatus = _mockery.StrictMock<IGeneralStatus>();
            _appDeclaration = _mockery.StrictMock<IApplicationDeclaration>();
            _appDecQues = _mockery.StrictMock<IApplicationDeclarationQuestion>();
            _appDecAns = _mockery.StrictMock<IApplicationDeclarationAnswer>();
            _legalEntity = _mockery.StrictMock<ILegalEntity>();
            //
            _appDeclarationList = new EventList<IApplicationDeclaration>();
            _appRoleList = new List<IApplicationRole>();
            //
            SetupResult.For(_legalEntity.DisplayName).Return("Test");
            //
            SetupResult.For(_generalStatus.Key).Return((int)GeneralStatuses.Active);
            //
            SetupResult.For(_appRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            //
            SetupResult.For(_appDecQues.Key).Return((int)OfferDeclarationQuestions.CurrentDebtRearrangement);
            //
            SetupResult.For(_appDecAns.Key).Return((int)OfferDeclarationAnswers.No);
            //
            SetupResult.For(_appDeclaration.ApplicationDeclarationAnswer).Return(_appDecAns);
            SetupResult.For(_appDeclaration.ApplicationDeclarationQuestion).Return(_appDecQues);
            //
            _appDeclarationList.Add(null, _appDeclaration);
            //
            SetupResult.For(_appRole.ApplicationDeclarations).Return(_appDeclarationList);
            SetupResult.For(_appRole.ApplicationRoleType).Return(_appRoleType);
            SetupResult.For(_appRole.GeneralStatus).Return(_generalStatus);
            SetupResult.For(_appRole.LegalEntity).Return(_legalEntity);
            //
            _appRoleList.Add(_appRole);
            _appRoleReadOnlyList = new ReadOnlyEventList<IApplicationRole>(_appRoleList);
            //
            SetupResult.For(_application.ApplicationRoles).Return(_appRoleReadOnlyList);
        }


        void SetUpFailHelper()
        {
            _application = _mockery.StrictMock<IApplication>();
            _appRole = _mockery.StrictMock<IApplicationRole>();
            _appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            _generalStatus = _mockery.StrictMock<IGeneralStatus>();
            _appDeclaration = _mockery.StrictMock<IApplicationDeclaration>();
            _appDecQues = _mockery.StrictMock<IApplicationDeclarationQuestion>();
            _appDecAns = _mockery.StrictMock<IApplicationDeclarationAnswer>();
            _legalEntity = _mockery.StrictMock<ILegalEntity>();
            //
            _appDeclarationList = new EventList<IApplicationDeclaration>();
            _appRoleList = new List<IApplicationRole>();
            //
            SetupResult.For(_legalEntity.DisplayName).Return("Test");
            //
            SetupResult.For(_generalStatus.Key).Return((int)GeneralStatuses.Active);
            //
            SetupResult.For(_appRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            //
            SetupResult.For(_appDecQues.Key).Return((int)OfferDeclarationQuestions.CurrentDebtRearrangement);
            //
            SetupResult.For(_appDecAns.Key).Return((int)OfferDeclarationAnswers.Yes);
            //
            SetupResult.For(_appDeclaration.ApplicationDeclarationAnswer).Return(_appDecAns);
            SetupResult.For(_appDeclaration.ApplicationDeclarationQuestion).Return(_appDecQues);
            //
            _appDeclarationList.Add(null, _appDeclaration);
            //
            SetupResult.For(_appRole.ApplicationDeclarations).Return(_appDeclarationList);
            SetupResult.For(_appRole.ApplicationRoleType).Return(_appRoleType);
            SetupResult.For(_appRole.GeneralStatus).Return(_generalStatus);
            SetupResult.For(_appRole.LegalEntity).Return(_legalEntity);
            //
            _appRoleList.Add(_appRole);
            _appRoleReadOnlyList = new ReadOnlyEventList<IApplicationRole>(_appRoleList);
            //
            SetupResult.For(_application.ApplicationRoles).Return(_appRoleReadOnlyList);
        }

 

    }
}
