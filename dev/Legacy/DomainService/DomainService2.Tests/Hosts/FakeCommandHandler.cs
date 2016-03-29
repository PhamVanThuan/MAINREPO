using System;
using System.Linq;
using System.Reflection;

namespace DomainService2.Tests.Hosts
{
    public class FakeCommandHandler : ICommandHandler
    {
        public void HandleCommand<T>(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, T command) where T : IDomainServiceCommand
        {
            TypeValueProvider typeValueProvider = new TypeValueProvider();
            // set all the settable property values
            Type commandType = command.GetType();
            PropertyInfo[] properties = commandType.GetProperties().Where(x => x.CanWrite).ToArray();
            foreach (PropertyInfo prop in properties)
            {
                prop.SetValue(command, typeValueProvider.GetValueForType(prop.PropertyType), new object[] { });
            }
        }

        public bool CheckRules<T>(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, T command) where T : RuleSetDomainServiceCommand
        {
            throw new NotImplementedException();
        }

        public bool CheckRule<T>(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, T command) where T : RuleDomainServiceCommand
        {
            throw new NotImplementedException();
        }
    }
}