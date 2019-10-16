using Newtonsoft.Json;

namespace MeetingApp.Models.Data
{
    public class ParticipantData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("uid")]
        public int Uid { get; set; }

        [JsonProperty("mid")]
        public int Mid { get; set; }

        public ParticipantData() { }

        public ParticipantData(int uid, int mid)
        {
            this.Uid = uid;
            this.Mid = mid;
        }

    }
}
