using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.IOC;
using DomainService2.Workflow.Cap2;
using DomainService2.Workflow.PersonalLoan;
using NUnit.Framework;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Tests.CommandHandlers
{
    [TestFixture]
    public class CanInstantiateWithIOC
    {
        [Test]
        [Ignore("this is the new do shit!")]
        public void testMethod()
        {
            IDomainMessageCollection messages = new DomainMessageCollection();
            DomainServiceLoader.ProcessName = "NUnitTestMap";
            var domainServiceIOC = DomainServiceLoader.Instance.DomainServiceIOC;

            var command = new CheckUnderDebtCounsellingRuleCommand(1190639, false);
            var handler = domainServiceIOC.GetCommandHandler<CheckUnderDebtCounsellingRuleCommand>();

            handler.Handle(messages, command);
        }

        [Test, TestCaseSource(typeof(CommandSource), "GetCommands")]
        public void CommandHandler_When_Given_Command(Type command)
        {
            IDomainMessageCollection messages = new DomainMessageCollection();
            var domainServiceIOC = DomainServiceLoader.Instance.DomainServiceIOC;
            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));
            MethodInfo method = domainServiceIOC.GetType().GetMethod("GetCommandHandler");
            var genericGetCommandHandler = method.MakeGenericMethod(command);
            var handlerFromIOC = genericGetCommandHandler.Invoke(domainServiceIOC, null);
            if (handlerFromIOC == null)
            {
                Assert.Fail(String.Format("Could not get {0}Handler from the IOC", command.Name));
            }
        }

        private class CommandSource
        {
            public static IEnumerable GetCommands()
            {
                SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages = new DomainMessageCollection();
                var domainServiceIOC = DomainServiceLoader.Instance.DomainServiceIOC;
                var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));
                var commands = GetAllCommands(assembly, typeof(IStandardDomainServiceCommand));
                commands.Remove(typeof(DomainServiceWithExclusionSetCommand));
                return commands;
            }
        }

        private static IList<Type> GetAllCommands(Assembly assembly, Type interfaceType)
        {
            return assembly.GetTypes().Where(x =>
                                !x.IsInterface &&
                                !x.IsGenericType &&
                                !x.IsAbstract &&
                                x.GetInterfaces().Where(i => i.Name == interfaceType.Name).Count() > 0
                              ).ToList();
        }

        private static Type GetCommandHandler(Assembly assembly, Type command)
        {
            var handler = assembly.GetTypes().FirstOrDefault(x => x.Name == command.Name + "Handler");
            return handler;
        }
    }
}