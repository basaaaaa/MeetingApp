using MeetingApp.Models.Data;
using MeetingApp.Models.Validate;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;

namespace MeetingApp.Test.Models.Validate
{
    class CreateMeetingValidationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// 正常系
        /// </summary>
        [Test]
        public void CreateMeeting_InputValidate_Success()
        {
            var createMeetingValidation = new CreateMeetingValidation();
            var testMeetingLabels = new ObservableCollection<MeetingLabelData>();
            var testMeetingLabel = new MeetingLabelData(1, "test");
            testMeetingLabels.Add(testMeetingLabel);

            var testMeetingDate = DateTime.UtcNow;
            var testMeetingStartTime = new TimeSpan(10, 0, 0);
            var testMeetingEndTime = new TimeSpan(11, 0, 0);

            var result = createMeetingValidation.InputValidate("testMeeting", testMeetingDate, testMeetingStartTime, testMeetingEndTime, "MeetingSpace", testMeetingLabels);

            Assert.IsTrue(result.IsSuccessed);
            Assert.IsFalse(result.HasError);
        }

        /// <summary>
        /// 異常系
        /// </summary>
        [Test]
        public void CreateMeeting_InputValidate_Failure_BlankMeetingTitle()
        {
            var createMeetingValidation = new CreateMeetingValidation();
            var testMeetingLabels = new ObservableCollection<MeetingLabelData>();
            var testMeetingLabel = new MeetingLabelData(1, "test");
            testMeetingLabels.Add(testMeetingLabel);

            var testMeetingDate = DateTime.UtcNow;
            var testMeetingStartTime = new TimeSpan(10, 0, 0);
            var testMeetingEndTime = new TimeSpan(11, 0, 0);

            var result = createMeetingValidation.InputValidate("", testMeetingDate, testMeetingStartTime, testMeetingEndTime, "MeetingSpace", testMeetingLabels);

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.BlankMeetingTitle);
        }

        [Test]
        public void CreateMeeting_InputValidate_Failure_BlankMeetingLocation()
        {
            var createMeetingValidation = new CreateMeetingValidation();
            var testMeetingLabels = new ObservableCollection<MeetingLabelData>();
            var testMeetingLabel = new MeetingLabelData(1, "test");
            testMeetingLabels.Add(testMeetingLabel);

            var testMeetingDate = DateTime.UtcNow;
            var testMeetingStartTime = new TimeSpan(10, 0, 0);
            var testMeetingEndTime = new TimeSpan(11, 0, 0);

            var result = createMeetingValidation.InputValidate("testMeeting", testMeetingDate, testMeetingStartTime, testMeetingEndTime, "", testMeetingLabels);

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.BlankMeetingLocation);
        }

        [Test]
        public void CreateMeeting_InputValidate_Failure_TimeError()
        {
            var createMeetingValidation = new CreateMeetingValidation();
            var testMeetingLabels = new ObservableCollection<MeetingLabelData>();
            var testMeetingLabel = new MeetingLabelData(1, "test");
            testMeetingLabels.Add(testMeetingLabel);

            var testMeetingDate = DateTime.UtcNow;
            var testMeetingStartTime = new TimeSpan(11, 0, 0);
            var testMeetingEndTime = new TimeSpan(10, 0, 0);

            var result = createMeetingValidation.InputValidate("testMeeting", testMeetingDate, testMeetingStartTime, testMeetingEndTime, "", testMeetingLabels);

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.TimeError);
        }

        [Test]
        public void CreateMeeting_InputValidate_Failure_NoExistLabel()
        {
            var createMeetingValidation = new CreateMeetingValidation();
            var testMeetingLabels = new ObservableCollection<MeetingLabelData>();

            var testMeetingDate = DateTime.UtcNow;
            var testMeetingStartTime = new TimeSpan(10, 0, 0);
            var testMeetingEndTime = new TimeSpan(11, 0, 0);

            var result = createMeetingValidation.InputValidate("testMeeting", testMeetingDate, testMeetingStartTime, testMeetingEndTime, "", testMeetingLabels);

            Assert.IsFalse(result.IsSuccessed);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.NoExistLabel);
        }




    }
}
