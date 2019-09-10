namespace MeetingApp.Models.Param
{
    public class CreateMeetingLabelParam
    {

        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか

        public bool BlankLabelName { get; set; }

        public CreateMeetingLabelParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.BlankLabelName = false;
        }
    }
}
