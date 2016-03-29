using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class _Definitions
    {
        /*

         * http://martinfowler.com/articles/mocksArentStubs.html

         */

        #region TestTypes

        //Unit tests are for one class at a time

        //Integration tests are for the interaction of two classes or external systems

        //Functional tests are for the entire system

        #endregion

        #region Mocks

        //Why Mocks
        //When one class relies on another class
        //Use Interfaces whenever possible
        //Mocks are implementations generated from a framework 
        //Abstract persistance store

        #endregion

        private MockRepository _mockery;

        #region Setup/TearDown
        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
        }
        #endregion


        [Test]
        public void CreateMock()
        {
            //_mockery = new MockRepository();

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();

            le.ChangeDate = DateTime.Now;
        }

        [Test]
        public void Mock()
        {
            string email = "bob@ano.com";

            _mockery = new MockRepository();

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();

            SetupResult.For(le.EmailAddress).Return(email);

            _mockery.ReplayAll();

            Assert.AreSame(le.EmailAddress, email);
        }

        [Test]
        public void Test()
        {
            string f = "first";
            string s = "second";

            Do2Things dt = new Do2Things("real", null);

            string str = dt.Do2things(f, s);

            Assert.Greater(str.Length, "firstsecond".Length);
        }

        [Test]
        public void unitTest()
        {
            string f = "first";
            string s = "second";

            string t = "test string";

            IDo1Thing d1t = _mockery.StrictMock<IDo1Thing>();

            SetupResult.For(d1t.Do1thing(f)).Return(t);

            _mockery.ReplayAll();

            Do2Things dt = new Do2Things("mock", d1t);
            string str = dt.Do2things(f, s);

            Assert.IsTrue(str.Contains(t));
        }
    }

    #region Objects

    public class Do2Things
    {

        IDo1Thing _d1t;

        public Do2Things(string TestType, IDo1Thing d1t)
        {
            if (TestType == "real")
                _d1t = new Do1Thing();
            else
                _d1t = d1t;
        }

        public string Do2things(string one, string two)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(_d1t.Do1thing(one));

            sb.AppendLine(String.Format("{0}: two", two));

            return sb.ToString();
        }
    }

    public class Do1Thing : IDo1Thing
    {
        public Do1Thing() { }

        public string Do1thing(string one)
        {
            return String.Format("{0}: one", one);
        }

    }

    public interface IDo1Thing
    {
        string Do1thing(string one);
    }

    #endregion

}
