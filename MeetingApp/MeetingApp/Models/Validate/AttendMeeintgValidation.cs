using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using System.Collections.Generic;

namespace MeetingApp.Models.Validate
{
    public class AttendMeetingValidation
    {
        /// <summary>
        /// ��c�ɎQ���{�^�������������ۂ̃o���f�[�V����
        /// </summary>
        /// <param name="meetingLabels">�Q���҂������x�����Ƃ̃f�[�^</param>
        /// <returns>��c�Q���Ɋւ���p�����[�^</returns>
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
