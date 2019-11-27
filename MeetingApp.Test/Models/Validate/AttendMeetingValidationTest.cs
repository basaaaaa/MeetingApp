using MeetingApp.Models.Data;
using MeetingApp.Models.Validate;
using NUnit.Framework;
using System.Collections.Generic;

namespace MeetingApp.Test.Models.Validate
{
    public class AttendMeetingValidationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// ê≥èÌån
        /// </summary>
        [Test]
        public void AttendMeeting_ButtonPushedValidate_Success()
        {
            var testMeetingLabels = new List<MeetingLabelData>();

            var testMeetingLabel = new MeetingLabelData(1, "testLabel");
            testMeetingLabel.MeetingLabelItemDatas.Add(new MeetingLabelItemData(1, 2, "testMeetignLabelItem"));
            testMeetingLabel.MeetingLabelItemDatas.Add(new MeetingLabelItemData(2, 3, "testMeetignLabelItem2"));

            var testMeetingLabel2 = new MeetingLabelData(2, "testLabel2");
            testMeetingLabel2.MeetingLabelItemDatas.Add(new MeetingLabelItemData(3, 4, "testMeetignLabelItem"));
            testMeetingLabel2.MeetingLabelItemDatas.Add(new MeetingLabelItemData(4, 5, "testMeetignLabelItem2"));

            var attendMeetingValidation = new AttendMeetingValidation();
            var result = attendMeetingValidation.ButtonPushedValidate(testMeetingLabels);
            Assert.AreEqual(result.IsSuccessed, true);
            Assert.AreEqual(result.HasError, false);


        }
        /// <summary>
        /// àŸèÌån
        /// </summary>
        [Test]
        public void AttendMeeting_ButtonPushedValidate_Failure_NoExistLabelItems()
        {
            var testMeetingLabels = new List<MeetingLabelData>();

            var testMeetingLabel = new MeetingLabelData(1, "testLabel");
            testMeetingLabel.MeetingLabelItemDatas.Add(new MeetingLabelItemData(1, 2, "testMeetignLabelItem"));
            testMeetingLabel.MeetingLabelItemDatas.Add(new MeetingLabelItemData(2, 3, "testMeetignLabelItem2"));
            testMeetingLabels.Add(testMeetingLabel);

            var testMeetingLabel2 = new MeetingLabelData(2, "testLabel2");
            testMeetingLabels.Add(testMeetingLabel2);

            var attendMeetingValidation = new AttendMeetingValidation();
            var result = attendMeetingValidation.ButtonPushedValidate(testMeetingLabels);

            Assert.AreEqual(result.IsSuccessed, false);
            Assert.AreEqual(result.HasError, true);
            Assert.AreEqual(result.NoExistLabelItems, true);



        }
    }
}
