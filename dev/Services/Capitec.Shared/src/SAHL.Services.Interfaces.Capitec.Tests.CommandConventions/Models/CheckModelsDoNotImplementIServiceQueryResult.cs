using NUnit.Framework;
using SAHL.Services.Interfaces.Capitec.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.Capitec.Tests.Models
{
    [TestFixture]
    public class ModelConventions
    {
        [Test, TestCaseSource(typeof(ModelProvider), "GetCapitecServiceModelNames")]
        public void CheckModelsDoNotImplementIServiceQueryResult(Type modelType)
        {
            if (modelType != null)
            {
                Assert.Fail(String.Format("{0} should not implement interface {1}", modelType.Name.ToString(), typeof(IServiceQueryResult).Name.ToString()));
            } 
        } 
    }
}