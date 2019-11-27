using MeetingApp.Models.Validate;
using NUnit.Framework;

namespace MeetingApp.Test.Models.Validate
{
    public class CreateMeetingLabelValidationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// 正常系
        /// </summary>
        [Test]
        public void CreateMeetingLabel_InputValidate_Success()
        {
            var createMeetingLabelValidation = new CreateMeetingLabelValidation();

            var result = createMeetingLabelValidation.InputValidate("test");

            Assert.AreEqual(result.IsSuccessed, true);
            Assert.AreEqual(result.HasError, false);
        }

        /// <summary>
        /// 異常系
        /// </summary>
        [Test]
        public void CreateMeetingLabel_InputValidate_Failure()
        {
            var createMeetingLabelValidation = new CreateMeetingLabelValidation();

            var result = createMeetingLabelValidation.InputValidate("");

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.BlankLabelName, true);
        }
    }
}
