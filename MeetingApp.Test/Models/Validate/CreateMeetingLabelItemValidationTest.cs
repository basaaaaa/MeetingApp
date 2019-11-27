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

            Assert.AreEqual(result.IsSuccessed, true);
            Assert.AreEqual(result.HasError, false);
        }

        [Test]
        public void CreateMeetingLabelItem_AddValidate_Success()
        {
            var createMeetingLabelItemValidation = new CreateMeetingLabelItemValidation();

            var testMeetingLabelItemDatas = new List<MeetingLabelItemData>();
            var testMeetingLabelItemData = new MeetingLabelItemData(1, 2, "test");

            testMeetingLabelItemDatas.Add(testMeetingLabelItemData);

            var result = createMeetingLabelItemValidation.AddValidate(testMeetingLabelItemDatas);

            Assert.AreEqual(result.IsSuccessed, true);
            Assert.AreEqual(result.HasError, false);
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

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.BlankItemName, true);
        }

        [Test]
        public void CreateMeetingLabelItem_AddValidate_Failure_NoExistItem()
        {
            var createMeetingLabelItemValidation = new CreateMeetingLabelItemValidation();
            var testMeetingLabelItemDatas = new List<MeetingLabelItemData>();

            var result = createMeetingLabelItemValidation.AddValidate(testMeetingLabelItemDatas);

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.NoExistItem, true);
        }
    }
}
