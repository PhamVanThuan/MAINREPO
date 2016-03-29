using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.Property;

namespace SAHL.Common.BusinessModel.Rules.Test.Property
{
    [TestFixture]
    public class PropertyAccessDetails : RuleBase
    {
        IPropertyAccessDetails propertyAccessDetails;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            propertyAccessDetails = _mockery.StrictMock<IPropertyAccessDetails>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void PropertyInspectionContactDetailsTest_Pass()
        {
            PropertyInspectionContactDetails rule = new PropertyInspectionContactDetails();

            SetupResult.For(propertyAccessDetails.Contact1).Return("Contact1");
            SetupResult.For(propertyAccessDetails.Contact1Phone).Return("12345");
            SetupResult.For(propertyAccessDetails.Contact2).Return("");
            SetupResult.For(propertyAccessDetails.Contact2Phone).Return("");

            // we except 0 messages
            ExecuteRule(rule, 0, propertyAccessDetails);
        }

        [NUnit.Framework.Test]
        public void PropertyInspectionContactDetailsTest_Fail_Contact1_NoNumber()
        {
            PropertyInspectionContactDetails rule = new PropertyInspectionContactDetails();

            SetupResult.For(propertyAccessDetails.Contact1).Return("Contact1");
            SetupResult.For(propertyAccessDetails.Contact1Phone).Return("");
            SetupResult.For(propertyAccessDetails.Contact2).Return("");
            SetupResult.For(propertyAccessDetails.Contact2Phone).Return("");

            // we except 1 messages
            ExecuteRule(rule, 1, propertyAccessDetails);
        }
        [NUnit.Framework.Test]
        public void PropertyInspectionContactDetailsTest_Fail_Contact1_NoName()
        {
            PropertyInspectionContactDetails rule = new PropertyInspectionContactDetails();

            SetupResult.For(propertyAccessDetails.Contact1).Return("");
            SetupResult.For(propertyAccessDetails.Contact1Phone).Return("12345");
            SetupResult.For(propertyAccessDetails.Contact2).Return("");
            SetupResult.For(propertyAccessDetails.Contact2Phone).Return("");

            // we except 1 messages
            ExecuteRule(rule, 1, propertyAccessDetails);
        }

        [NUnit.Framework.Test]
        public void PropertyInspectionContactDetailsTest_Fail_Contact1and2_NoNumber()
        {
            PropertyInspectionContactDetails rule = new PropertyInspectionContactDetails();

            SetupResult.For(propertyAccessDetails.Contact1).Return("Contact1");
            SetupResult.For(propertyAccessDetails.Contact1Phone).Return("");
            SetupResult.For(propertyAccessDetails.Contact2).Return("Contact2");
            SetupResult.For(propertyAccessDetails.Contact2Phone).Return("");

            // we except 2 messages
            ExecuteRule(rule, 2, propertyAccessDetails);
        }

        [NUnit.Framework.Test]
        public void PropertyInspectionContactDetailsTest_Fail_Contact1and2_NoName()
        {
            PropertyInspectionContactDetails rule = new PropertyInspectionContactDetails();

            SetupResult.For(propertyAccessDetails.Contact1).Return("");
            SetupResult.For(propertyAccessDetails.Contact1Phone).Return("12345");
            SetupResult.For(propertyAccessDetails.Contact2).Return("");
            SetupResult.For(propertyAccessDetails.Contact2Phone).Return("12345");

            // we except 2 messages
            ExecuteRule(rule, 2, propertyAccessDetails);
        }
    }
}
