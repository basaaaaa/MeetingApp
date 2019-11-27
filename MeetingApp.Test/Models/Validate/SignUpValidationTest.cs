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

            Assert.IsTrue(result.IsSuccessed);
            Assert.IsFalse(result.HasError);

        }
        /// <summary>
        /// 異常系
        /// </summary>
        [Test]
        public void SignUp_InputValidate_Failure_BlankUserId()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("", "password");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.BlankUserId);

        }
        [Test]
        public void SignUp_InputValidate_Failure_BlankPassword()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.BlankPassword);

        }

        [Test]
        public void SignUp_InputValidate_Failure_ShortPassword()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "pass");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.ShortPassword);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedUserId()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("あかさたなはま", "password");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.UnSpecifiedUserId);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedUserId_AllUnderBar()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("_______", "password");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.UnSpecifiedUserId);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedUserId_AllNumber()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("1237832", "password");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.UnSpecifiedUserId);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedUserId_ContainNumberAndKana()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("1234あじゃさあ", "password");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.UnSpecifiedUserId);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedUserId_AllSymbol()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate(";;;;;", "password");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.UnSpecifiedUserId);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedPassword_AllKana()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "あかさたなはま");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.UnSpecifiedPassword);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedPassword_AllUnderBar()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "______");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.UnSpecifiedPassword);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedPassword_AllNumber()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "123456");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.UnSpecifiedPassword);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedPassword_ContainNumberAndKana()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", "1234あじゃさあ");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.UnSpecifiedPassword);

        }

        [Test]
        public void SignUp_InputValidate_Failure_UnSpecifiedPassword_AllSymbol()
        {
            var signUpValidation = new SignUpValidation();
            var result = signUpValidation.InputValidate("test", ";;;;;;");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.UnSpecifiedPassword);

        }





    }
}
