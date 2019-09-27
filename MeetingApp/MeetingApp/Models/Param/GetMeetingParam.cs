using MeetingApp.Data;

namespace MeetingApp.Models.Param
{
    public class GetMeetingParam
    {

        public bool IsSuccessed { get; set; }   //TokenCheckが正常に成功したかどうか

        public bool HasError { get; set; }  //TokenCheckにおいてエラーが発生したかどうか
        public MeetingData MeetingData { get; set; }    //GETするMeetingData



        public GetMeetingParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.MeetingData = new MeetingData();
        }
    }
}
