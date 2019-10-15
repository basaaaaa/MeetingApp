namespace MeetingApp.Models.Param
{
    public class AttendMeetingParam
    {
        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか

        public bool NoExistLabelItems { get; set; }

        public AttendMeetingParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.NoExistLabelItems = false;
        }
    }
}
