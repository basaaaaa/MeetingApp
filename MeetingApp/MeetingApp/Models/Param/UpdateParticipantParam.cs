namespace MeetingApp.Models.Param
{
    public class UpdateParticipantParam
    {
        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか


        public UpdateParticipantParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
        }
    }
}
