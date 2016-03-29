using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;
using SAHL.UI.Halo.Configuration.Client.Detail;
using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Models.Client;

namespace SAHL.UI.Halo.Tests
{
    [TestFixture]
    public class TestClientDetailRootTileHeaderLeftIconProvider
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var provider = new ClientDetailRootTileHeaderLeftIconProvider();
            //---------------Test Result -----------------------
            Assert.IsNotNull(provider);
        }

        [Test]
        public void Execute_GivenNullDataModel_ShouldNotSetHeaderIcons()
        {
            //---------------Set up test pack-------------------
            var provider = new ClientDetailRootTileHeaderLeftIconProvider();
            ClientRootModel model = null;
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, provider.HeaderIcons.Count);
            //---------------Execute Test ----------------------
            provider.Execute(model);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, provider.HeaderIcons.Count);
        }

        [Test]
        public void Execute_GivenLegalEntityKeyTypeIsNotNaturalPerson_ShouldSetHeaderIconsToFaBuilding()
        {
            //---------------Set up test pack-------------------
            var provider = new ClientDetailRootTileHeaderLeftIconProvider();
            var model    = new ClientRootModel
                {
                    LegalEntityTypeKey = LegalEntityType.Trust
                };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            provider.Execute(model);
            //---------------Test Result -----------------------
            CollectionAssert.Contains(provider.HeaderIcons, "fa-building");
        }

        [TestCase(LegalEntityType.NaturalPerson, Gender.Male, LegalEntityStatus.Alive, null, "fa-male status-alive")]
        [TestCase(LegalEntityType.NaturalPerson, Gender.Female, LegalEntityStatus.Alive, null, "fa-female status-alive")]
        [TestCase(LegalEntityType.NaturalPerson, Gender.Male, LegalEntityStatus.Deceased, null, "fa-male status-deceased")]
        [TestCase(LegalEntityType.NaturalPerson, Gender.Female, LegalEntityStatus.Deceased, null, "fa-female status-deceased")]
        [TestCase(LegalEntityType.NaturalPerson, Gender.Male, LegalEntityStatus.Disabled, null, "fa-male status-disabled")]
        [TestCase(LegalEntityType.NaturalPerson, Gender.Female, LegalEntityStatus.Disabled, null, "fa-female status-disabled")]
        [TestCase(LegalEntityType.NaturalPerson, Gender.Female, LegalEntityStatus.Disabled, CitizenType.SACitizen, "flag-icon flag-icon-za")]
        [TestCase(LegalEntityType.NaturalPerson, Gender.Female, LegalEntityStatus.Disabled, CitizenType.SACitizen_NonResident, "flag-icon flag-icon-za")]
        public void Execute_GivenSpecificParameters_ShouldSetExpectedHeaderIcon(LegalEntityType legalEntityType, Gender gender,
                                                                                LegalEntityStatus legalEntityStatus, CitizenType citizenType, string expectedIcon)
        {
            //---------------Set up test pack-------------------
            var provider = new ClientDetailRootTileHeaderLeftIconProvider();
            var model    = new ClientRootModel
                {
                    LegalEntityTypeKey = legalEntityType,
                    Gender             = gender,
                    Status             = legalEntityStatus,
                    CitizenTypeKey     = citizenType,
                };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            provider.Execute(model);
            //---------------Test Result -----------------------
            CollectionAssert.Contains(provider.HeaderIcons, expectedIcon);
        }
    }
}
