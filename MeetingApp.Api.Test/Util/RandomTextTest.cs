using MeetingApp.Api.Util;
using NUnit.Framework;

namespace MeetingApp.Api.Test.Util
{
    public class RandomTextTest
    {
        [SetUp]
        public void Setup()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void RandomText_Success()
        {
            var encrypter = new RandomText();
            var result = encrypter.CreateRandomText();
            var result2 = encrypter.CreateRandomText();
            Assert.AreNotEqual(result, result2);
            Assert.AreNotEqual(result, result2);
        }
    }
}
