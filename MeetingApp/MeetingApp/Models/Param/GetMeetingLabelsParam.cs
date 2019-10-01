using MeetingApp.Models.Data;
using System.Collections.Generic;

namespace MeetingApp.Models.Param
{
    public class GetMeetingLabelsParam
    {

        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか

        public bool BlankLabelName { get; set; }

        public List<MeetingLabelData> MeetingLabelDatas { get; set; }

        public GetMeetingLabelsParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.BlankLabelName = false;
            this.MeetingLabelDatas = null;
        }
    }
}
