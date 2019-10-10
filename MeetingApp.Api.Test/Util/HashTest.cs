using MeetingApp.Api.Util;
using NUnit.Framework;

namespace MeetingApp.Api.Test.Util
{
    public class HashTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Encrypt_TextIsChanged()
        {
            var text = "aiueo";
            Hash hash = new Hash();
            var returnText = hash.Encrypt(text);
            Assert.AreNotEqual(text, returnText);
        }
    }
}
