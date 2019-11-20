namespace MeetingApp.Models.Param
{
    public class CreateMeetingParam
    {
        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか
        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか
        public bool BlankMeetingTitle { get; set; }
        public bool BlankMeetingDate { get; set; }
        public bool BlankMeetingStartTime { get; set; }
        public bool BlankMeetingEndTime { get; set; }
        public bool BlankMeetingLocation { get; set; }
        public bool NoExistLabel { get; set; }

        public bool NotDateType { get; set; }
        public bool NotTimeType { get; set; }
        public bool ApiCallError { get; set; }

        public bool TimeError { get; set; }


        public CreateMeetingParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.BlankMeetingTitle = false;
            this.BlankMeetingDate = false;
            this.BlankMeetingStartTime = false;
            this.BlankMeetingEndTime = false;
            this.BlankMeetingLocation = false;
            this.NoExistLabel = false;
            this.NotDateType = false;
            this.NotTimeType = false;
            this.ApiCallError = false;
            this.TimeError = false;
        }
    }
}
