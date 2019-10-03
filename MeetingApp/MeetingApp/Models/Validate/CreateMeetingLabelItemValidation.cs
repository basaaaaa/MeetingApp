using MeetingApp.Models.Param;

namespace MeetingApp.Models.Validate
{
    public class CreateMeetingLabelItemValidation
    {
        public CreateMeetingLabelItemParam InputValidate(string itemName)
        {
            CreateMeetingLabelItemParam createMeetingLabelItemParam = new CreateMeetingLabelItemParam();

            //入力されたラベル名が空かどうかチェック
            if (string.IsNullOrEmpty(itemName))
            {
                //存在していた場合POSTを失敗で終了
                createMeetingLabelItemParam.HasError = true;
                createMeetingLabelItemParam.BlankItemName = true;
            }

            return createMeetingLabelItemParam;
        }

    }
}
