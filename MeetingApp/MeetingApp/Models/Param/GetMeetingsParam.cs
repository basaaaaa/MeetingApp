using MeetingApp.Data;
using System.Collections.Generic;

namespace MeetingApp.Models.Param
{
    public class GetMeetingsParam
    {
        public bool IsSuccessed { get; set; }   //成功

        public bool HasError { get; set; }  //失敗

        public List<MeetingData> Meetings { get; set; }


        public GetMeetingsParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.Meetings = new List<MeetingData>();
        }
    }
}
