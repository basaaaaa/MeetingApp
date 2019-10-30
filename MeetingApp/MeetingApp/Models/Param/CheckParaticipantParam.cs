using MeetingApp.Models.Data;

namespace MeetingApp.Models.Param
{
    public class CheckParticipantParam
    {
        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか
        public bool NoExistUser { get; set; }

        public ParticipantData Participant { get; set; }


        public CheckParticipantParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.NoExistUser = false;
            this.Participant = new ParticipantData();
        }
    }
}
