using MeetingApp.Models.Validate;
using NUnit.Framework;

namespace MeetingApp.Test.Models.Validate
{
    public class SignUpValidationTest
    {
        [SetUp]
        public void Setup()
        {
        }
        /// <summary>
        /// 正常系
        /// </summary>
        [Test]
        public void SignUp_InputValidate_Success()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "password");

            Assert.AreEqual(result.IsSuccessed, true);
            Assert.AreEqual(result.HasError, false);

        }
        /// <summary>
        /// 異常系
        /// </summary>
        [Test]
        public void SignUp_InputValidate_Failure_BlankUserId()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("", "password");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.BlankUserId, true);

        }
        [Test]
        public void SignUp_InputValidate_Failure_BlankPassword()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.BlankPassword, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_ShortPassword()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "pass");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.ShortPassword, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedUserId()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("あかさたなはま", "password");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.UnSpecifiedUserId, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedUserId_AllUnderBar()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("_______", "password");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.UnSpecifiedUserId, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedUserId_AllNumber()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("1237832", "password");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.UnSpecifiedUserId, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedUserId_ContainNumberAndKana()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("1234あじゃさあ", "password");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.UnSpecifiedUserId, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedUserId_AllSymbol()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate(";;;;;", "password");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.UnSpecifiedUserId, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedPassword_AllKana()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "あかさたなはま");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.UnSpecifiedPassword, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedPassword_AllUnderBar()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "______");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.UnSpecifiedPassword, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedPassword_AllNumber()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "123456");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.UnSpecifiedPassword, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedPassword_ContainNumberAndKana()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "1234あじゃさあ");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.UnSpecifiedPassword, true);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedPassword_AllSymbol()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", ";;;;;;");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.UnSpecifiedPassword, true);

        }





    }
}
