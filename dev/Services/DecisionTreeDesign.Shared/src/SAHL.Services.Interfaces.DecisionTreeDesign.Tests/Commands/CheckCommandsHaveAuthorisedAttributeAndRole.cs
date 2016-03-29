using NUnit.Framework;
using SAHL.Core.Services.Attributes;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Tests.Commands
{
    [TestFixture]
    public partial class CommandConventions
    {
        [Test, TestCaseSource(typeof(CommandProvider), "GetServiceCommandNames")]
        public void CheckCommandsHaveAuthorisedAttributeAndRole(string commandName)
        {
            var assembly = Assembly.GetAssembly(typeof(SaveDecisionTreeAsCommand));
            var type = assembly.GetTypes().First(x => x.Name == commandName);

            if (!commandName.ToLower().Contains("login"))
            {
                // Check for existence of any custom attribute.
                if (type.CustomAttributes.Count() == 0)
                {
                    Assert.Fail(String.Format("{0} has no attributes defined.", commandName));
                }
                else
                {
                    // Add the attributes and their associated arguments to a dictionary
                    Dictionary<string, string> attributesAndArguments = new Dictionary<string, string>();
                    foreach (var item in type.CustomAttributes)
                    {
                        attributesAndArguments.Add(item.AttributeType.Name, item.NamedArguments.FirstOrDefault().ToString());
                    }

                    // Check for the existence of the AuthorisedCommand attribute
                    if (!attributesAndArguments.ContainsKey(typeof(AuthorisedCommandAttribute).Name))
                    {
                        Assert.Fail(String.Format("{0} does not have an Authorised attribute defined.", commandName));
                    }
                    else
                    {
                        // As we have the AuthorisedCommand attribute, let's now check that at least one role has been defined.
                        if (!attributesAndArguments["AuthorisedCommandAttribute"].ToLower().Contains("role"))
                        {
                            Assert.Fail(String.Format("{0} must have at least one role defined for the Authorised attribute.", commandName));
                        }
                    }
                }
            }
        }
    }
}