using NUnit.Framework;

using SAHL.Core.Testing;
using System;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestHaloRoleAttribute
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var roleAttribute = new HaloRoleAttribute("Test", "Capabilities");
            //---------------Test Result -----------------------
            Assert.IsNotNull(roleAttribute);
        }

        [TestCase("roleName")]
        public void Construct_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.Throws<ArgumentNullException>(()=>new HaloRoleAttribute(null));
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            const string roleName     = "Test";
            const string capabilities = "Cabailities";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var roleAttribute = new HaloRoleAttribute(roleName, capabilities);
            //---------------Test Result -----------------------
            Assert.AreEqual(roleName, roleAttribute.RoleName);
            Assert.AreEqual(new string[]{capabilities}, roleAttribute.Capabilities);
        }
    }
}
