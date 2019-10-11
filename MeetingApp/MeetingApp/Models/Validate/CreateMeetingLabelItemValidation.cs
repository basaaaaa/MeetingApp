using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using System.Collections.Generic;

namespace MeetingApp.Models.Validate
{
    public class CreateMeetingLabelItemValidation
    {
        public CreateMeetingLabelItemParam InputValidate(string itemName)
        {
            CreateMeetingLabelItemParam createMeetingLabelItemParam = new CreateMeetingLabelItemParam();

            //入力された項目名が空かどうかチェック
            if (string.IsNullOrEmpty(itemName))
            {
                //存在していた場合作成を失敗で終了
                createMeetingLabelItemParam.HasError = true;
                createMeetingLabelItemParam.BlankItemName = true;
            }

            return createMeetingLabelItemParam;
        }

        public CreateMeetingLabelItemParam AddValidate(List<MeetingLabelItemData> meetingLabelItemDatas)
        {
            CreateMeetingLabelItemParam createMeetingLabelItemParam = new CreateMeetingLabelItemParam();

            //リストが空かどうかチェック
            if (meetingLabelItemDatas.Count == 0)
            {
                //存在していた場合作成を失敗で終了
                createMeetingLabelItemParam.HasError = true;
                createMeetingLabelItemParam.NoExistItem = true;
            }

            return createMeetingLabelItemParam;
        }

    }
}
