using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using SAHL.Core.UI.Halo.Tiles.LegalEntity.Default;
using SAHL.Core.UI.Models;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tests
{
    [TestFixture]
    public class ConventionTests
    {
        [Test, TestCaseSource(typeof(ConventionTests), "GetTileModels")]
        public void EnsureThatASingleDataProviderExists(TileModelTypeDefinition tileModels)
        {
            IEnumerable<Type> tileDataProviders = GetProviders(tileModels, typeof(ITileDataProvider<>));
            Assert.That(tileDataProviders.Count() == 1, "{0} has {1} providers configured. Expected a single DataProvider", tileModels.TileModelType, tileDataProviders.Count());
        }

        [Test, TestCaseSource(typeof(ConventionTests), "GetTileModels")]
        public void EnsureThatASingleContentProviderExists(TileModelTypeDefinition tileModels)
        {
            IEnumerable<Type> tileContentProviders = GetProviders(tileModels, typeof(ITileContentProvider<>));
            Assert.That(tileContentProviders.Count() == 1, "{0} has {1} providers configured. Expected a single ContentProvider", tileModels.TileModelType, tileContentProviders.Count());
        }

        [Test, TestCaseSource(typeof(ConventionTests), "GetTileModels")]
        public void EnsureThatDataProviderIsCorrectlyNamed(TileModelTypeDefinition tileModels)
        {
            var tileDataProvider = GetProviders(tileModels, typeof(ITileDataProvider<>)).SingleOrDefault();
            if (tileDataProvider != null)
            {
                string expectedDataProviderName = tileModels.TileModelType.Name.Replace("Model", "DataProvider");
                Assert.AreEqual(expectedDataProviderName, tileDataProvider.Name, string.Format("{0} is incorrectly named for {1}. It should be {2}. ", tileDataProvider.Name,
                    tileModels.TileModelType.Name, expectedDataProviderName));
            }
        }

        [Test, TestCaseSource(typeof(ConventionTests), "GetTileModels")]
        public void EnsureThatContentProviderIsCorrectlyNamed(TileModelTypeDefinition tileModels)
        {
            var contentProvider = GetProviders(tileModels, typeof(ITileContentProvider<>)).SingleOrDefault();
            if (contentProvider != null)
            {
                string expectedContentProviderName = tileModels.TileModelType.Name.Replace("Model", "ContentProvider");
                Assert.AreEqual(expectedContentProviderName, contentProvider.Name, string.Format("{0} is incorrectly named for {1}. It should be {2}. ", contentProvider.Name,
                    tileModels.TileModelType.Name, expectedContentProviderName));
            }
        }

        [Test, TestCaseSource(typeof(ConventionTests), "GetAlternateTileModels")]
        public void EnsureThatAlternateTileModelInheritsFromMatchingTileModel(TileModelTypeDefinition alternateTileModels)
        {
            var matchingTileModel = (from tileModels in alternateTileModels.AllTypes
                                     where tileModels.Name == alternateTileModels.TileModelType.Name
                                     && (typeof(ITileModel).IsAssignableFrom(tileModels))
                                     && !(typeof(IAlternateTileModel).IsAssignableFrom(tileModels))
                                     select tileModels).SingleOrDefault();
            Assert.NotNull(matchingTileModel, string.Format("No matching tile model exists for {0}", alternateTileModels.TileModelType.Name));

            bool alternateTileModelInheritsFromTileModel = alternateTileModels.TileModelType
                                                            .GetInterfaces()
                                                            .Any(x => x.IsGenericType && x.GetGenericArguments()
                                                            .Contains(matchingTileModel));
            Assert.That(alternateTileModelInheritsFromTileModel, string.Format("{0} does not inherit from it matching tile model", alternateTileModels.TileModelType.Name));
        }

        public List<TileModelTypeDefinition> GetTileModels()
        {
            return this.GetTileModelsByType(typeof(ITileModel));
        }

        public List<TileModelTypeDefinition> GetAlternateTileModels()
        {
            return this.GetTileModelsByType(typeof(IAlternateTileModel));
        }

        public class TileModelTypeDefinition
        {
            public Type TileModelType { get; protected set; }

            public List<Type> AllTypes { get; protected set; }

            public TileModelTypeDefinition(IEnumerable<Type> allTypes, Type tileModelType)
            {
                this.TileModelType = tileModelType;
                this.AllTypes = new List<Type>(allTypes);
            }

            public override string ToString()
            {
                return this.TileModelType.FullName.Replace("SAHL.Core.UI.Halo.Tiles", "");
            }
        }

        private IEnumerable<Type> GetProviders(TileModelTypeDefinition tileModelTypeDefinition, Type openGenericProviderType)
        {
            Type genericProviderType = openGenericProviderType.MakeGenericType(tileModelTypeDefinition.TileModelType);
            var providers = tileModelTypeDefinition.AllTypes.Where(x => x.GetInterfaces().Any(y => y.Equals(genericProviderType)));
            return providers;
        }

        private List<TileModelTypeDefinition> GetTileModelsByType(Type type)
        {
            Assembly sahl_core_ui_halo_assembly = Assembly.GetAssembly(typeof(LegalEntityMajorTileContentProvider));
            Type[] allTypes = sahl_core_ui_halo_assembly.GetTypes();
            List<TileModelTypeDefinition> tileModelTypeDefinitions = new List<TileModelTypeDefinition>();
            var tileModels = sahl_core_ui_halo_assembly.GetTypes().Where(x =>
                   x.GetInterfaces().Any(y => y.Equals(type)
                   && x.IsAbstract == false && !typeof(IActionTileModel).IsAssignableFrom(x))
               ).ToList();
            foreach (var tileModel in tileModels)
            {
                tileModelTypeDefinitions.Add(new TileModelTypeDefinition(allTypes, tileModel));
            }
            return tileModelTypeDefinitions;
        }
    }
}