using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Models.Validate;
using MeetingApp.Utils;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace MeetingApp.Test.Models.Validate
{
    public class TokenCheckValidationTest
    {
        private Mock<RestService> _mockRestService;
        private Mock<OperateDateTime> _mockOperateDateTime;
        private TokenCheckValidation _target;
        [SetUp]
        public void Setup()
        {
            _mockRestService = new Mock<RestService>();
            _mockOperateDateTime = new Mock<OperateDateTime>();
            _target = new TokenCheckValidation(_mockRestService.Object);

        }
        /// <summary>
        /// ê≥èÌån
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TokenCheck_Validate_Success()
        {
            var testTokenText = "test";
            var testTokenStartTime = DateTime.Now;
            var testTokenEndTime = DateTime.Now.AddHours(1);
            var testToken = new TokenData(testTokenText, testTokenStartTime, testTokenEndTime);

            var responseTokenCheckParam = new TokenCheckParam();
            responseTokenCheckParam.IsSuccessed = true;

            _mockRestService.Setup(r => r.CheckTokenDataAsync(It.IsAny<string>(), It.IsAny<TokenData>())).ReturnsAsync(responseTokenCheckParam);

            var actual = await _target.Validate(testToken);

            Assert.IsNotNull(actual);
            Assert.AreEqual(responseTokenCheckParam.IsSuccessed, actual.IsSuccessed);
            Assert.AreEqual(responseTokenCheckParam.HasError, actual.HasError);
            _mockRestService.VerifyAll();
        }
        /// <summary>
        /// àŸèÌån
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TokenCheck_Validate_Failure_NoExistMyToken()
        {
            var testTokenText = null as string;
            var testTokenStartTime = DateTime.Now;
            var testTokenEndTime = DateTime.Now.AddHours(1);
            var testToken = new TokenData(testTokenText, testTokenStartTime, testTokenEndTime);

            var responseTokenCheckParam = new TokenCheckParam();
            responseTokenCheckParam.HasError = true;
            responseTokenCheckParam.NoExistMyToken = true;

            _mockRestService.Setup(r => r.CheckTokenDataAsync(It.IsAny<string>(), It.IsAny<TokenData>())).ReturnsAsync(responseTokenCheckParam);

            var actual = await _target.Validate(testToken);

            Assert.IsNotNull(actual);
            Assert.AreEqual(responseTokenCheckParam.IsSuccessed, actual.IsSuccessed);
            Assert.AreEqual(responseTokenCheckParam.HasError, actual.HasError);
            _mockRestService.VerifyAll();
        }
    }
}
