using MeetingApp.Models.Data;

namespace MeetingApp.Models.Param
{
    public class LoginParam
    {
        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか

        public bool BlankUserId { get; set; }    //エラーにおいて、userIdが未入力   

        public bool BlankPassword { get; set; }    //エラーにおいて、passwordが未入力

        public bool NotFoundUser { get; set; }

        public TokenData TokenData { get; set; }


        public LoginParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.BlankUserId = false;
            this.BlankPassword = false;
            this.NotFoundUser = false;
            this.TokenData = null;
        }
    }
}
