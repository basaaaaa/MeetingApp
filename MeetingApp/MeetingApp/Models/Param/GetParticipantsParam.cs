namespace MeetingApp.Models.Param
{
    public class GetParticipantsParam
    {
        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか


        public GetParticipantsParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
        }
    }
}
