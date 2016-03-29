using System;
using System.Data;
using System.Resources;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework;
using SAHL.Config.Web.Mvc;
using SAHL.Core;
using SAHL.Core.Strings;
using SAHL.Services.Query;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Serialiser;
using SAHL.Services.Query.Url;
using StructureMap;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Config.Services.Query.Server.Tests
{
    [TestFixture]
    public class IocRegistryTests
    {
        [Test]
        public void GivenValidConfig_CanRetrieveInstanceOfCustomHttpConfigurationFromIocContainerAsSingleton()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            var instance = container.GetInstance<CustomHttpConfiguration>();
            var instance2 = container.GetInstance<CustomHttpConfiguration>();

            Assert.That(instance, Is.Not.Null);

            Assert.That(ReferenceEquals(instance, instance2), Is.True);
        }

        [Test]
        public void GivenValidConfig_CanRetrieveInstanceOfLinkCollectionFromIocContainerAsSingleton()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            var instance = container.GetInstance<ILinkMetadataCollection>();
            var instance2 = container.GetInstance<ILinkMetadataCollection>();

            Assert.That(instance, Is.Not.Null);

            Assert.That(ReferenceEquals(instance, instance2), Is.True);
        }

        [Test]
        public void GivenValidConfig_ShouldHaveNonNullAndNonEmptyStaticRepresentationLinks()
        {
            var bootstrapper = new ServiceBootstrapper();
            bootstrapper.Initialise();

            Assert.That(Representations.LinkMetadata, Is.Not.Null);
            Assert.That(Representations.LinkMetadata, Is.Not.Empty);
        }

        [Test]
        public void GivenValidConfig_ShouldHaveRegisteredTheConfiguredRoutesAsStaticRepresentationLinks()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            var instance = container.GetInstance<ILinkMetadataCollection>();

            Assert.That(instance.Count(), Is.EqualTo(Representations.LinkMetadata.Count()));

            var instanceItems = instance
                .SelectMany(a => a.Value.Values)
                .SelectMany(a => a)
                .OrderBy(a => a.UrlTemplate);

            var staticItems = Representations.LinkMetadata
                .SelectMany(a => a.Value.Values)
                .SelectMany(a => a)
                .OrderBy(a => a.UrlTemplate);

            var pairedItems = instanceItems
                .Zip(staticItems, (instanceItem, staticItem) => new { InstanceItem = instanceItem, StaticItem = staticItem })
                .ToList();

            foreach (var item in pairedItems)
            {
                Assert.That(item.InstanceItem.Controller, Is.EqualTo(item.StaticItem.Controller));
                Assert.That(item.InstanceItem.UrlTemplate, Is.EqualTo(item.StaticItem.UrlTemplate));
                Assert.That(item.InstanceItem.Relationship, Is.EqualTo(item.StaticItem.Relationship));
                Assert.That(item.InstanceItem.RepresentationType, Is.EqualTo(item.StaticItem.RepresentationType));

                if (item.InstanceItem.RepresentationType != null) //only compare if linked to a specific representation
                {
                    Assert.That(item.InstanceItem.Name, Is.EqualTo(item.StaticItem.Name));
                }
            }
        }

        [Test]
        public void GivenValidConfig_ShouldHaveRegisteredTheLinkResolverAsSingleton()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            var instance = container.GetInstance<ILinkResolver>();
            var instance2 = container.GetInstance<ILinkResolver>();

            Assert.That(instance, Is.Not.Null);

            Assert.That(ReferenceEquals(instance, instance2), Is.True);
        }

        [Test]
        public void GivenValidConfig_ShouldHaveRegisteredTheRepresentationTemplateCacheAsSingleton()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            var instance = container.GetInstance<IRepresentationTemplateCache>();
            var instance2 = container.GetInstance<IRepresentationTemplateCache>();

            Assert.That(instance, Is.Not.Null);

            Assert.That(ReferenceEquals(instance, instance2), Is.True);
        }

        [Test]
        public void GivenValidConfig_ShouldHaveRegisteredTheHalSerialiserAsSingleton()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            var instance = container.GetInstance<IHalSerialiser>();
            var instance2 = container.GetInstance<IHalSerialiser>();

            Assert.That(instance, Is.Not.Null);

            Assert.That(ReferenceEquals(instance, instance2));
        }

        [Test]
        public void GivenValidConfig_ShouldHaveRegisteredTheStringReplacerAsSingleton()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            var instance = container.GetInstance<IStringReplacer>();
            var instance2 = container.GetInstance<IStringReplacer>();

            Assert.That(instance, Is.Not.Null);

            Assert.That(ReferenceEquals(instance, instance2));
        }

        [Test]
        public void GivenValidConfig_ShouldHaveRegisteredTheUrlParameterSubstituterAsSingleton()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            var instance = container.GetInstance<IUrlParameterSubstituter>();
            var instance2 = container.GetInstance<IUrlParameterSubstituter>();

            Assert.That(instance, Is.Not.Null);

            Assert.That(ReferenceEquals(instance, instance2));
        }

        [Test]
        public void GivenValidConfig_ShouldHaveRegisteredTheQueryCoordinatorAsSingleton()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            var instance = container.GetInstance<IQueryCoordinator>();
            var instance2 = container.GetInstance<IQueryCoordinator>();

            Assert.That(instance, Is.Not.Null);

            Assert.That(ReferenceEquals(instance, instance2));

        }
    }
}
