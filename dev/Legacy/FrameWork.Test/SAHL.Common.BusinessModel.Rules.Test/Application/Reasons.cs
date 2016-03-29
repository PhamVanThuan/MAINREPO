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
using SAHL.Common.BusinessModel.Rules.Application.Reasons;

namespace SAHL.Common.BusinessModel.Rules.Test.Application.Reasons
{
    [TestFixture]
    public class Reasons : RuleBase
    {
        IApplication _application;

        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();
            _application = _mockery.StrictMock<IApplication>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        //ApplicationReasonTypeAdd TESTS

        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ApplicationReasonTypeAddNoArgumentsPassed()
        {
            ApplicationReasonTypeAdd art = new ApplicationReasonTypeAdd();

            ExecuteRule(art, 0);

        }

        [NUnit.Framework.Test]
        public void ApplicationReasonTypeAddSuccess()
        {
            ApplicationReasonTypeAdd art = new ApplicationReasonTypeAdd();
            IApplication application = _mockery.StrictMock<IApplication>();

            IApplicationType apptype = _mockery.StrictMock<IApplicationType>();
            Expect.Call(application.ApplicationType).Return(apptype);
            Expect.Call(apptype.Key).Return(2);

            ExecuteRule(art, 0, application);

        }


        [NUnit.Framework.Test]
        public void ApplicationReasonTypeAddFail()
        {
            ApplicationReasonTypeAdd rule = new ApplicationReasonTypeAdd();
            IApplicationType apptype = _mockery.StrictMock<IApplicationType>();

            SetupResult.For(_application.ApplicationType).Return(apptype);
            SetupResult.For(apptype.Key).Return(-1);
            ExecuteRule(rule, 1, _application);


            SetupResult.For(_application.ApplicationType).Return(apptype);
            SetupResult.For(apptype.Key).Return((int)OfferTypes.ReAdvance);
            ExecuteRule(rule, 0, _application);

        }


        // ApplicationReasonUpdate TESTS

        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ApplicationReasonUpdateNoArgumentsPassed()
        {
            ApplicationReasonUpdate aru = new ApplicationReasonUpdate();
            ExecuteRule(aru, 0);
        }

        [NUnit.Framework.Test]
        public void ApplicationReasonUpdateSuccess()
        {
            ApplicationReasonUpdate aru = new ApplicationReasonUpdate();
            IApplication application = _mockery.StrictMock<IApplication>();
            IApplicationStatus appstatus = _mockery.StrictMock<IApplicationStatus>();

            SetupResult.For(application.ApplicationStatus).Return(appstatus);
            SetupResult.For(appstatus.Key).Return(1);

            ExecuteRule(aru, 0, application);
        }

        [NUnit.Framework.Test]
        public void ApplicationReasonUpdateFail()
        {
            ApplicationReasonUpdate aru = new ApplicationReasonUpdate();
            IApplication application = _mockery.StrictMock<IApplication>();
            IApplicationStatus appstatus = _mockery.StrictMock<IApplicationStatus>();

            SetupResult.For(application.ApplicationStatus).Return(appstatus);
            SetupResult.For(appstatus.Key).Return(-1);

            ExecuteRule(aru, 1, application);


        }

        [NUnit.Framework.Test]
        public void ApplicationReasonMiscellaneousEnforceCommentTest()
        {
            // FAIL - no comments
            ApplicationReasonMiscellaneousEnforceCommentTestHelper(1, (int)ReasonDescriptions.MiscellaneousReason, "");
            // FAIL - only spaces in comment
            ApplicationReasonMiscellaneousEnforceCommentTestHelper(1, (int)ReasonDescriptions.MiscellaneousReason, "  ");
            // PASS - comments
            ApplicationReasonMiscellaneousEnforceCommentTestHelper(0, (int)ReasonDescriptions.MiscellaneousReason, "some comment");
            // Pass - not miscellaneous
            ApplicationReasonMiscellaneousEnforceCommentTestHelper(0, (int)ReasonDescriptions.DeedsBondGrantRights, "");

        }

        private void ApplicationReasonMiscellaneousEnforceCommentTestHelper(int expectedMessages, int reasonDef, string comment)
        {
            ApplicationReasonMiscellaneousEnforceComment rule = new ApplicationReasonMiscellaneousEnforceComment();

            IReason reason = _mockery.StrictMock<IReason>();
            IReasonDefinition reaDef = _mockery.StrictMock<IReasonDefinition>();
            IReasonDescription reaDes = _mockery.StrictMock<IReasonDescription>();

            SetupResult.For(reaDes.Key).Return(reasonDef);
            SetupResult.For(reaDef.ReasonDescription).Return(reaDes);
            SetupResult.For(reason.ReasonDefinition).Return(reaDef);

            SetupResult.For(reason.Comment).Return(comment);

            ExecuteRule(rule, expectedMessages, reason);
        }



    }
}
