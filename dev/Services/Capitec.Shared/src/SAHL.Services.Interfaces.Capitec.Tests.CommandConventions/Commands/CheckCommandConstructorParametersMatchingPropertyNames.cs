using NUnit.Framework;
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
        [Test, TestCaseSource(typeof(CommandProvider), "GetCapitecServiceCommandTypes")]
        public void CheckCommandConstructorParametersMatchingPropertyNames(Type commandType)
        {
            // get the constructor
            ConstructorInfo mi = commandType.GetConstructors().Single();

            // get the constructor args
            ParameterInfo[] pis = mi.GetParameters();

            // get the command properties
            PropertyInfo[] props = commandType.GetProperties();

            foreach (ParameterInfo pi in pis)
            {
                string paramName = pi.Name.ToLower();
                PropertyInfo prop = props.Where(x => x.Name.ToLower() == paramName).Single();
                Assert.That(prop != null);
            }
        }
    }
}
