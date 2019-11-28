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

            Assert.IsTrue(result.IsSuccessed);
            Assert.IsFalse(result.HasError);
        }

        /// <summary>
        /// 異常系
        /// </summary>
        [Test]
        public void CreateMeetingLabel_InputValidate_Failure()
        {
            var createMeetingLabelValidation = new CreateMeetingLabelValidation();

            var result = createMeetingLabelValidation.InputValidate("");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.BlankLabelName);
        }
    }
}
