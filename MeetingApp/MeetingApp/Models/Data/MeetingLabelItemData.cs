using Newtonsoft.Json;

namespace MeetingApp.Models.Data
{
    public class MeetingLabelItemData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("lid")]
        public int Lid { get; set; }

        [JsonProperty("uid")]
        public int Uid { get; set; }
        [JsonProperty("itemName")]
        public string ItemName { get; set; }




        public MeetingLabelItemData() { }

        public MeetingLabelItemData(int lid, string itemName)
        {
            this.Lid = lid;
            this.ItemName = itemName;
        }

        public MeetingLabelItemData(int lid, int uid, string itemName)
        {
            this.Lid = lid;
            this.Uid = uid;
            this.ItemName = itemName;
        }

    }
}
