using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;
using SAHL.Core.Testing.FileConventions;
using SAHL.Core.Testing.Ioc;
using SAHL.Core.Testing.Ioc.Registration;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Testing
{
    public class ConventionTestSuite: IConventionTestSuite
    {
        protected ITestingIoc testingIoc;
        private int testCount;
        protected AssemblyDefinition assemblyDefinition;

        [TestFixtureSetUp]
        public void Initialise()
        {
            testingIoc = TestingIoc.Initialise();
            testingIoc.Configure<SAHLServiceAssemblyConvention, ServiceCommandHandlerConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, InternalServiceCommandHandlerForDomainConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, DomainServiceCommandHandlerConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, ServiceManagerConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, ServiceCommandConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, ServiceQueryConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, SqlStatementConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, ServiceQuerySqlStatementConvention>();
            testingIoc.Configure<SAHLDataModelsAssemblyConvention, SqlStatementConvention>();
            testingIoc.Configure<SAHLDataModelsAssemblyConvention, ServiceQuerySqlStatementConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, DomainRuleConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, DomainDataManagerConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, LegacyEventQueryHandlerConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, LegacyEventQueryConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, LegacyEventConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, DomainProcessManagerModelConvention>();
            testingIoc.Configure<SAHLServiceAssemblyConvention, EventConvention>();
        }

        public void Run(string testSuiteName, string testItemName, ITestParams testParams, Action testToRun)
        {
            this.assemblyDefinition = AssemblyDefinition.ReadAssembly(testParams.TypeUnderTest.Assembly.Location);
            var task = default(Task);
            try
            {
                task = Task.Factory.StartNew(() =>
                {
                    var runTime = this.Run(testToRun, testSuiteName, testItemName, testParams);
                    if (this.IsTimeout(runTime))
                    {
                        throw new Exception(this.CreateMessage(testSuiteName,testItemName,testParams.TypeUnderTest,testCount,runTime, String.Format("Test timed out after {0} ms.", runTime)));
                    }
                });
                task.Wait();
            }
            catch
            {
                throw task.Exception.InnerException;
            }
        }

        private long Run(Action testToRun, string testSuiteName, string testItemName, ITestParams testParams)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                testCount++;
                testToRun();
                watch.Stop();
                return watch.ElapsedMilliseconds;
            }
            catch (Exception e)
            {
                throw new Exception(this.CreateMessage(testSuiteName, testItemName, testParams.TypeUnderTest, testCount, watch.ElapsedMilliseconds, e.Message));
            }
        }

        private string CreateMessage(string testSuiteName, string testItemName, Type typeUnderTest, int testCount, long runTime, string errorMessage)
        {
            var template = "The test {0}.{1} failed to execute in assembly {2}, \nThe error was \"{3}\", \nTotal Test Count: {4}";
            template = String.Format(template, testSuiteName, testItemName, typeUnderTest.Assembly.GetName(), errorMessage, testCount);
            return template;
        }

        private bool IsTimeout(long runTime)
        {
            return runTime > 10000;
        }

        protected ParameterDefinition AssertParameterDefinition(MethodDefinition expectedMethod, string expectedTypeName)
        {
            var paramDefinition = this.GetParameterDefinition(expectedMethod, expectedTypeName);
            if (paramDefinition == null)
            {
                Assert.Fail("Expected method: {0} to have a parameter with type: {1}", expectedMethod.Name, expectedTypeName);
            }
            return paramDefinition;
        }

        protected ParameterDefinition GetParameterDefinition(MethodDefinition expectedMethod, string expectedTypeName)
        {
            foreach (var param in expectedMethod.Parameters)
            {
                if (param.ParameterType.Name.Contains(expectedTypeName))
                {
                    return param;
                }
            }
            return null;
        }

        protected List<MethodReference> AssertMethodBodyForAtLeastOneReference(MethodDefinition expectedMethod, params ExpectedReference[] expectedReferences)
        {
            var foundReferences = new List<MethodReference>();
            foreach (var ins in expectedMethod.Body.Instructions)
            {
                if (ins.OpCode == OpCodes.Callvirt)
                {
                    var reference = (MethodReference)ins.Operand;

                    foreach (var expectedReference in expectedReferences)
                    {
                        var typeFullName = reference.DeclaringType.FullName;
                        var expectedBodyMethodName = expectedReference.ExpectedBodyMethodName;
                        var expectedTypeInMethodBody = expectedReference.ExpectedTypeInMethodBody;
                        var expectedReturnTypeOfBodyMethod = expectedReference.ExpectedReturnTypeOfBodyMethod;

                        if (typeFullName.Contains(expectedTypeInMethodBody) && reference.Name == expectedBodyMethodName && reference.ReturnType.Name == expectedReturnTypeOfBodyMethod)
                        {
                            foundReferences.Add(reference);
                        }
                    }
                }
            }
            if (foundReferences.Count > 0)
            {
                return foundReferences;
            }
            else
            {
                var expectedReferencesStr = string.Empty;
                foreach (var expectedReference in expectedReferences)
                {
                    expectedReferencesStr += String.Format("{0} \n ", expectedReference.ToString());
                }
                var message = String.Format("Expected the body of method: {0} to have at least one reference to the following: \n {1}", expectedMethod.Name, expectedReferencesStr);
                Assert.Fail(message);
            }
            return foundReferences;
        }

        protected MethodDefinition AssertMethodDefinition(TypeDefinition expectedType, bool isConstrutor, string methodName="")
        {
            var matchingMethodName = String.Empty;
            if (String.IsNullOrEmpty(methodName) && isConstrutor)
            {
                matchingMethodName = "Ctor";
            }
            else
            {
                matchingMethodName = methodName;
            }
            foreach (var method in expectedType.Methods)
            {
                if (isConstrutor && method.IsConstructor)
                {
                    return method;
                }
                else if (method.Name == matchingMethodName)
                {
                    return method;
                }
            }
            Assert.Fail("Expected type: {0} to have a method called {1}", expectedType.FullName, methodName);
            return null;
        }

        protected TypeDefinition AssertTypeDefinition(Type expectedType)
        {
            foreach (var type in this.assemblyDefinition.Modules[0].Types)
            {
                if (expectedType.FullName == type.FullName) {
                    return type;
                }
            }
            Assert.Fail("Expected assembly: {0} to have a type called {1}", this.assemblyDefinition.FullName,expectedType.FullName);
            return null;
        }

    }
    public class ExpectedReference
    {
        public ExpectedReference(string expectedTypeInMethodBody, string expectedBodyMethodName, string expectedReturnTypeOfBodyMethod)
        {
            this.ExpectedTypeInMethodBody = expectedTypeInMethodBody;
            this.ExpectedBodyMethodName = expectedBodyMethodName;
            this.ExpectedReturnTypeOfBodyMethod = expectedReturnTypeOfBodyMethod;
        }

        public string ExpectedTypeInMethodBody { get; private set; }
        public string ExpectedBodyMethodName { get; private set; }
        public string ExpectedReturnTypeOfBodyMethod { get; private set; }

        public override string ToString()
        {
            return String.Format("Type: {0}, Method: {1}, ReturnType: {2}", ExpectedTypeInMethodBody,ExpectedBodyMethodName,ExpectedReturnTypeOfBodyMethod);
        }
    }
}