namespace MeetingApp.Models.Param
{
    public class UpdateMeetingParam
    {

        public bool IsSuccessed { get; set; }   //TokenCheckが正常に成功したかどうか

        public bool HasError { get; set; }  //TokenCheckにおいてエラーが発生したかどうか



        public UpdateMeetingParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
        }
    }
}
