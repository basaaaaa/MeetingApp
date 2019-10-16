namespace MeetingApp.Models.Param
{
    public class CreateParticipateParam
    {
        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか


        public CreateParticipateParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
        }
    }
}
