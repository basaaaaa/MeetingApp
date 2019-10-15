using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using System.Collections.Generic;

namespace MeetingApp.Models.Validate
{
    public class AttendMeetingValidation
    {
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
