namespace MeetingApp.Models.Param
{
    class CreateMeetingParam
    {
        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか

        public bool BlankMeetingTitle { get; set; }
        public bool BlankMeetingDate { get; set; }
        public bool BlankMeetingStartTime { get; set; }
        public bool BlankMeetingEndTime { get; set; }

        public CreateMeetingParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.BlankMeetingTitle = false;
            this.BlankMeetingDate = false;
            this.BlankMeetingStartTime = false;
            this.BlankMeetingEndTime = false;
        }
    }
}
