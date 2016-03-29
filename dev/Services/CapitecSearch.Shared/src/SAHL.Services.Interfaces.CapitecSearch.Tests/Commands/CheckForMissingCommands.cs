using NUnit.Framework;
using SAHL.Services.Interfaces.CapitecSearch.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.CapitecSearch.Tests.Commands
{
    [TestFixture]
    public partial class CommandConventions
    {
        [Test, TestCaseSource(typeof(CommandProvider), "GetCapitecSearchServiceCommandNames")]
        public void CheckForMissingCommands(string commandName)
        {
            var assembly = Assembly.GetAssembly(typeof(RefreshLuceneIndexCommand));

            List<Type> types = assembly.GetExportedTypes().Where(x => x.Name == commandName).ToList();
            Assert.That(types.Count >= 1);
        }
    }
}