using MeetingApp.Utils;
using NUnit.Framework;

namespace MeetingApp.Test.Models.Utils
{
    public class CheckStringTest
    {
        [SetUp]
        public void Setup()
        {
        }
        /// <summary>
        /// 正常系
        /// </summary>
        [Test]
        public void CheckString_isAlphaNumericPlusAlphaOnly_Success_AlphabetAndNumber()
        {

            var checkString = new CheckString();
            var result = checkString.isAlphaNumericPlusAlphaOnly("test0000");

            Assert.IsTrue(result);
        }
        [Test]
        public void CheckString_isAlphaNumericPlusAlphaOnly_Success_AlphabetOnly()
        {

            var checkString = new CheckString();
            var result = checkString.isAlphaNumericPlusAlphaOnly("test");

            Assert.IsTrue(result);
        }
        [Test]
        public void CheckString_isAlphaNumericPlusAlphaOnly_Success_NumberOnly()
        {

            var checkString = new CheckString();
            var result = checkString.isAlphaNumericPlusAlphaOnly("123456789");

            Assert.IsTrue(result);
        }
        [Test]
        public void CheckString_isAlphaNumericPlusAlphaOnly_Success_ContainsUnderBar()
        {

            var checkString = new CheckString();
            var result = checkString.isAlphaNumericPlusAlphaOnly("12345_6789");

            Assert.IsTrue(result);
        }
        /// <summary>
        /// 失敗系
        /// </summary>
        [Test]
        public void CheckString_isAlphaNumericPlusAlphaOnly_Failure_UnderBarOnly()
        {

            var checkString = new CheckString();
            var result = checkString.isAlphaNumericPlusAlphaOnly("______");

            Assert.IsFalse(result);
        }
        [Test]
        public void CheckString_isAlphaNumericPlusAlphaOnly_Failure_ContainsKana()
        {

            var checkString = new CheckString();
            var result = checkString.isAlphaNumericPlusAlphaOnly("あかさたなABCD");

            Assert.IsFalse(result);
        }
        [Test]
        public void CheckString_isAlphaNumericPlusAlphaOnly_Failure_ContainsFullWidth()
        {

            var checkString = new CheckString();
            var result = checkString.isAlphaNumericPlusAlphaOnly("１２８７９２３");

            Assert.IsFalse(result);
        }
        [Test]
        public void CheckString_isAlphaNumericPlusAlphaOnly_Failure_SpaceOnly()
        {

            var checkString = new CheckString();
            var result = checkString.isAlphaNumericPlusAlphaOnly("      ");

            Assert.IsFalse(result);
        }

    }
}
