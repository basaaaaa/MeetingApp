using MeetingApp.Models.Data;

namespace MeetingApp.Models.Param
{
    class GetUserParam
    {
        public bool IsSuccessed { get; set; }   //ユーザー単一処理取得が正常に成功したかどうか

        public bool HasError { get; set; }  //ユーザー単一処理取得においてエラーが発生したかどうか

        public UserData User { get; set; } //取得したユーザーデータ

        public GetUserParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.User = null;
        }
    }
}
