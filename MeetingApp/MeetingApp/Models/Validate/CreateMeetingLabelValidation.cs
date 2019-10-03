using MeetingApp.Models.Param;

namespace MeetingApp.Models.Validate
{
    public class CreateMeetingLabelValidation
    {
        public CreateMeetingLabelParam InputValidate(string labelName)
        {
            CreateMeetingLabelParam createMeetingLabelParam = new CreateMeetingLabelParam();

            //入力されたラベル名が空かどうかチェック
            if (string.IsNullOrEmpty(labelName))
            {
                //存在していた場合POSTを失敗で終了
                createMeetingLabelParam.HasError = true;
                createMeetingLabelParam.BlankLabelName = true;
            }

            return createMeetingLabelParam;
        }
    }
}
