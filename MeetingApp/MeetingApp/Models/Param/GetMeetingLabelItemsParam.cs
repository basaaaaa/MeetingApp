using MeetingApp.Models.Data;
using System;
using System.Collections.Generic;

namespace MeetingApp.Models.Param
{
    public class GetMeetingLabelItemsParam
    {
        public bool IsSuccessed { get; set; }   //ログイン処理が正常に成功したかどうか

        public bool HasError { get; set; }  //ログイン処理においてエラーが発生したかどうか

        public bool BlankLabelName { get; set; }
        public List<MeetingLabelItemData> MeetingLabelItemDatas;

        public GetMeetingLabelItemsParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.BlankLabelName = false;
            this.MeetingLabelItemDatas = new List<MeetingLabelItemData>();
        }

        public static implicit operator List<object>(GetMeetingLabelItemsParam v)
        {
            throw new NotImplementedException();
        }
    }
}
