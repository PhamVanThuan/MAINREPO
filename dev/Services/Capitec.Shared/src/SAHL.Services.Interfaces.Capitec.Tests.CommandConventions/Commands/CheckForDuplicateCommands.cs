using NUnit.Framework;
using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Tests.Commands
{
    [TestFixture]
    public partial class CommandConventions
    {
        [Test, TestCaseSource(typeof(CommandProvider), "GetCapitecServiceCommandNames")]
        public void CheckForDuplicateCommands(string commandName)
        {
            var assembly = Assembly.GetAssembly(typeof(LoginCommand));

            List<Type> types = assembly.GetExportedTypes().Where(x => x.Name == commandName).ToList();
            Assert.That(types.Count <= 1);
        }
    }
}
