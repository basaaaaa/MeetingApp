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
            var encrypter = new Hash();
            var result = encrypter.Encrypt(text);
            Assert.AreNotEqual(text, result);
        }
        public void Encrypt_Null()
        {
            var encrypter = new Hash();
            var text = null as string;
            var result = encrypter.Encrypt(text);
            Assert.IsNull(result);
        }
    }
}
