using Newtonsoft.Json;

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

        public MeetingLabelData(string labelName)
        {
            this.LabelName = labelName;
        }

    }
}
