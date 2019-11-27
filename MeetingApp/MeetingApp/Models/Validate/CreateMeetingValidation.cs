using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using System;
using System.Collections.ObjectModel;

namespace MeetingApp.Models.Validate
{
    public class CreateMeetingValidation
    {
        public CreateMeetingParam InputValidate(string title, DateTime date, TimeSpan startTime, TimeSpan endTime, string location, ObservableCollection<MeetingLabelData> labels)
        {
            CreateMeetingParam createMeetingParam = new CreateMeetingParam();

            //入力された会議タイトルが空かどうかチェック
            if (string.IsNullOrEmpty(title))
            {
                //存在していた場合POSTを失敗で終了
                createMeetingParam.HasError = true;
                createMeetingParam.BlankMeetingTitle = true;
            }
            //入力された実施場所が空かどうかチェック
            if (string.IsNullOrEmpty(location))
            {
                createMeetingParam.HasError = true;
                createMeetingParam.BlankMeetingLocation = true;
            }

            var test = startTime - endTime;
            //開始時刻より終了時刻のほうが早く設定されている場合
            if (test > new TimeSpan(0))
            {
                createMeetingParam.HasError = true;
                createMeetingParam.TimeError = true;
            }

            //会議ラベルが1つ以上追加されているかどうかチェック
            if (labels.Count == 0)
            {
                createMeetingParam.HasError = true;
                createMeetingParam.NoExistLabel = true;
            }

            if (createMeetingParam.HasError == false)
            {
                createMeetingParam.IsSuccessed = true;
            }

            return createMeetingParam;
        }

    }
}
