using Newtonsoft.Json;
using System;

namespace MeetingApp.Data
{
    public class MeetingData
    {
        [JsonProperty("scheduledDate")]
        public DateTime ScheduledDate { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("regular")]
        public Boolean Regular { get; set; }

        [JsonProperty("owner")]
        public String Owner { get; set; }

        [JsonProperty("location")]
        public String Location { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

    }
}
