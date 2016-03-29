using NUnit.Framework;
using SAHL.Core.BusinessModel;
using SAHL.Core.Testing;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Tests.HaloUIConfigPredicates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.UI.Halo.Tests.HaloUIConfig
{
    [TestFixture]
    public class DataProviderTests : HaloUIConfigTests
    {
        [Test]
        public void NonDynamicTilesWithContentDataProvidersTest()
        {
            //---------------Set up test pack-------------------
            var configurations = new Dictionary<string, string>();
            var expectedProviderConfigName = String.Empty;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse<IsNonDynamicTilesWithContentDataProvidersPredicate>((expectedConfig) =>
            {
                this.DataProviderTests(expectedConfig, configurations, dataContentProviderConfSuffix, out expectedProviderConfigName, this.contentDataProviders);
            });
            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }

        [Test]
        public void NonDynamicTilesWithDataProvidersTest()
        {
            //---------------Set up test pack-------------------
            var configurations = new Dictionary<string, string>();
            var expectedProviderConfigName = String.Empty;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse<IsNonDynamicTilesWithDataProvidersPredicate>((expectedConfig) =>
            {
                this.DataProviderTests(expectedConfig, configurations, dataProviderConfSuffix, out expectedProviderConfigName, this.dataProviderTypes);
            });

            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }

        [Test]
        public void NonDynamicTilesWithoutAnyDataProvidersTest()
        {
            //---------------Set up test pack-------------------
            var configurations = new Dictionary<string, string>();
            var messageTemplate = "DataProvider: {0}";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            this.haloUIConfigTree.Traverse<IsNonDynamicTilesWithoutAnyDataProvidersPredicate>((expectedConfig) =>
            {
                var expectedConfigName = String.Format("{0}{1}", expectedConfig.Name, expectedConfig.Type);
                var expected = String.Format(messageTemplate, "A data provider specified for the {0} tile in the HaloUIConfig document");
                expected = String.Format(expected, expectedConfigName);
                var actual = String.Format(messageTemplate, "No data provider was specified for the {0} tile in the HaloUIConfig document");
                actual = String.Format(actual, expectedConfigName);
                if (!configurations.ContainsKey(expected))
                {
                    configurations.Add(expected, actual);
                }
            });
            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }

        [Test]
        public void ExecuteContentDataProvidersTest()
        {
            //---------------Set up test pack-------------------
            var configurations = new Dictionary<string, string>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var contentDataProviderType in this.contentDataProviders.GetRegisteredTypes())
            {
                this.ExecuteDataProvider(configurations, contentDataProviderType, () =>
                {
                    var provider = base.testingIoc.GetInstance(contentDataProviderType);
                    var businessKey = new BusinessKey(123, Core.BusinessModel.Enums.GenericKeyType.Account);
                    var businessContext = new BusinessContext("42087", businessKey);
                    if (provider is IHaloTileContentDataProvider)
                    {
                        (provider as IHaloTileContentDataProvider).Load(businessContext);
                    }
                    if (provider is IHaloTileContentMultipleDataProvider)
                    {
                        (provider as IHaloTileContentMultipleDataProvider).Load(businessContext);
                    }
                });
            }
            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }

        [Test]
        public void ExecuteDataProvidersTest()
        {
            //---------------Set up test pack-------------------
            var configurations = new Dictionary<string, string>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var dataProviderType in this.dataProviderTypes.GetRegisteredTypes())
            {
                this.ExecuteDataProvider(configurations, dataProviderType, () =>
                {
                    var provider = (IHaloTileDataProvider)base.testingIoc.GetInstance(dataProviderType);
                    var businessKey = new BusinessKey(55839, Core.BusinessModel.Enums.GenericKeyType.Account);
                    var businessContext = new BusinessContext("'55839'", businessKey);
                    var contentSqlStatement = provider.GetSqlStatement(businessContext);
                    var sqlStatementInfo = new SqlStatementInfo(contentSqlStatement);

                    if (!string.IsNullOrEmpty(contentSqlStatement))
                    {
                        switch (sqlStatementInfo.StatementType)
                        {
                            case SqlStatementType.Select:
                                using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
                                {
                                    db.Select<HaloTileBusinessModel>(contentSqlStatement);
                                }
                                break;

                            case SqlStatementType.None:
                                break;

                            default:
                                var interfaces = dataProviderType.GetInterfaces();
                                var dataModelType = interfaces.FirstOrDefault().GetGenericArguments().FirstOrDefault();
                                var dataModel = Activator.CreateInstance(dataModelType);

                                using (var db = this.dbFactory.NewDb().InAppContext())
                                {
                                    db.ExecuteSqlStatement(contentSqlStatement, dataModel);
                                }
                                break;
                        }
                    }
                });
            }

            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }

        [Test]
        public void ExecuteDynamicDataProvidersTest()
        {
            //---------------Set up test pack-------------------
            var configurations = new Dictionary<string, string>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            foreach (var dataProviderType in this.dynamicDataProviderTypes.GetRegisteredTypes())
            {
                this.ExecuteDataProvider(configurations, dataProviderType, () =>
                {
                });
            }
            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }
    }
}