namespace MeetingApp.Models.Param
{
    public class SignUpParam
    {

        public bool IsSuccessed { get; set; }   //新規登録が正常に成功したかどうか

        public bool HasError { get; set; }  //新規登録においてエラーが発生したかどうか


        public bool UserExists { get; set; }    //エラーにおいて、userId被りによるものかどうか

        public bool BlankUserId { get; set; }    //エラーにおいて、userIdが未入力   

        public bool BlankPassword { get; set; }    //エラーにおいて、passwordが未入力   

        public bool ShortPassword { get; set; }    //エラーにおいて、passwordが短い   

        public bool UnSpecifiedUserId { get; set; }    //エラーにおいて、userIdに指定外文字列使用されている   

        public bool UnSpecifiedPassword { get; set; }    //エラーにおいて、Passwordに指定外文字列使用されている  

        public SignUpParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.UserExists = false;
            this.BlankUserId = false;
            this.BlankPassword = false;
            this.ShortPassword = false;
            this.UnSpecifiedUserId = false;
            this.UnSpecifiedPassword = false;
        }

    }
}
