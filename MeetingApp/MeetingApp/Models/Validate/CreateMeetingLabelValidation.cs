using MeetingApp.Models.Param;

namespace MeetingApp.Models.Validate
{
    public class CreateMeetingLabelValidation
    {
        public CreateMeetingLabelParam InputValidate(string labelName)
        {
            var createMeetingLabelParam = new CreateMeetingLabelParam();

            //ラベル名が空だった場合
            if (string.IsNullOrEmpty(labelName))
            {
                createMeetingLabelParam.HasError = true;
                createMeetingLabelParam.BlankLabelName = true;
                return createMeetingLabelParam;
            }
            //ラベル名が長すぎた場合
            if (labelName.Length > 12)
            {
                createMeetingLabelParam.HasError = true;
                createMeetingLabelParam.MeetingLabelNameCharactersOver = true;
                return createMeetingLabelParam;

            }

            createMeetingLabelParam.IsSuccessed = true;

            return createMeetingLabelParam;
        }
    }
}
