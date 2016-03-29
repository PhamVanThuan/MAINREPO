using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

namespace SAHL.Core.Testing
{
    public class ConstructorTestUtils
    {
        public static void CheckForExceptionWhenParameterIsNull<T>(string parameterName)
        {
            var constructorInfo       = RetrieveConstructorInfo(typeof(T));
            var constructorParameters = RetrieveConstructorParameters(constructorInfo);

            if (IsParameterNameInConstructorParameterList(parameterName, constructorParameters))
            {
                var parameterValues = CreateParameterValues(constructorParameters, parameterName);
                var thrownException = InvokeConstructor(constructorInfo, parameterValues);

                var argumentNullException = AssertArgumentNullExceptionWasThrown(thrownException);
                Assert.AreEqual(parameterName, argumentNullException.ParamName);
            }
            else
            {
                Assert.Fail("The constructor for {0} didn't contain a parameter with the name '{1}'.", constructorInfo.Name, parameterName);
            }
        }

        private static ConstructorInfo RetrieveConstructorInfo(Type objectType)
        {
            var allConstructors = objectType.GetConstructors();
            return !allConstructors.Any()
                        ? null
                        : allConstructors.FirstOrDefault();
        }

        private static List<ParameterInfo> RetrieveConstructorParameters(ConstructorInfo constructorInfo)
        {
            return constructorInfo.GetParameters().ToList();
        }

        private static List<object> CreateParameterValues(IEnumerable<ParameterInfo> allParameters, string nullParameterName = "")
        {
            var parameterValues = new List<object>();

            foreach (var currentParameter in allParameters)
            {
                var parameterValue = currentParameter.Name == nullParameterName
                                            ? null
                                            : CreateParameterValue(currentParameter);
                parameterValues.Add(parameterValue);
            }

            return parameterValues;
        }

        private static bool IsParameterNameInConstructorParameterList(string parameterName, IEnumerable<ParameterInfo> constructorParameters)
        {
            var constructorParameter = constructorParameters.FirstOrDefault(info => info.Name == parameterName);
            return constructorParameter != null;
        }

        private static object SubstituteObject(Type objectType)
        {
            var constructorInfo       = RetrieveConstructorInfo(objectType);
            var constructorParameters = RetrieveConstructorParameters(constructorInfo);

            if (constructorParameters.Any())
            {
                var parameterValues = CreateParameterValues(constructorParameters);
                return Substitute.For(new[] { objectType }, parameterValues.ToArray());
            }
            else
            {
                return Substitute.For(new[] { objectType }, new object[0]);
            }
        }

        private static object CreateParameterValue(ParameterInfo currentParameter)
        {
            object parameterValue;

            if (currentParameter.ParameterType == typeof(string))
            {
                parameterValue = Guid.NewGuid().ToString();
            }
            else if (currentParameter.ParameterType == typeof(int) || currentParameter.ParameterType == typeof(long))
            {
                parameterValue = 1234;
            }
            else if (currentParameter.ParameterType.IsClass)
            {
                parameterValue = SubstituteObject(currentParameter.ParameterType);
            }
            else if (currentParameter.ParameterType.IsEnum)
            {
                parameterValue = Enum.GetValues(currentParameter.ParameterType).GetValue(0);
            }
            else
            {
                parameterValue = Substitute.For(new[] { currentParameter.ParameterType }, new object[0]);
            }

            return parameterValue;
        }

        private static Exception InvokeConstructor(ConstructorInfo constructor, IEnumerable<object> parameterValues)
        {
            try
            {
                constructor.Invoke(parameterValues.ToArray());
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private static ArgumentNullException AssertArgumentNullExceptionWasThrown(Exception thrownException)
        {
            var targetInvocationException = thrownException as TargetInvocationException;
            if (targetInvocationException == null)
            {
                ThrowArgumentNullExpectedException(thrownException);
            }

            var argumentNullException = targetInvocationException.InnerException as ArgumentNullException;
            if (argumentNullException == null)
            {
                ThrowArgumentNullExpectedException(targetInvocationException.InnerException);
            }

            return argumentNullException;
        }

        private static void ThrowArgumentNullExpectedException(Exception actualException)
        {
            var expectedValue = typeof(ArgumentNullException);
            var wasValue = (actualException == null) ? "Null" : actualException.GetType().ToString();
            throw new AssertionException(string.Format("Expected: {0} but was: {1}\n{2}", expectedValue, wasValue, actualException));
        }
    }
}