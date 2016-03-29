using System;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;
using SAHL.Common.Factories;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Rules.ApplicationRole;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DataAccess;
using System.Data;
using SAHL.Common.BusinessModel.Rules;
using SAHL.Common.BusinessModel.Rules.Role;

namespace SAHL.Common.BusinessModel.Test.Rules.Role
{
    [TestFixture]
    public class Role : RuleBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void ValidateUniqueAccountRoleTestPass()
        {
            ValidateUniqueAccountRole rule = new ValidateUniqueAccountRole();
            IAccount account = _mockery.CreateMock<IAccount>();
            IRole accRole1 = _mockery.CreateMock<IRole>();
            IRole accRole2 = _mockery.CreateMock<IRole>();
            IRoleType roleType = _mockery.CreateMock<IRoleType>();
            ILegalEntity le1 = _mockery.CreateMock<ILegalEntity>();
            ILegalEntity le2 = _mockery.CreateMock<ILegalEntity>();
            IEventList<IRole> roles = new EventList<IRole>();
            IGeneralStatus status = _mockery.CreateMock<IGeneralStatus>();
            //
            SetupResult.For(status.Key).Return(1);
            SetupResult.For(accRole1.GeneralStatus).Return(status);
            SetupResult.For(accRole2.GeneralStatus).Return(status);
            SetupResult.For(le1.Key).Return(1);
            SetupResult.For(le2.Key).Return(2);
            SetupResult.For(accRole1.LegalEntity).Return(le1);
            SetupResult.For(accRole2.LegalEntity).Return(le2);
            roles.Add(null,accRole2);
            SetupResult.For(account.Roles).Return(roles);
            SetupResult.For(accRole1.Account).Return(account);
            ExecuteRule(rule, 0, accRole1);
        }


        [Test]
        public void ValidateUniqueAccountRoleTestFail()
        {
            ValidateUniqueAccountRole rule = new ValidateUniqueAccountRole();
            IAccount account = _mockery.CreateMock<IAccount>();
            IRole accRole1 = _mockery.CreateMock<IRole>();
            IRole accRole2 = _mockery.CreateMock<IRole>();
            IRoleType roleType = _mockery.CreateMock<IRoleType>();
            ILegalEntity le1 = _mockery.CreateMock<ILegalEntity>();
            ILegalEntity le2 = _mockery.CreateMock<ILegalEntity>();
            IEventList<IRole> roles = new EventList<IRole>();
            IGeneralStatus status = _mockery.CreateMock<IGeneralStatus>();
            //
            SetupResult.For(status.Key).Return(1);
            SetupResult.For(accRole1.GeneralStatus).Return(status);
            SetupResult.For(accRole2.GeneralStatus).Return(status);
            SetupResult.For(le1.Key).Return(1);
            SetupResult.For(le2.Key).Return(1);
            SetupResult.For(accRole1.LegalEntity).Return(le1);
            SetupResult.For(accRole2.LegalEntity).Return(le2);
            roles.Add(null, accRole2);
            SetupResult.For(account.Roles).Return(roles);
            SetupResult.For(accRole1.Account).Return(account);
            ExecuteRule(rule, 1, accRole1);
        }
    }
}
