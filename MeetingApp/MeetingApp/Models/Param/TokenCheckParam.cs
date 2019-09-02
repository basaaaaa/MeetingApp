namespace MeetingApp.Models.Param
{
    public class TokenCheckParam
    {
        public bool IsSuccessed { get; set; }   //TokenCheckが正常に成功したかどうか

        public bool HasError { get; set; }  //TokenCheckにおいてエラーが発生したかどうか

        public bool NotFoundTokenText { get; set; }  //MyTokenがDB上にあるかどうか

        public bool IsOverTimeToken { get; set; }  //時間外のTokenかどうか

        public bool NoExistMyToken { get; set; }


        public TokenCheckParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.NotFoundTokenText = false;
            this.IsOverTimeToken = false;
            this.NoExistMyToken = false;
        }
    }
}
