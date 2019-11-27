using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using System.Collections.Generic;

namespace MeetingApp.Models.Validate
{
    public class AttendMeetingValidation
    {
        /// <summary>
        /// 会議に参加ボタンを押下した際のバリデーション
        /// </summary>
        /// <param name="meetingLabels">参加者が持つラベルごとのデータ</param>
        /// <returns>会議参加に関するパラメータ</returns>
        public AttendMeetingParam ButtonPushedValidate(List<MeetingLabelData> meetingLabels)
        {
            AttendMeetingParam attendMeetingParam = new AttendMeetingParam();

            foreach (MeetingLabelData l in meetingLabels)
            {
                if (l.MeetingLabelItemDatasCount == 0)
                {
                    attendMeetingParam.HasError = true;
                    attendMeetingParam.NoExistLabelItems = true;
                    break;
                }

            }
            if (attendMeetingParam.HasError == false) { attendMeetingParam.IsSuccessed = true; }


            return attendMeetingParam;
        }
    }
}
