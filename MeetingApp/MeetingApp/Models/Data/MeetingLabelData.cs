using MeetingApp.Constants;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MeetingApp.Models.Data
{
    public class MeetingLabelData
    {
        RestService _restService;

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("mid")]
        public int Mid { get; set; }

        [JsonProperty("labelName")]
        public string LabelName { get; set; }
        public int MeetingLabelItemDatasCount { get; set; }
        public string MeetingLabelItemDatasCountString { get; set; }

        public List<MeetingLabelItemData> MeetingLabelItemDatas { get; set; }

        public MeetingLabelData()
        {
            this.MeetingLabelItemDatas = new List<MeetingLabelItemData>();
        }

        public MeetingLabelData(string labelName)
        {
            this.LabelName = labelName;
        }

        public MeetingLabelData(int mid, string labelName)
        {
            this.Mid = mid;
            this.LabelName = labelName;
            this.MeetingLabelItemDatas = new List<MeetingLabelItemData>();
        }
        public async System.Threading.Tasks.Task GetMyItemsAsync(int uid)
        {
            _restService = new RestService();


            var getMeetingLabelItemsParam = await _restService.GetMeetingLabelItemsDataAsync(MeetingConstants.OPENMeetingLabelItemEndPoint, this.Id, uid);
            this.MeetingLabelItemDatas = getMeetingLabelItemsParam.MeetingLabelItemDatas;
            this.MeetingLabelItemDatasCount = this.MeetingLabelItemDatas.Count;
            this.MeetingLabelItemDatasCountString = (this.MeetingLabelItemDatas.Count).ToString();
        }

    }
}
