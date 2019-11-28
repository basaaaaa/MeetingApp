using MeetingApp.Models.Data;
using MeetingApp.Models.Validate;
using NUnit.Framework;
using System.Collections.Generic;

namespace MeetingApp.Test.Models.Validate
{
    public class CreateMeetingLabelItemValidationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// 正常系
        /// </summary>
        [Test]
        public void CreateMeetingLabelItem_InputValidate_Success()
        {
            var createMeetingLabelItemValidation = new CreateMeetingLabelItemValidation();
            var result = createMeetingLabelItemValidation.InputValidate("test");

            Assert.IsTrue(result.IsSuccessed);
            Assert.IsFalse(result.HasError);
        }

        [Test]
        public void CreateMeetingLabelItem_AddValidate_Success()
        {
            var createMeetingLabelItemValidation = new CreateMeetingLabelItemValidation();

            var testMeetingLabelItemDatas = new List<MeetingLabelItemData>();
            var testMeetingLabelItemData = new MeetingLabelItemData(1, 2, "test");

            testMeetingLabelItemDatas.Add(testMeetingLabelItemData);

            var result = createMeetingLabelItemValidation.AddValidate(testMeetingLabelItemDatas);

            Assert.IsTrue(result.IsSuccessed);
            Assert.IsFalse(result.HasError);
        }

        /// <summary>
        /// 異常系
        /// </summary>
        /// 
        [Test]
        public void CreateMeetingLabelItem_InputValidate_Failure_BlankItemName()
        {
            var createMeetingLabelItemValidation = new CreateMeetingLabelItemValidation();
            var result = createMeetingLabelItemValidation.InputValidate("");

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.BlankItemName);
        }

        [Test]
        public void CreateMeetingLabelItem_AddValidate_Failure_NoExistItem()
        {
            var createMeetingLabelItemValidation = new CreateMeetingLabelItemValidation();
            var testMeetingLabelItemDatas = new List<MeetingLabelItemData>();

            var result = createMeetingLabelItemValidation.AddValidate(testMeetingLabelItemDatas);

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.NoExistItem);
        }
    }
}
