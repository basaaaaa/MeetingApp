using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MeetingApp.Models.Data
{
    public class MeetingLabelData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("mid")]
        public int Mid { get; set; }

        [JsonProperty("labelName")]
        public string LabelName { get; set; }

        public ObservableCollection<MeetingLabelItemData> MeetingLabelItemDatas { get; set; }

        public MeetingLabelData()
        {
            this.MeetingLabelItemDatas = new ObservableCollection<MeetingLabelItemData>();
        }

        public MeetingLabelData(string labelName)
        {
            this.LabelName = labelName;
        }

        public MeetingLabelData(int mid, string labelName)
        {
            this.Mid = mid;
            this.LabelName = labelName;
            this.MeetingLabelItemDatas = new ObservableCollection<MeetingLabelItemData>();
        }

    }
}
