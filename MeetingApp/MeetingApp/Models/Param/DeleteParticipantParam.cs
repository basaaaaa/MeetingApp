namespace MeetingApp.Models.Param
{
    public class DeleteParticipantParam
    {
        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか


        public DeleteParticipantParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
        }
    }
}
