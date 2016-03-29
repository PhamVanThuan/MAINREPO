using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Core.Web.Tests
{
    [TestFixture]
    public class TestServiceHttpClient
    {
        [Test]
        public void Constructor_GivenServiceUrlConfiguration_ShouldNotBeNull()
        {
            //---------------Set up test pack-------------------
            var serviceUrlConfiguration = Substitute.For<IServiceUrlConfiguration>();
            var jsonActivator = Substitute.For<IJsonActivator>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var serviceHttpClient = new FakeServiceHttpClient(serviceUrlConfiguration, jsonActivator);
            //---------------Test Result -----------------------
            Assert.IsNotNull(serviceHttpClient);
        }

        [Test]
        public void Constructor_GivenServiceUrlConfigurationProvider_ShouldNotBeNull()
        {
            //---------------Set up test pack-------------------
            var configurationProvider = Substitute.For<IServiceUrlConfigurationProvider>();
            var jsonActivator = Substitute.For<IJsonActivator>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var serviceHttpClient = new FakeServiceHttpClient(configurationProvider, jsonActivator);
            //---------------Test Result -----------------------
            Assert.IsNotNull(serviceHttpClient);
        }

        [Test]
        public void Constructor_GivenServiceUrlConfigurationAndNullJsonActivator_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var serviceUrlConfiguration = Substitute.For<IServiceUrlConfiguration>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new FakeServiceHttpClient(serviceUrlConfiguration, null));
            //---------------Test Result -----------------------
            Assert.AreEqual("jsonActivator", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenServiceUrlConfigurationProviderAndNullJsonActivator_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var configurationProvider = Substitute.For<IServiceUrlConfigurationProvider>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new FakeServiceHttpClient(configurationProvider, null));
            //---------------Test Result -----------------------
            Assert.AreEqual("jsonActivator", exception.ParamName);
        }

        [Test]
        public void GetConfiguredClient_GivenServiceUrlConfiguration_ShouldSetBaseAddressToUrl()
        {
            //---------------Set up test pack-------------------
            var serviceUrlConfiguration = new ServiceUrlConfiguration("TestHost/TestService");
            var serviceHttpClient = this.CreateServiceHttpClient(serviceUrlConfiguration);
            var expectedUri = new Uri(serviceUrlConfiguration.GetCommandServiceUrl());
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var httpClient = serviceHttpClient.RetrieveWebHttpClient();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedUri, httpClient.BaseAddress);
        }

        [Test]
        public void GetConfiguredClient_GivenServiceUrlConfigurationProvider_ShouldSetBaseAddressToProviderUrl()
        {
            //---------------Set up test pack-------------------
            var configurationProvider = new ServiceUrlConfigurationProvider("TestService");
            var serviceHttpClient = this.CreateServiceHttpClient(serviceUrlConfigurationProvider: configurationProvider);
            var expectedUri = new Uri(string.Format("http://{0}/{1}/", configurationProvider.ServiceHostName,
                                                                                 configurationProvider.ServiceName));
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var httpClient = serviceHttpClient.RetrieveWebHttpClient();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedUri, httpClient.BaseAddress);
        }

        [Test]
        public void GetConfiguredClient_GivenAuthenticatioNHeader_ShouldAddDefaultRequestHeadersForAuthentication()
        {
            //---------------Set up test pack-------------------
            var authHeader = "testAuthHeader";
            var configurationProvider = new ServiceUrlConfigurationProvider("TestService");
            var serviceHttpClient = this.CreateServiceHttpClient(serviceUrlConfigurationProvider: configurationProvider);
            serviceHttpClient.UseCustomHeaderAuth(authHeader);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var httpClient = serviceHttpClient.RetrieveWebHttpClient();
            //---------------Test Result -----------------------
            Assert.IsTrue(httpClient.DefaultRequestHeaders.Contains(authHeader));
        }

        [Test]
        public void UseCustomHeaderAuth_GivenAuthenticationHeader_ShouldSetTheAuthenticationHeader()
        {
            //---------------Set up test pack-------------------
            var authHeader = "authHeader";
            var serviceHttpClient = this.CreateServiceHttpClient();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            serviceHttpClient.UseCustomHeaderAuth(authHeader);
            //---------------Test Result -----------------------
            Assert.AreEqual(authHeader, serviceHttpClient.AuthenticationHeader);
        }

        [Test]
        public void UseWindowsAuth_GivenNoPrincipal_ShouldSetTheHttpHandlerWithDefaultCredentials()
        {
            //---------------Set up test pack-------------------
            var serviceHttpClient = this.CreateServiceHttpClient();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            serviceHttpClient.UseWindowsAuth();
            //---------------Test Result -----------------------
            Assert.IsTrue(serviceHttpClient.HttpClientHandler.UseDefaultCredentials);
        }

        [Test]
        public void UseWindowsAuth_GivenWindowsPrincipal_ShouldSetTheHttpHandlerWithCredential()
        {
            //---------------Set up test pack-------------------
            var credentials = Substitute.For<ICredentials>();
            var serviceHttpClient = this.CreateServiceHttpClient();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            serviceHttpClient.UseWindowsAuth(credentials);
            //---------------Test Result -----------------------
            Assert.IsFalse(serviceHttpClient.HttpClientHandler.UseDefaultCredentials);
            Assert.AreEqual(credentials, serviceHttpClient.HttpClientHandler.Credentials);
        }

        [Test]
        public void PerformCommandInternal_GivenCommandAndSuccessfulPost_ShouldReturnResult()
        {
            //---------------Set up test pack-------------------
            var webHttpClient = this.CreateWebHttpClient(HttpStatusCode.OK);
            var jsonActivator = Substitute.For<IJsonActivator>();
            jsonActivator.DeserializeObject<SAHL.Core.Web.Services.ServiceCommandResult>(Arg.Any<string>()).Returns(new SAHL.Core.Web.Services.ServiceCommandResult(SystemMessageCollection.Empty()));

            var serviceHttpClient = this.CreateServiceHttpClient(webHttpClient: webHttpClient, jsonActivator: jsonActivator);
            var testCommand = new FakeTestCommand();
            var metadata = new ServiceRequestMetadata();
            serviceHttpClient.ResultMessage = "Test Result";
            ISystemMessageCollection result;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => result = serviceHttpClient.PerformCommand(testCommand, metadata));

            //---------------Test Result -----------------------
            Assert.AreEqual(serviceHttpClient.ResultMessage, testCommand.ResultMessage);
        }

        [Test]
        public void PerformCommandInternal_GivenCommandAndFailureDuringPost_ShouldNotThrowExceptionWithReasonPhrase()
        {
            //---------------Set up test pack-------------------
            var webHttpClient = this.CreateWebHttpClient(HttpStatusCode.InternalServerError);
            var jsonActivator = Substitute.For<IJsonActivator>();
            jsonActivator.DeserializeObject<SAHL.Core.Web.Services.ServiceCommandResult>(Arg.Any<string>()).Returns(new SAHL.Core.Web.Services.ServiceCommandResult(SystemMessageCollection.Empty()));

            var serviceHttpClient = this.CreateServiceHttpClient(webHttpClient: webHttpClient, jsonActivator: jsonActivator);
            var testCommand = new FakeTestCommand();
            var metadata = new ServiceRequestMetadata();
            serviceHttpClient.ResultMessage = string.Empty;
            Exception exception = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                serviceHttpClient.PerformCommand(testCommand, metadata);
            }
            catch (Exception exc)
            {
                exception = exc;
            }
            //---------------Test Result -----------------------
            Assert.IsNull(exception);
        }

        [Test]
        public void PerformCommandInternal_GivenAdditionalBaseHeadersAreSet_ShouldSetHeaders()
        {
            //---------------Set up test pack-------------------
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("username", "test");
            headers.Add("ipaddress", "::1");

            var configurationProvider = new ServiceUrlConfigurationProvider("TestService");
            var serviceHttpClient = this.CreateServiceHttpClient(serviceUrlConfigurationProvider: configurationProvider);
            serviceHttpClient.AddCustomHeaders(headers);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var httpClient = serviceHttpClient.RetrieveWebHttpClient();
            //---------------Test Result -----------------------
            Assert.AreEqual((httpClient.DefaultRequestHeaders.FirstOrDefault(x => x.Key == "username").Value as string[]).First(), "test");
            Assert.AreEqual((httpClient.DefaultRequestHeaders.FirstOrDefault(x => x.Key == "ipaddress").Value as string[]).First(), "::1");
        }

        [Test]
        public void PerformCommandInternal_GivenHeadersSentWithCommand_ShouldSetHeaders()
        {
            //---------------Set up test pack-------------------
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("username", "test");
            headers.Add("ipaddress", "::1");

            ServiceRequestMetadata extraMeta = new ServiceRequestMetadata();
            extraMeta.Add("bob", "sagget");

            var configurationProvider = new ServiceUrlConfigurationProvider("TestService");
            var serviceHttpClient = this.CreateServiceHttpClient(serviceUrlConfigurationProvider: configurationProvider);
            serviceHttpClient.AddCustomHeaders(headers);
            var testCommand = new FakeTestCommand();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                serviceHttpClient.PerformCommand(testCommand, extraMeta); //ignore errors thrown, as we want to see that the event fires
            }
            catch
            {
                //ignore errors thrown, as we want to see that the headers are there
            }

            var httpClient = serviceHttpClient.RetrieveWebHttpClient();
            //---------------Test Result -----------------------
            Assert.AreEqual((httpClient.DefaultRequestHeaders.FirstOrDefault(x => x.Key == "username").Value as string[]).First(), "test");
            Assert.AreEqual((httpClient.DefaultRequestHeaders.FirstOrDefault(x => x.Key == "ipaddress").Value as string[]).First(), "::1");
            Assert.AreEqual((httpClient.DefaultRequestHeaders.FirstOrDefault(x => x.Key == ServiceHttpClient.MetaHeaderPrefix + "bob").Value as string[]).First(), "sagget");
        }

        [Test]
        public void PerformCommandInternal_GivenCurrentlyPerformingRequestEventIsAttached_ShouldTriggerWithCommandType()
        {
            //---------------Set up test pack-------------------
            Guid correlationMetadataGuid = Guid.NewGuid();
            string correlationKey = "CorrelationGuid";
            var command = new FakeTestCommand();
            var metadata = new ServiceRequestMetadata { { correlationKey, correlationMetadataGuid.ToString() } };

            var webHttpClient = this.CreateWebHttpClient(HttpStatusCode.OK);
            var serviceHttpClient = this.CreateServiceHttpClient(webHttpClient: webHttpClient);

            Type actualCommandType = null;
            Guid actualRequestMetadataGuid = Guid.Empty;
            serviceHttpClient.CurrentlyPerformingRequest += (sender, e) =>
            {
                actualCommandType = e.RequestType;
                actualRequestMetadataGuid = Guid.Parse(e.Metadata[correlationKey]);
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                serviceHttpClient.PerformCommand(command, metadata); //ignore errors thrown, as we want to see that the event fires
            }
            catch
            {
                //ignore errors thrown, as we want to see that the event fires
            }
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(FakeTestCommand), actualCommandType);
            Assert.AreEqual(correlationMetadataGuid, actualRequestMetadataGuid);
        }

        [Test]
        public void PerformQueryInternal_GivenQueryAndSuccessfulPost_ShouldReturnResult()
        {
            //---------------Set up test pack-------------------
            var expectedResult = new FakeTestQueryResult();
            var webHttpClient = this.CreateWebHttpClient(HttpStatusCode.OK);
            var jsonActivator = Substitute.For<IJsonActivator>();
            jsonActivator.DeserializeObject<ServiceQueryResult>(Arg.Any<string>()).Returns(x =>
            {
                return new ServiceQueryResult(SystemMessageCollection.Empty(), expectedResult);
            });
            var serviceHttpClient = this.CreateServiceHttpClient(webHttpClient: webHttpClient, jsonActivator: jsonActivator);
            var query = new FakeTestQuery();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            serviceHttpClient.PerformQuery(query);
            //---------------Test Result -----------------------

            Assert.AreEqual(query.Result, expectedResult);
        }

        [Test]
        public void SetPaginationQueryParameters_ShouldSetPaginationQuereyParameters()
        {
            //---------------Set up test pack-------------------
            var expectedResult = new FakeTestQueryResult();
            var webHttpClient = this.CreateWebHttpClient(HttpStatusCode.OK);
            var jsonActivator = Substitute.For<IJsonActivator>();
            jsonActivator.DeserializeObject<ServiceQueryResult>(Arg.Any<string>()).Returns(x =>
            {
                return new ServiceQueryResult(SystemMessageCollection.Empty(), expectedResult);
            });
            var serviceHttpClient = this.CreateServiceHttpClient(webHttpClient: webHttpClient, jsonActivator: jsonActivator);
            serviceHttpClient.SetPaginationQueryParameters(2, 1);
            var query = new FakeTestQuery();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            serviceHttpClient.PerformQuery(query);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, serviceHttpClient.PaginationQueryParameter.PageSize);
            Assert.AreEqual(1, serviceHttpClient.PaginationQueryParameter.CurrentPage);
            Assert.AreEqual(query.Result, expectedResult);
        }

        [Test]
        public void SetFilterQueryParameters_ShouldSetPaginationQuereyParameters()
        {
            //---------------Set up test pack-------------------
            var filterOn = "filterOn";
            var filterValue = "filterValue";
            var expectedResult = new FakeTestQueryResult();
            var webHttpClient = this.CreateWebHttpClient(HttpStatusCode.OK);
            var jsonActivator = Substitute.For<IJsonActivator>();
            jsonActivator.DeserializeObject<ServiceQueryResult>(Arg.Any<string>()).Returns(x =>
            {
                return new ServiceQueryResult(SystemMessageCollection.Empty(), expectedResult);
            });
            var serviceHttpClient = this.CreateServiceHttpClient(webHttpClient: webHttpClient, jsonActivator: jsonActivator);
            serviceHttpClient.SetFilterQueryParameters(filterOn, filterValue);
            serviceHttpClient.SetPaginationQueryParameters(2, 1);
            var query = new FakeTestQuery();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            serviceHttpClient.PerformQuery(query);
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(filterOn, serviceHttpClient.FilterQueryParameter.FilterOn);
            StringAssert.AreEqualIgnoringCase(filterValue, serviceHttpClient.FilterQueryParameter.FilterValue);
            Assert.AreEqual(query.Result, expectedResult);
        }

        [Test]
        public void SetSortQueryParameters_ShouldSetPaginationQuereyParameters()
        {
            //---------------Set up test pack-------------------
            var expectedResult = new FakeTestQueryResult();
            var webHttpClient = this.CreateWebHttpClient(HttpStatusCode.OK);
            var jsonActivator = Substitute.For<IJsonActivator>();
            jsonActivator.DeserializeObject<ServiceQueryResult>(Arg.Any<string>()).Returns(x =>
            {
                return new ServiceQueryResult(SystemMessageCollection.Empty(), expectedResult);
            });
            var serviceHttpClient = this.CreateServiceHttpClient(webHttpClient: webHttpClient, jsonActivator: jsonActivator);
            var orderBy = "orderBy";
            var sortDirectionOptions = SortQueryParameter.SortDirectionOptions.Ascending;
            serviceHttpClient.SetSortQueryParameters(orderBy, sortDirectionOptions);
            var query = new FakeTestQuery();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            serviceHttpClient.PerformQuery(query);
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(orderBy, serviceHttpClient.SortQueryParameter.OrderBy);
            Assert.AreEqual(sortDirectionOptions, serviceHttpClient.SortQueryParameter.SortDirection);
            Assert.AreEqual(query.Result, expectedResult);
        }

        [Test]
        public void PerformQueryInternal_GivenQueryAndFailPost_Does_Not_ThrowError()
        {
            //---------------Set up test pack-------------------
            var expectedResult = new FakeTestQueryResult();
            var webHttpClient = this.CreateWebHttpClient(HttpStatusCode.InternalServerError);
            var jsonActivator = Substitute.For<IJsonActivator>();
            jsonActivator.DeserializeObject<ServiceQueryResult>(Arg.Any<string>()).Returns(x =>
            {
                return new ServiceQueryResult(SystemMessageCollection.Empty(), expectedResult);
            });
            var serviceHttpClient = this.CreateServiceHttpClient(webHttpClient: webHttpClient, jsonActivator: jsonActivator);
            var query = new FakeTestQuery();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>
            {
                serviceHttpClient.PerformQuery(query);
            });
            //---------------Test Result -----------------------
        }

        [Test]
        public void PerformQueryInternal_GivenCurrentlyPerformingRequestEventIsAttached_ShouldTriggerWithQueryType()
        {
            //---------------Set up test pack-------------------
            var query = new FakeTestQuery();

            var webHttpClient = this.CreateWebHttpClient(HttpStatusCode.OK);
            var serviceHttpClient = this.CreateServiceHttpClient(webHttpClient: webHttpClient);

            Type actualQueryType = null;
            serviceHttpClient.CurrentlyPerformingRequest += (sender, e) =>
            {
                actualQueryType = e.RequestType;
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                serviceHttpClient.PerformQuery(query);//ignore errors thrown, as we want to see that the event fires
            }
            catch
            {
                //ignore errors thrown, as we want to see that the event fires
            }
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(FakeTestQuery), actualQueryType);
        }

        private FakeServiceHttpClient CreateServiceHttpClient(IServiceUrlConfiguration serviceUrlConfiguration = null,
                                                              IServiceUrlConfigurationProvider serviceUrlConfigurationProvider = null,
                                                              IJsonActivator jsonActivator = null,
                                                              IWebHttpClient webHttpClient = null)
        {
            var jsonActivatorInternal = jsonActivator ?? Substitute.For<IJsonActivator>();

            FakeServiceHttpClient serviceHttpClient;
            if (serviceUrlConfiguration != null)
            {
                serviceHttpClient = new FakeServiceHttpClient(serviceUrlConfiguration, jsonActivatorInternal);
            }
            else
            {
                var serviceUrlConfigurationProviderInternal = serviceUrlConfigurationProvider ?? new ServiceUrlConfigurationProvider("TestService");
                serviceHttpClient = new FakeServiceHttpClient(serviceUrlConfigurationProviderInternal, jsonActivatorInternal);
            }

            if (webHttpClient != null)
            {
                serviceHttpClient.WebHttpClient = webHttpClient;
            }

            return serviceHttpClient;
        }

        private IWebHttpClient CreateWebHttpClient(HttpStatusCode httpStatusCode, string reasonPhrase = "", string exceptionMessage = "")
        {
            var webHttpClient = Substitute.For<IWebHttpClient>();
            var jsonActivator = new JsonActivator();
            var systemMessageCollection = new SystemMessageCollection();

            if (httpStatusCode != HttpStatusCode.OK)
            {
                systemMessageCollection.AddMessage(new SystemMessage(exceptionMessage, SystemMessageSeverityEnum.Exception));
            }

            var stringContent = new StringContent(jsonActivator.SerializeObject(systemMessageCollection), Encoding.UTF8, "application/json");
            var httpResponseMessage = new HttpResponseMessage(httpStatusCode)
            {
                Content = stringContent,
                ReasonPhrase = reasonPhrase,
            };

            webHttpClient.PostAsync(Arg.Any<string>(), Arg.Any<HttpContent>())
                         .Returns(info =>
                             {
                                 var tcs = new TaskCompletionSource<HttpResponseMessage>();
                                 tcs.SetResult(httpResponseMessage);
                                 return tcs.Task;
                             });

            return webHttpClient;
        }
    }
}
