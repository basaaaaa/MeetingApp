using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace MeetingApp.Models.Validate
{
    public class CreateMeetingValidation
    {
        public CreateMeetingParam InputValidate(string title, string date, string startTime, string endTime, string location, ObservableCollection<MeetingLabelData> labels)
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

            //入力された会議実施日が空かどうかチェック
            if (string.IsNullOrEmpty(date))
            {
                //存在していた場合POSTを失敗で終了
                createMeetingParam.HasError = true;
                createMeetingParam.BlankMeetingDate = true;
            }

            //入力された会議開始時間が空かどうかチェック
            if (string.IsNullOrEmpty(startTime))
            {
                //存在していた場合POSTを失敗で終了
                createMeetingParam.HasError = true;
                createMeetingParam.BlankMeetingStartTime = true;
            }

            //入力された会議終了時間が空かどうかチェック
            if (string.IsNullOrEmpty(endTime))
            {
                //存在していた場合POSTを失敗で終了
                createMeetingParam.HasError = true;
                createMeetingParam.BlankMeetingEndTime = true;
            }

            //入力された日付文字列が指定形式に合っているかどうかチェック
            if (!string.IsNullOrEmpty(date) && !Regex.IsMatch(date, @"\d{2,4}-\d{1,2}-\d{1,2}"))
            {
                createMeetingParam.HasError = true;
                createMeetingParam.NotDateType = true;
            }

            //入力された開始時刻文字列が指定形式に合っているかどうかチェック
            if (!string.IsNullOrEmpty(startTime) && !Regex.IsMatch(startTime, @"\d{1,2}:\d{1,2}"))
            {
                createMeetingParam.HasError = true;
                createMeetingParam.NotTimeType = true;
            }

            //入力された終了時刻文字列が指定形式に合っているかどうかチェック
            if (!string.IsNullOrEmpty(endTime) && !Regex.IsMatch(endTime, @"\d{1,2}:\d{1,2}"))
            {
                createMeetingParam.HasError = true;
                createMeetingParam.NotTimeType = true;
            }

            //会議ラベルが1つ以上追加されているかどうかチェック
            if (labels.Count == 0)
            {
                createMeetingParam.HasError = true;
                createMeetingParam.NoExistLabel = true;
            }

            return createMeetingParam;
        }

    }
}
