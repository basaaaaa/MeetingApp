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

        [Test]
        public void CheckString_isAlphaNumericPlusAlphaOnly_Success()
        {

            var checkString = new CheckString();
            var result = checkString.isAlphaNumericPlusAlphaOnly("test0000");

            Assert.IsTrue(result);
        }
    }
}
