using NUnit.Framework;
using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.CapitecSearch.Tests.Models
{
    [TestFixture]
    public class ModelConventions
    {
        [Test, TestCaseSource(typeof(ModelProvider), "GetCapitecSearchServiceModelNames")]
        public void CheckModelsDoNotImplementIServiceQueryResult(Type modelType)
        {
            if (modelType != null)
            {
                Assert.Fail(String.Format("{0} should not implement interface {1}", modelType.Name.ToString(), typeof(IServiceQueryResult).Name.ToString()));
            }
        }
    }
}