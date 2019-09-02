using Newtonsoft.Json;
using System;

namespace MeetingApp.Models.Data
{
    public class TokenData
    {

        [JsonProperty("tokenText")]
        public String TokenText { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        public TokenData(string tokenText, DateTime startTime, DateTime endTime)
        {
            this.TokenText = tokenText;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }
    }
}
