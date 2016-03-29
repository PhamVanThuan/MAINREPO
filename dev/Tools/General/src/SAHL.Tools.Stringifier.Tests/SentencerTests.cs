using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SAHL.Tools.Stringifier.Tests
{
    [TestFixture]
    public class SentencerTests
    {
        [Test]
        public void InsertWhiteSpaceBetweenWords_GivenEnmptyString_ShouldReturnEmptyString()
        {
            //Assign
            const string toTest = "";

            //Action
            string output = Sentencer.InsertWhiteSpaceBetweenWords(toTest);

            //Assert
            Assert.AreEqual("", output);
        }

        [Test]
        public void InsertWhiteSpaceBetweenWords_GivenPascalCaseString_ShouldReturnStringWithSpaces()
        {
            //Assign
            const string toTest = "MyStringValue";

            //Action
            string output = Sentencer.InsertWhiteSpaceBetweenWords(toTest);

            //Assert
            Assert.AreEqual("My String Value", output);
        }

        [Test]
        public void InsertWhiteSpaceBetweenWords_GivenSingleWord_ShouldSameWord()
        {
            //Assign
            const string toTest = "Value";

            //Action
            string output = Sentencer.InsertWhiteSpaceBetweenWords(toTest);

            //Assert
            Assert.AreEqual("Value", output);
        }

        [Test]
        public void InsertWhiteSpaceBetweenWords_GivenStringWhichContainsA_ShouldReturnAWithWhiteSpace()
        {
            //Assign
            const string toTest = "AValue";

            //Action
            string output = Sentencer.InsertWhiteSpaceBetweenWords(toTest);

            //Assert
            Assert.AreEqual("A Value", output);
        }

        [TestCase("Value28thNumber", "Value 28th Number")]
        [TestCase("Value28th", "Value 28th")]
        [TestCase("Value2nd", "Value 2nd")]
        public void InsertWhiteSpaceBetweenWords_GivenStringWithNumericDescription_ShouldInsertWhiteSpaceAfterNumberDescription(string toTest, string result)
        {
            //Assign

            //Action
            string output = Sentencer.InsertWhiteSpaceBetweenWords(toTest);

            //Assert
            Assert.AreEqual(result, output);
        }

        [TestCase("ABCType", "ABC Type")]
        [TestCase("ABC", "ABC")]
        [TestCase("AType", "A Type")]
        [TestCase("ABCTypeResult", "ABC TypeResult")]
        public void InsertWhiteSpaceBetweenWords_GivenStringWithMoreThanOnePreceedingUpperCase_ShouldReturnStringWithWithSpaceAfterSecondLastUpperCase(string toTest, string result)
        {
            //Assign

            //Action
            string output = Sentencer.HandleUpperCasesCase(toTest);

            //Assert
            Assert.AreEqual(result, output);
        }

        [Test]
        public void ToSentenceFromTypeName_GivenStringWithPreceedingIForInterface_ShouldRemoveTheI()
        {
            //Assign
            const string toTest = "IRequires";
            const string result = "Requires";

            //Action
            string output = Sentencer.ToSentenceFromTypeName(toTest);

            //Assert
            Assert.AreEqual(result, output);
        }

        [Test]
        public void ToSentenceFromTypeName_GivenStringWithNoPreceedingIForInterface_ShouldReturnTheSameAsTheInput()
        {
            //Assign
            const string toTest = "Requires";
            const string result = "Requires";

            //Action
            string output = Sentencer.ToSentenceFromTypeName(toTest);

            //Assert
            Assert.AreEqual(result, output);
        }

        [Test]
        public void ToSentenceFromTypeName_GivenStringWhichContainsPascalCaseAndPreceedingI_ShouldReturnStringWithWhiteSpaceAndNoI()
        {
            //Assign
            const string input = "IRequiresOpenApplication";
            const string result = "Requires Open Application";

            //Action
            string output = Sentencer.ToSentenceFromTypeName(input);

            //Assert
            Assert.AreEqual(result, output);
        }

        [Test]
        public void ToSentenceFromTypeName_GivenTypeNameWithNumbers_ShouldReturnStringWithSpacingAroundNumbers()
        {
            const string input = "ApplicantsMustBeBetween18And65YearsOldRule";
            const string expected = "Applicants Must Be Between 18 And 65 Years Old Rule";

            var actual = Sentencer.ToSentenceFromTypeName(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToSentenceFromTypeName_GivenTypeNameWithPositionalNumbers_ShouldReturnStringWithNoSpaceingAroundPositionals()
        {
            const string input = "ApplicantsMustHaveArrived1stOr2ndAndBeOver18YearsOld";
            const string expected = "Applicants Must Have Arrived 1st Or 2nd And Be Over 18 Years Old";

            var actual = Sentencer.ToSentenceFromTypeName(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToSentenceFromTypename_GivenStringWithUnderscores_ShouldReturnStringWithNoUnderscores()
        {
            const string input = "DomesticWorkerWage_GardenServices";
            const string expected = "Domestic Worker Wage Garden Services";

            var actual = Sentencer.ToSentenceFromTypeName(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToSentenceFromTypeName_GivenLowercaseStringWithUnderscores_ShouldReturnStringWithNoUnderscores()
        {
            const string input = "domesticworkerwage_gardenservices";
            const string expected = "domesticworkerwagegardenservices";

            var actual = Sentencer.ToSentenceFromTypeName(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToSentenceFromTypeName_GivenStringWithGenericTypeDefinition_ShouldReturnStringWithoutTypeDefinition()
        {
            const string input = "ClientAddressMustBeAStreetAddressRule`1";
            const string expected = "Client Address Must Be A Street Address Rule";

            var actual = Sentencer.ToSentenceFromTypeName(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToSentenceFromTypeName_GivenStringStartingWithI_ShouldNotRemoveTheIIfTheStringIsID()
        {
            const string input = "IDNumber";
            const string expected = "ID Number";

            var actual = Sentencer.ToSentenceFromTypeName(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToSentenceFromTypeName_GivenStringStartingWithIFollowedByAD_ShouldRemoveTheIIfTheStringIsNotID()
        {
            const string input = "IDisinterested";
            const string expected = "Disinterested";

            var actual = Sentencer.ToSentenceFromTypeName(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToSentenceFromTypeName_GivenStringStartingAsID_ShouldReturnStringAsIs()
        {
            const string input = "ID";
            const string expected = "ID";

            var actual = Sentencer.ToSentenceFromTypeName(input);

            Assert.AreEqual(expected, actual);
        }
    }
}
