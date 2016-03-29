using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SAHL.Core.Tests.DataTypes
{
    [TestFixture]
    public class FormatsTests
    {
        private readonly Dictionary<string,object[]> typicalValueByFormatType = new Dictionary<string, object[]>
        {
            { "Date", new object[]{ DateTime.Now, DateTime.Today } },
            { "DateTime", new object[] { DateTime.Now, DateTime.Today } },
            { "Currency", new object[]{ 12345.67890m, 12345m, 12345.67890d, 12345.67890f, 12345, 12345U, 12345L, 12345UL } },
            { "Number", new object[]{ 12345.67890m, 12345m, 12345.67890d, 12345.67890f, 12345, 12345U, 12345L, 12345UL } },
        };
        
        [Test]
        public void Formats_WhenGivenDefaultValues_EnsureNoExceptionIsThrown()
        {
            var assembly = Assembly.Load("SAHL.Core");

            var types = assembly.GetTypes()
                .Where(a => a.IsClass && a.IsSealed && a.IsAbstract //closest we can get to IsStatic
                    && a.Namespace != null
                    && a.Namespace.StartsWith("SAHL.Core.DataType")
                    && !a.FullName.Equals("SAHL.Core.DataType.Formats") //only classes within class Formats, not the Formats class itself
                );

            foreach (var type in types)
            {
                var formatType = type.Name;

                var constants = type
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(a => a.IsLiteral && a.FieldType == typeof(string));

                foreach (var item in constants)
                {
                    var propertyName = item.Name;
                    var value = (string) item.GetRawConstantValue();

                    ValidateFormatForTypicalValueOfType(propertyName, value, formatType);
                }
            }
        }

        private void ValidateFormatForTypicalValueOfType(string propertyName, string format, string formatType)
        {
            var values = this.typicalValueByFormatType[formatType];
            object value = null;
            try
            {
                foreach (var item in values)
                {
                    value = item;
                    string.Format(format, value);
                }
            }
            catch (Exception ex)
            {
                var message = "The format string {0} does not contain a value that correctly formats a typical value of {1}. " +
                    "Format string property name: {2}. Attempted to format value: {3}. Exception: {4}";
                Assert.Fail(message
                    , format, value.GetType().FullName, formatType + "." + propertyName, value, ex);
            }
        }
    }
}
